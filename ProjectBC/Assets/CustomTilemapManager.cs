using System.Collections.Generic;
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
        for (int i = 0; i < allCharacters.Length; i++)
        {
            if (allCharacters[i] != character && Vector3.Distance(allCharacters[i].transform.position, position) < PositionTolerance)
            {
                return true;
            }
        }
        return false;
    }

    public override List<Vector3> GetNeighbors(Vector3 position)
    {
        List<Vector3> neighbors = baseTilemapManager.GetNeighbors(position);
        List<Vector3> validNeighbors = new List<Vector3>();
        for (int i = 0; i < neighbors.Count; i++)
        {
            if (!IsObstacle(neighbors[i]))
            {
                validNeighbors.Add(neighbors[i]);
            }
        }
        return validNeighbors;
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
        List<Vector3> neighbors = GetNeighbors(nearest);
        Vector3 nearestNeighbor = nearest;
        float minDistance = float.MaxValue;
        for (int i = 0; i < neighbors.Count; i++)
        {
            float distance = Vector3.Distance(neighbors[i], position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestNeighbor = neighbors[i];
            }
        }
        return nearestNeighbor;
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
            var current = GetLowestFScoreNode(openSet, fScore);
            if (Vector3.Distance(current, goal) < 0.1f)
            {
                return ReconstructPath(cameFrom, current);
            }

            openSet.Remove(current);
            closedSet.Add(current);

            List<Vector3> neighbors = GetNeighbors(current);
            for (int i = 0; i < neighbors.Count; i++)
            {
                var neighbor = neighbors[i];
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

    private Vector3 GetLowestFScoreNode(List<Vector3> openSet, Dictionary<Vector3, float> fScore)
    {
        Vector3 lowestNode = openSet[0];
        float lowestFScore = float.MaxValue;
        for (int i = 0; i < openSet.Count; i++)
        {
            if (fScore.TryGetValue(openSet[i], out float score) && score < lowestFScore)
            {
                lowestFScore = score;
                lowestNode = openSet[i];
            }
        }
        return lowestNode;
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