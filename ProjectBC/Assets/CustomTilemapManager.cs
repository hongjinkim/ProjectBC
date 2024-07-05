using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CustomTilemapManager : TilemapManager
{
    private TilemapManager baseTilemapManager;
    private CharacterMovement character;

    public CustomTilemapManager(TilemapManager baseTilemapManager, CharacterMovement character)
    {
        this.baseTilemapManager = baseTilemapManager;
        this.character = character;
    }

    public override bool IsObstacle(Vector3 position)
    {
        return character.detection.detectedObj.Any(obj =>
            Vector3.Distance(obj.transform.position, position) < 0.5f);
    }

    public override List<Vector3> GetNeighbors(Vector3 position)
    {
        return baseTilemapManager.GetNeighbors(position).Where(p => !IsObstacle(p)).ToList();
    }

    public override bool IsValidMovePosition(Vector3 position)
    {
        return baseTilemapManager.IsValidMovePosition(position);
    }

    public override Vector3 GetNearestValidPosition(Vector3 position)
    {
        return baseTilemapManager.GetNearestValidPosition(position);
    }

    public override float GetDistance(Vector3 a, Vector3 b)
    {
        return baseTilemapManager.GetDistance(a, b);
    }

    public override List<Vector3> FindPath(Vector3 start, Vector3 goal)
    {
        return baseTilemapManager.FindPath(start, goal);
    }
}