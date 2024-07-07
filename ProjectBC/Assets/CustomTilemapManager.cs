using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CustomTilemapManager : TilemapManager
{
    private TilemapManager baseTilemapManager;
    private CharacterMovement character;
    private const float PositionTolerance = 0.1f;
    public CustomTilemapManager(TilemapManager baseTilemapManager, CharacterMovement character)
    {
        this.baseTilemapManager = baseTilemapManager;
        this.character = character;
    }
    public override bool IsObstacle(Vector3 position)
    {
        CharacterMovement[] allCharacters = Object.FindObjectsOfType<CharacterMovement>();
        return allCharacters.Any(c => c != character && Vector3.Distance(c.transform.position, position) < PositionTolerance);
    }
    public override List<Vector3> GetNeighbors(Vector3 position)
    {
        return baseTilemapManager.GetNeighbors(position)
            .Where(p => !IsObstacle(p))
            .ToList();
    }
    public override bool IsValidMovePosition(Vector3 position)
    {
        Vector3 nearestValidPosition = GetNearestValidPosition(position);
        return Vector3.Distance(position, nearestValidPosition) < PositionTolerance;
    }
    public override Vector3 GetNearestValidPosition(Vector3 position)
    {
        Vector3 nearest = baseTilemapManager.GetNearestValidPosition(position);
        if (!IsObstacle(nearest))
        {
            return nearest;
        }
        return GetNeighbors(nearest).OrderBy(p => Vector3.Distance(p, position)).FirstOrDefault();
    }
    public override float GetDistance(Vector3 a, Vector3 b)
    {
        return Vector3.Distance(a, b);
    }
    public override List<Vector3> FindPath(Vector3 start, Vector3 goal)
    {
        var openSet = new List<Vector3> { start };
        var closedSet = new HashSet<Vector3>();
        var cameFrom = new Dictionary<Vector3, Vector3>();
        var gScore = new Dictionary<Vector3, float> { { start, 0 } };
        var fScore = new Dictionary<Vector3, float> { { start, GetDistance(start, goal) } };

        while (openSet.Count > 0)
        {
            var current = openSet.OrderBy(p => fScore.GetValueOrDefault(p, float.MaxValue)).First();

            if (Vector3.Distance(current, goal) < 0.1f)
            {
                return ReconstructPath(cameFrom, current);
            }
            openSet.Remove(current);
            closedSet.Add(current);
            foreach (var neighbor in GetNeighbors(current))
            {
                if (closedSet.Contains(neighbor)) continue;

                var tentativeGScore = gScore[current] + GetDistance(current, neighbor);

                if (!openSet.Contains(neighbor))
                {
                    openSet.Add(neighbor);
                }
                else if (tentativeGScore >= gScore.GetValueOrDefault(neighbor, float.MaxValue))
                {
                    continue;
                }

                cameFrom[neighbor] = current;
                gScore[neighbor] = tentativeGScore;
                fScore[neighbor] = gScore[neighbor] + GetDistance(neighbor, goal);
            }
        }
        return null;
    }

    private List<Vector3> ReconstructPath(Dictionary<Vector3, Vector3> cameFrom, Vector3 current)
    {
        var path = new List<Vector3> { current };
        while (cameFrom.ContainsKey(current))
        {
            current = cameFrom[current];
            path.Add(current);
        }
        path.Reverse();
        return path;
    }

}