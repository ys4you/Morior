using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class MouseController : MonoBehaviour
{
    public float speed;
    public CharacterManager character;
    public GameObject characterPrefab;
    public int movementRange = 3;
    private PathFinder _pathFinder;
    private RangeFinder _rangeFinder;



    private List<OverlayTile> _path = new();
    private List<OverlayTile> _inRangeTiles = new List<OverlayTile>();
    private void Start()
    {
        _pathFinder = new PathFinder();
        _rangeFinder = new RangeFinder();
    }

    private void Update()
    {
        var focusedTileHit = GetFocusedOnTile();

        if (focusedTileHit.HasValue)
        {
            GameObject overlayTile = focusedTileHit.Value.collider.gameObject;
            this.transform.position = overlayTile.transform.position;
            gameObject.GetComponent<SpriteRenderer>().sortingOrder = overlayTile.GetComponent<SpriteRenderer>().sortingOrder;

            foreach (var character in MapManager.Instance.characters)
            {
                if (overlayTile.GetComponent<OverlayTile>() == character.activeTile)
                {
                    Debug.Log($"found Character: {character.characterInfo.characterName}");
                }
            }

            if (Input.GetMouseButtonDown(0))
            {
                if(character != null)
                {
                    var searchableTiles = _rangeFinder.GetTilesInRange(character.activeTile, movementRange);
                    _path = _pathFinder.FindPath(character.activeTile, overlayTile.GetComponent<OverlayTile>(), searchableTiles);
                }
            }
        }

        if (_path.Count > 0)
        {
            MoveAlongPath();
        }
    }
    private void GetInRangeTiles()
    {
        foreach (var item in _inRangeTiles)
        {
            item.HideTile();
        }
        
        _inRangeTiles = _rangeFinder.GetTilesInRange(character.activeTile, movementRange);

        foreach (var item in _inRangeTiles)
        {
            item.ShowTile();
        }
    }
    private void MoveAlongPath()
    {
        var step = speed * Time.deltaTime;

        var zIndex = _path[0].transform.position.z;
        character.transform.position = Vector2.MoveTowards(character.transform.position, _path[0].transform.position, step);
        character.transform.position = new Vector3(character.transform.position.x, character.transform.position.y, zIndex);

        if (Vector2.Distance(character.transform.position, _path[0].transform.position) < 0.0001f)
        {
            MapManager.Instance.PositionCharacterOnTile(_path[0], character);
            _path.RemoveAt(0);
        }

        if (_path.Count == 0)
        {
            GetInRangeTiles();
        }
    }
    

    public RaycastHit2D? GetFocusedOnTile()
    {
        Vector3 mousepos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos2d = new Vector2(mousepos.x, mousepos.y);

        RaycastHit2D[] hits = Physics2D.RaycastAll(mousePos2d, Vector2.zero);

        if (hits.Length>0)
        {
            return hits.OrderByDescending(i => i.collider.transform.position.z).First();
        }
        return null;
    }
}
