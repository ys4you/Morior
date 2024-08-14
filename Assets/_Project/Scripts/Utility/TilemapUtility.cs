using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapUtility : Singleton<TilemapUtility>
{
    protected override void Awake()
    {
        base.Awake();
    }

    public Vector3Int GetGridPos(Transform location, Tilemap tilemap)
    {
        return tilemap.WorldToCell(location.position);
    }
}
