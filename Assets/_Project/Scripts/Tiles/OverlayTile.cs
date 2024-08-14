using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class OverlayTile : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;

    public int G;
    public int H;
    public int F
    {
        get { return G + H; }
    }

    public bool isBlocked;

    public OverlayTile previous;

    public Vector3Int gridLocation;
    public Vector2Int grid2DLocation => new Vector2Int(gridLocation.x, gridLocation.y);

    public void ShowTile()
    {
        if (_spriteRenderer == null)
        {
            _spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        }

        _spriteRenderer.color = new Color(1, 1, 1, 1);
    }
    
    public void HideTile()
    {
        if (_spriteRenderer == null)
        {
            _spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        }

        _spriteRenderer.color = new Color(1, 1, 1, 0);
    }
}
