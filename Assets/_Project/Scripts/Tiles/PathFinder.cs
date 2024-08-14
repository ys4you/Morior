using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PathFinder
{
    public List<OverlayTile> FindPath(OverlayTile start, OverlayTile end, List<OverlayTile> searchableTiles)
    {
        List<OverlayTile> openList = new List<OverlayTile>();
        List<OverlayTile> closedList = new List<OverlayTile>();
        
        openList.Add(start);

        while (openList.Count > 0)
        {
            OverlayTile currentOverlayTile = openList.OrderBy(x => x.F).First();

            openList.Remove(currentOverlayTile);
            closedList.Add(currentOverlayTile);

            if (currentOverlayTile == end)
            {
                return GetFinishedList(start, end);
            }

            var neighborTiles = MapManager.Instance.GetNeighborTiles(currentOverlayTile, searchableTiles);

            foreach (var neighbor in neighborTiles)
            {
                if (neighbor.isBlocked || closedList.Contains(neighbor))
                {
                    continue;
                }

                neighbor.G = GetManhattenDistence(start, neighbor);
                neighbor.H = GetManhattenDistence(end, neighbor);

                neighbor.previous = currentOverlayTile;

                if (!openList.Contains(neighbor))
                {
                    openList.Add(neighbor);
                }
            }
        }

        return new List<OverlayTile>();
    }

    private List<OverlayTile> GetFinishedList(OverlayTile start, OverlayTile end)
    {
        List<OverlayTile> finishedList = new List<OverlayTile>();

        OverlayTile currentTile = end;

        while (currentTile != start)
        {
            finishedList.Add(currentTile);
            currentTile = currentTile.previous;
        }

        finishedList.Reverse();

        return finishedList;
    }

    private int GetManhattenDistence(OverlayTile start, OverlayTile neighbour)
    {
        return Mathf.Abs(start.gridLocation.x - neighbour.gridLocation.x) + Mathf.Abs(start.gridLocation.y - neighbour.gridLocation.y);
    }

}
