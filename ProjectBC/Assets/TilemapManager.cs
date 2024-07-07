using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class TilemapManager : MonoBehaviour
{
    private static TilemapManager _Instance;
    public static TilemapManager Instance
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = FindObjectOfType<TilemapManager>();
            }
            return _Instance;
        }
    }
    public Tilemap tilemap;
    private List<Vector3> tileCenters = new List<Vector3>();
    public List<Vector3> currentPath; // 새로 추가된 필드

    private void Awake()
    {
        if (_Instance == null)
        {
            _Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (_Instance != this)
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        if (tilemap != null)
        {
            CalculateTileCenters();
        }
    }
    void CalculateTileCenters()
    {
        BoundsInt bounds = tilemap.cellBounds;

        for (int x = bounds.min.x; x < bounds.max.x; x++)
        {
            for (int y = bounds.min.y; y < bounds.max.y; y++)
            {
                Vector3Int tilePosition = new Vector3Int(x, y, 0);

                if (tilemap.HasTile(tilePosition))
                {
                    Vector3 centerPosition = tilemap.GetCellCenterWorld(tilePosition);
                    centerPosition.z = 0;
                    tileCenters.Add(centerPosition);
                }
            }
        }
    }
    public virtual bool IsValidMovePosition(Vector3 position)
    {
        return tileCenters.Contains(position);
    }

    public virtual Vector3 GetNearestValidPosition(Vector3 position)
    {
        Vector3 nearest = tileCenters.OrderBy(p => Vector3.Distance(p, position)).FirstOrDefault();
        if (IsValidMovePosition(position) && Vector3.Distance(position, nearest) < 0.1f)
        {
            return position;
        }
        return nearest;
    }
    public virtual bool IsObstacle(Vector3 position)
    {
        return false;
    }
    public virtual List<Vector3> GetNeighbors(Vector3 position)
    {
        return tileCenters.Where(p =>
            Vector3.Distance(p, position) <= 1.5f &&
            !IsObstacle(p)
        ).ToList();
    }
    public virtual float GetDistance(Vector3 a, Vector3 b)
    {
        float dx = Mathf.Abs(a.x - b.x);
        float dy = Mathf.Abs(a.y - b.y);

        return Mathf.Max(dx, dy) + (Mathf.Sqrt(2) - 1) * Mathf.Min(dx, dy);
    }
    public virtual List<Vector3> FindPath(Vector3 start, Vector3 goal)
    {
        var openSet = new List<Vector3> { start };
        var closedSet = new List<Vector3>();
        var cameFrom = new Dictionary<Vector3, Vector3>();
        var gScore = new Dictionary<Vector3, float> { { start, 0 } };
        var fScore = new Dictionary<Vector3, float> { { start, GetDistance(start, goal) } };

        while (openSet.Count > 0)
        {
            var current = openSet.OrderBy(p => fScore.GetValueOrDefault(p, float.MaxValue)).First();

            if (current == goal || IsObstacle(goal))
            {
                currentPath = ReconstructPath(cameFrom, current); 
                return currentPath;
            }

            openSet.Remove(current);
            closedSet.Add(current);

            foreach (var neighbor in GetNeighbors(current))
            {
                if (closedSet.Contains(neighbor)) continue;

                var tentativeGScore = gScore[current] + GetDistance(current, neighbor);

                if (!openSet.Contains(neighbor)) openSet.Add(neighbor);
                else if (tentativeGScore >= gScore.GetValueOrDefault(neighbor, float.MaxValue)) continue;

                if (current == goal || IsObstacle(goal))
                {
                    currentPath = ReconstructPath(cameFrom, current);
                    return currentPath;
                }

                cameFrom[neighbor] = current;
                gScore[neighbor] = tentativeGScore;
                fScore[neighbor] = gScore[neighbor] + GetDistance(neighbor, goal);
            }
        }
        return null;
    }
    private Vector3 RoundToHalf(Vector3 v)
    {
        return new Vector3(
            Mathf.Round(v.x * 2) / 2,
            Mathf.Round(v.y * 2) / 2,
            v.z
        );
    }
    private List<Vector3> ReconstructPath(Dictionary<Vector3, Vector3> cameFrom, Vector3 current)
    {
        var path = new List<Vector3> { RoundToHalf(current) };
        while (cameFrom.ContainsKey(current))
        {
            current = cameFrom[current];
            path.Add(RoundToHalf(current));
        }
        path.Reverse();
        return path;
    }
    private void OnDrawGizmos()
    {
        if (currentPath != null && currentPath.Count > 0)
        {
            Gizmos.color = Color.yellow;
            for (int i = 0; i < currentPath.Count - 1; i++)
            {
                Gizmos.DrawLine(currentPath[i], currentPath[i + 1]);
                Gizmos.DrawSphere(currentPath[i], 0.1f);
            }
            Gizmos.DrawSphere(currentPath[currentPath.Count - 1], 0.1f);
        }
    }

    public void SetDebugPath(List<Vector3> debugPath)
    {
        currentPath = debugPath;
    }
}