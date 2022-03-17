using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridTile
{
    public Vector2Int coord;
    public bool walkable = true;
    public int gCost;
    public int hCost;
    public GridTile parent;


    public GridTile(Vector2Int position)
    {
        this.coord = position;
    }
    public int fCost
    {
        get
        {
            return gCost + hCost;
        }
    }
    public void ResetPathfinding()
    {
        gCost = 0;
        hCost = 0;
        parent = null;
    }
}
