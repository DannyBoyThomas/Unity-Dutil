using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Dutil
{
    public class GridNetwork
    {

        public GridTile[,] tiles;
        Vector2Int size;

        public GridNetwork(Vector2Int size, System.Func<Vector2Int, bool> isWalkable)
        {
            this.size = size;
            tiles = new GridTile[size.x, size.y];
            for (int x = 0; x < size.x; x++)
            {
                for (int y = 0; y < size.y; y++)
                {
                    tiles[x, y] = new GridTile(new Vector2Int(x, y));
                    tiles[x, y].walkable = isWalkable(new Vector2Int(x, y));
                }
            }
        }
        public bool IsInBounds(int x, int y)
        {
            return x >= 0 && x < size.x && y >= 0 && y < size.y;
        }
        public GridTile Get(int x, int y)
        {
            if (!IsInBounds(x, y))
            {
                return null;
            }
            return tiles[x, y];
        }
        public void UpdateWalkable(int x, int y, bool walkable)
        {
            GridTile tile = Get(x, y);
            if (tile != null)
            {
                tile.walkable = walkable;
            }
        }
        public void ResetPathfinding()
        {
            for (int x = 0; x < size.x; x++)
            {
                for (int y = 0; y < size.y; y++)
                {
                    tiles[x, y].ResetPathfinding();
                }
            }
        }
        public void Draw()
        {
            Gizmos.color = Color.white;
            for (int x = 0; x < size.x; x++)
            {
                for (int y = 0; y < size.y; y++)
                {
                    GridTile tile = Get(x, y);
                    bool walkable = tile.walkable;
                    Gizmos.color = walkable ? Color.white : Color.black;
                    Gizmos.DrawCube(new Vector3(x, y, 0), Vector3.one * .8f);
                }
            }
        }
    }
}
