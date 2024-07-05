using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using System.Linq;

public class TilemapManager : MonoBehaviour
{
    public Tilemap tilemap;
    private List<Vector3> tileCenters = new List<Vector3>(); // 타일맵들의 중앙값 저장
    public List<Vector3> obstacles = new List<Vector3>(); // 장애물 위치 저장

    void Start()
    {
        if (tilemap == null)
        {
            tilemap = GetComponent<Tilemap>();
        }

        if (tilemap != null)
        {
            CalculateTileCenters();
        }
    }

    void CalculateTileCenters() // 각타일의 중앙값을 계산 > tileCenters 리스트에 저장
    {
        BoundsInt bounds = tilemap.cellBounds;

        for (int x = bounds.min.x; x < bounds.max.x; x++)
        {
            for (int y = bounds.min.y; y < bounds.max.y; y++)
            {
                Vector3Int tilePosition = new Vector3Int(x, y, 0);

                if (tilemap.HasTile(tilePosition)) // HasTile이 해당 위치에 타일이 있는지 확인
                {
                    Vector3 centerPosition = tilemap.GetCellCenterWorld(tilePosition);
                    centerPosition.z = 0; // 2D 환경에서는 z 좌표를 0으로 설정
                    tileCenters.Add(centerPosition);

                    // 2번째 타일의 좌표를 출력
                    if (tileCenters.Count == 2)
                    {
                        Debug.Log($"Coordinates of the 2nd tile: {centerPosition}");
                    }
                }
            }
        }
    }

    //// 타일 중심점 목록을 반환하는 메서드
    //public List<Vector3> GetTileCenters()
    //{
    //    return tileCenters;
    //}
    //// 예: 가장 가까운 타일 중심점 찾기
    //public Vector3 GetNearestTileCenter(Vector3 position)
    //{
    //    Vector3 nearest = Vector3.zero;
    //    float minDistance = float.MaxValue;
    //    foreach (Vector3 center in tileCenters)
    //    {
    //        float distance = Vector3.Distance(position, center);
    //        if (distance < minDistance)
    //        {
    //            minDistance = distance;
    //            nearest = center;
    //        }
    //    }
    //    return nearest;
    //}

    public bool IsValidMovePosition(Vector3 position)
    {
        return tileCenters.Contains(position);
    }

    public Vector3 GetNearestValidPosition(Vector3 position)
    {
        return tileCenters.OrderBy(p => Vector3.Distance(p, position)).FirstOrDefault();
    }

    public bool IsObstacle(Vector3 position)
    {
        return obstacles.Contains(position);
    }

    public List<Vector3> GetNeighbors(Vector3 position)
    {
        return tileCenters.Where(p =>
            Vector3.Distance(p, position) < 1.1f && // 인접한 타일만 (대각선 포함)
            !IsObstacle(p) // 장애물이 아닌 경우
        ).ToList();
    }

    public float GetDistance(Vector3 a, Vector3 b)
    {
        return Vector3.Distance(a, b);
    }

    public List<Vector3> FindPath(Vector3 start, Vector3 goal)
    {
        var openSet = new List<Vector3> { start };
        var closedSet = new List<Vector3>();
        var cameFrom = new Dictionary<Vector3, Vector3>();
        var gScore = new Dictionary<Vector3, float> { { start, 0 } };
        var fScore = new Dictionary<Vector3, float> { { start, GetDistance(start, goal) } };

        while (openSet.Count > 0)
        {
            var current = openSet.OrderBy(p => fScore.GetValueOrDefault(p, float.MaxValue)).First();

            if (current == goal)
            {
                return ReconstructPath(cameFrom, current);
            }

            openSet.Remove(current);
            closedSet.Add(current);

            foreach (var neighbor in GetNeighbors(current))
            {
                if (closedSet.Contains(neighbor)) continue;

                var tentativeGScore = gScore[current] + GetDistance(current, neighbor);

                if (!openSet.Contains(neighbor)) openSet.Add(neighbor);
                else if (tentativeGScore >= gScore.GetValueOrDefault(neighbor, float.MaxValue)) continue;

                cameFrom[neighbor] = current;
                gScore[neighbor] = tentativeGScore;
                fScore[neighbor] = gScore[neighbor] + GetDistance(neighbor, goal);
            }
        }

        return null; // 경로를 찾지 못함
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

    public void MoveObjectToNearestValidPosition(Transform objectTransform, Vector3 targetPosition)
    {
        Vector3 nearestValidPosition = GetNearestValidPosition(targetPosition);

        if (IsValidMovePosition(nearestValidPosition))
        {
            objectTransform.position = nearestValidPosition;
        }
    }

}