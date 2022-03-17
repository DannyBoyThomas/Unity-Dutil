using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Dutil
{
    public class PathFinder
    {
        public static List<GridTile> FindPath(GridNetwork grid, Vector2Int start, Vector2Int end)
        {
            GridTile startNode = grid.Get(start.x, start.y);
            GridTile endNode = grid.Get(end.x, end.y);
            if (startNode == null || endNode == null)
            {
                return new List<GridTile>();
            }

            return FindPath(grid, startNode, endNode);
        }
        public static List<GridTile> FindPath(GridNetwork grid, GridTile startNode, GridTile targetNode)
        {
            grid.ResetPathfinding();
            List<GridTile> empty = new List<GridTile>();

            List<GridTile> openSet = new List<GridTile>();
            HashSet<GridTile> closedSet = new HashSet<GridTile>();
            openSet.Add(startNode);

            while (openSet.Count > 0)
            {
                GridTile node = openSet[0];
                for (int i = 1; i < openSet.Count; i++)
                {
                    if (openSet[i].fCost < node.fCost || openSet[i].fCost == node.fCost)
                    {
                        if (openSet[i].hCost < node.hCost)
                            node = openSet[i];
                    }
                }

                openSet.Remove(node);
                closedSet.Add(node);

                if (node == targetNode)
                {
                    return RetracePath(startNode, targetNode);
                }

                foreach (GridTile neighbour in GetNeighbours(grid, node))
                {
                    if (!neighbour.walkable || closedSet.Contains(neighbour))
                    {
                        continue;
                    }

                    int newCostToNeighbour = node.gCost + GetDistance(node, neighbour);
                    if (newCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                    {
                        neighbour.gCost = newCostToNeighbour;
                        neighbour.hCost = GetDistance(neighbour, targetNode);
                        neighbour.parent = node;

                        if (!openSet.Contains(neighbour))
                            openSet.Add(neighbour);
                    }
                }
            }
            return empty;
        }
        static List<GridTile> RetracePath(GridTile startNode, GridTile endNode)
        {
            List<GridTile> path = new List<GridTile>();
            GridTile currentNode = endNode;

            while (currentNode != startNode)
            {
                path.Add(currentNode);
                currentNode = currentNode.parent;
            }
            path.Add(startNode);
            path.Reverse();

            return path;

        }

        static int GetDistance(GridTile nodeA, GridTile nodeB)
        {

            int dstX = Mathf.Abs(nodeA.coord.x - nodeB.coord.x);
            int dstY = Mathf.Abs(nodeA.coord.y - nodeB.coord.y);

            return dstX + dstY;
            /* if (dstX > dstY)
                return 14 * dstY + 10 * (dstX - dstY);
            return 14 * dstX + 10 * (dstY - dstX); */
        }
        static public List<GridTile> GetNeighbours(GridNetwork grid, GridTile node)
        {
            List<GridTile> neighbours = new List<GridTile>();

            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    if (x == 0 && y == 0)
                        continue;

                    if (Mathf.Abs(x) > 0 && Mathf.Abs(y) > 0)
                    {
                        continue;
                    }

                    int checkX = node.coord.x + x;
                    int checkY = node.coord.y + y;

                    if (grid.IsInBounds(checkX, checkY))
                    {
                        neighbours.Add(grid.Get(checkX, checkY));
                    }

                }
            }

            return neighbours;
        }
    }
}