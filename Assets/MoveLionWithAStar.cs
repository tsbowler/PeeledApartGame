using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MoveLionWithAStar : MonoBehaviour
{
    public float moveSpeed = 1f;
    public Tilemap obstaclesTilemap; // Reference to the obstacle tilemap
    public Transform monkeyTransform; // Reference to the monkey's position

    private List<Vector2> currentPath;
    private int currentPathIndex;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentPath = new List<Vector2>();
    }

    void Update()
    {
        if (currentPath == null || currentPath.Count == 0 || currentPathIndex >= currentPath.Count)
        {
            // Find a path to the Monkey if we don't have one or reached the end of the current path
            currentPath = FindPath(transform.position, monkeyTransform.position);
            currentPathIndex = 0;
        }

        if (currentPath != null && currentPathIndex < currentPath.Count)
        {
            Vector2 targetPosition = currentPath[currentPathIndex];
            rb.MovePosition(Vector2.MoveTowards(rb.position, targetPosition, moveSpeed * Time.deltaTime));

            // If reached the target position, move to the next point in the path
            if ((Vector2)transform.position == targetPosition)
            {
                currentPathIndex++;
            }
        }
    }

    // A* Pathfinding method
    List<Vector2> FindPath(Vector2 startPos, Vector2 targetPos)
    {
        Vector3Int startCell = obstaclesTilemap.WorldToCell(startPos);
        Vector3Int targetCell = obstaclesTilemap.WorldToCell(targetPos);

        List<Node> openList = new List<Node>();
        HashSet<Node> closedList = new HashSet<Node>();

        Node startNode = new Node(startCell, null, 0, GetDistance(startCell, targetCell));
        openList.Add(startNode);

        while (openList.Count > 0)
        {
            Node currentNode = GetNodeWithLowestFScore(openList);
            openList.Remove(currentNode);
            closedList.Add(currentNode);

            if (currentNode.Position == targetCell)
            {
                return ReconstructPath(currentNode);
            }

            foreach (Vector3Int neighbor in GetNeighbors(currentNode.Position))
            {
                if (closedList.Contains(new Node(neighbor))) continue;

                float gScore = currentNode.G + GetDistance(currentNode.Position, neighbor);
                Node neighborNode = new Node(neighbor, currentNode, gScore, GetDistance(neighbor, targetCell));

                if (!openList.Contains(neighborNode) || gScore < neighborNode.G)
                {
                    openList.Add(neighborNode);
                }
            }
        }

        return null; // No path found
    }

    // Reconstructs the path by going backward from the target node to the start node
    List<Vector2> ReconstructPath(Node node)
    {
        List<Vector2> path = new List<Vector2>();
        Node currentNode = node;

        while (currentNode != null)
        {
            path.Add(obstaclesTilemap.CellToWorld(currentNode.Position) + new Vector3(0.5f, 0.5f, 0f)); // Center the position on the tile
            currentNode = currentNode.Parent;
        }

        path.Reverse(); // Reverse the path so it goes from start to target
        return path;
    }

    // Finds the node with the lowest F score in the open list
    Node GetNodeWithLowestFScore(List<Node> openList)
    {
        Node bestNode = openList[0];

        foreach (Node node in openList)
        {
            if (node.F < bestNode.F)
            {
                bestNode = node;
            }
        }

        return bestNode;
    }

    // Gets the neighboring tiles of a given position
    List<Vector3Int> GetNeighbors(Vector3Int cellPosition)
    {
        List<Vector3Int> neighbors = new List<Vector3Int>();

        Vector3Int[] directions = {
            Vector3Int.up,
            Vector3Int.down,
            Vector3Int.left,
            Vector3Int.right
        };

        foreach (Vector3Int direction in directions)
        {
            Vector3Int neighborPos = cellPosition + direction;

            if (!IsObstacleAtPosition(neighborPos))
            {
                neighbors.Add(neighborPos);
            }
        }

        return neighbors;
    }

    // Checks if a tile is an obstacle
    bool IsObstacleAtPosition(Vector3Int position)
    {
        TileBase tile = obstaclesTilemap.GetTile(position);
        return tile != null;
    }

    // Heuristic: Returns the distance between two points (Manhattan distance for grid-based maps)
    float GetDistance(Vector3Int a, Vector3Int b)
    {
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
    }

    // Node class to store pathfinding data
    class Node
    {
        public Vector3Int Position;
        public Node Parent;
        public float G; // Cost from the start node to this node
        public float H; // Heuristic cost to the target node

        public float F => G + H; // Total cost

        public Node(Vector3Int position, Node parent = null, float g = 0, float h = 0)
        {
            Position = position;
            Parent = parent;
            G = g;
            H = h;
        }

        public override bool Equals(object obj)
        {
            return obj is Node node && Position == node.Position;
        }

        public override int GetHashCode()
        {
            return Position.GetHashCode();
        }
    }
}
