using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Dutil
{
    public class GridNetwork
    {

        public GridTile[,] tiles;
        Vector2Int size;
        public GridNetwork(Vector2Int size)
        {
            this.size = size;
            tiles = new GridTile[size.x, size.y];
            for (int x = 0; x < size.x; x++)
            {
                for (int y = 0; y < size.y; y++)
                {
                    tiles[x, y] = new GridTile(new Vector2Int(x, y));
                }
            }
        }
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
            Draw(Vector3.zero);
        }
        public void Draw(Vector3 offset, float tileSize = 1)
        {
            Gizmos.color = Color.white;
            for (int x = 0; x < size.x; x++)
            {
                float xPos = x * tileSize + offset.x;
                for (int y = 0; y < size.y; y++)
                {
                    float yPos = y * tileSize + offset.y;
                    GridTile tile = Get(x, y);
                    bool walkable = tile.walkable;
                    Gizmos.color = walkable ? Color.white : Color.black;
                    Gizmos.DrawCube(new Vector3(xPos, yPos, 0), Vector3.one * .8f * tileSize);
                }
            }

        }
        public List<GridTile> FindPath(Vector2Int startCoord, Vector2Int endCoord)
        {
            return PathFinder.FindPath(this, startCoord, endCoord);
        }
        public List<GridTile> FindPath(GridTile startTile, GridTile endTile)
        {
            return PathFinder.FindPath(this, startTile, endTile);
        }
    }
}
