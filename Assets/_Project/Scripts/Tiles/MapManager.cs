using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class MapManager : MonoBehaviour
{
    public OverlayTile overlayTilePrefab;
    public GameObject overlayTileContainer;
    public List<CharacterManager> characters = new List<CharacterManager>();

    private static MapManager _instance;
    public static MapManager Instance => _instance;

    public Dictionary<Vector2Int, OverlayTile> Map;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    private void Start()
    {
        Map = new Dictionary<Vector2Int, OverlayTile>();
        CreateOverlayTiles();
        PlacePlayersOnGrid();
    }
    private void CreateOverlayTiles()
    {
        var tileMap = GetComponentInChildren<Tilemap>();
        
        BoundsInt bounds = tileMap.cellBounds;

        Debug.Log($"z MIN: {bounds.min.z}, z MAX: {bounds.max.z}");
        for (int z = bounds.max.z; z >= bounds.min.z ; z--)
        {
            for (int y = bounds.min.y; y < bounds.max.y; y++)
            {
                for (int x = bounds.min.x; x < bounds.max.x; x++)
                {
                    //Debug.Log($"looped thru x:{x}, y:{y}, z:{z}");
                    var tileLocation = new Vector3Int(x, y, z);
                    var tileKey = new Vector2Int(x, y);
                    if (tileMap.HasTile(tileLocation)&& !Map.ContainsKey(tileKey))
                    {
                        var overlayTile = Instantiate(overlayTilePrefab.gameObject, overlayTileContainer.transform);
                        var cellWorldPosition = tileMap.GetCellCenterWorld(tileLocation);

                        overlayTile.transform.position = new Vector3(cellWorldPosition.x, cellWorldPosition.y +0.25f, cellWorldPosition.z+0.25f);
                        //Debug.Log("instantiated on: " + overlayTile.transform.position);
                        overlayTile.GetComponent<SpriteRenderer>().sortingOrder = tileMap.GetComponent<TilemapRenderer>().sortingOrder;
                        var OverlayTileComponent = overlayTile.GetComponent<OverlayTile>();
                        OverlayTileComponent.gridLocation = tileLocation;
                        Map.Add(tileKey, OverlayTileComponent);
                    }
                    else
                    {
                        //Debug.LogWarning("NO tile found on: "+ tileLocation);
                    }
                }
            }
        }
    }

    public List<OverlayTile> GetNeighborTiles(OverlayTile currentOverlayTile, List<OverlayTile> searchableTiles)
    {
        Dictionary<Vector2Int, OverlayTile> tilesToSearch = new Dictionary<Vector2Int, OverlayTile>();
         
        if (searchableTiles.Count > 0)
        {
            foreach (var item in searchableTiles)
            {
                tilesToSearch.Add(item.grid2DLocation, item);
            }
        }
        else
        {
            tilesToSearch = Map;
        }


        List<OverlayTile> neighbors = new List<OverlayTile>();

        //top
        Vector2Int locationToCheck = new Vector2Int(currentOverlayTile.gridLocation.x, currentOverlayTile.gridLocation.y + 1);

        if (tilesToSearch.ContainsKey(locationToCheck))
        {
            if (Mathf.Abs(currentOverlayTile.gridLocation.z - tilesToSearch[locationToCheck].gridLocation.z) <= 1)
                neighbors.Add(tilesToSearch[locationToCheck]);
        }

        //bottom
        locationToCheck = new Vector2Int(currentOverlayTile.gridLocation.x, currentOverlayTile.gridLocation.y - 1);

        if (tilesToSearch.ContainsKey(locationToCheck))
        {
            if (Mathf.Abs(currentOverlayTile.gridLocation.z - tilesToSearch[locationToCheck].gridLocation.z) <= 1)
                neighbors.Add(tilesToSearch[locationToCheck]);
        }

        //right
        locationToCheck = new Vector2Int(currentOverlayTile.gridLocation.x + 1, currentOverlayTile.gridLocation.y);

        if (tilesToSearch.ContainsKey(locationToCheck))
        {
            if (Mathf.Abs(currentOverlayTile.gridLocation.z - tilesToSearch[locationToCheck].gridLocation.z) <= 1)
                neighbors.Add(tilesToSearch[locationToCheck]);
        }

        //left
        locationToCheck = new Vector2Int(currentOverlayTile.gridLocation.x - 1, currentOverlayTile.gridLocation.y);

        if (tilesToSearch.ContainsKey(locationToCheck))
        {
            if (Mathf.Abs(currentOverlayTile.gridLocation.z - tilesToSearch[locationToCheck].gridLocation.z) <= 1)
                neighbors.Add(tilesToSearch[locationToCheck]);
        }

        return neighbors;
    }
    
    public void PositionCharacterOnTile(OverlayTile tile, CharacterManager character)
    {
        character.transform.position = new Vector3(tile.transform.position.x, tile.transform.position.y + 0.0001f, tile.transform.position.z);
        character.GetComponent<SpriteRenderer>().sortingOrder = tile.GetComponent<SpriteRenderer>().sortingOrder;
        character.activeTile = tile;
    }
    
    public OverlayTile GetRandomOverlayTile()
    {
        List<Vector2Int> keys = new List<Vector2Int>(Map.Keys);
        if (keys.Count == 0)
        {
            return null; 
        }

        int randomIndex = Random.Range(0, keys.Count);
        Vector2Int randomKey = keys[randomIndex];
        return Map[randomKey];
    }

    private void PlacePlayersOnGrid()
    {
        foreach (var character in characters)
        {
            if (character == null)
            {
                return;
            }
            
            Instantiate(character.characterInfo.characterPrefab).GetComponent<CharacterManager>();
                
            PositionCharacterOnTile(GetRandomOverlayTile(), character);
        }
    }

}
