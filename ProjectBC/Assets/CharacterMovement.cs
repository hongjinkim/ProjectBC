using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    private List<Vector3> path;
    private int currentPathIndex;
    private bool isSelected = false;
    private static CharacterMovement selectedCharacter = null;
    public Detection detection;
    public bool autoMove = false;
    public float updatePathInterval = 1f;
    private float lastPathUpdateTime = 0f;

    private CustomTilemapManager customTilemapManager;
    private const float PositionTolerance = 0.1f;
    private const float PositionToleranceSquared = PositionTolerance * PositionTolerance;

    private Vector3 targetPosition;
    private Vector3 currentPosition;
    private Vector3 nearestValidPosition;

    private void Awake()
    {
        detection = GetComponent<Detection>();
    }

    void Start()
    {
        customTilemapManager = new CustomTilemapManager(TilemapManager.Instance, this);
        transform.position = customTilemapManager.GetNearestValidPosition(transform.position);
        StartCoroutine(AutoMoveCoroutine());
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            HandleSelectionAndMovement();
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            autoMove = !autoMove;
        }
        MoveAlongPath();
        if (path == null || path.Count == 0)
        {
            SnapToNearestTileCenter();
        }
    }

    void UpdatePath()
    {
        if (detection != null)
        {
            GameObject closestObject = detection.GetClosestObject();
            if (closestObject != null)
            {
                targetPosition = closestObject.transform.position;
                currentPosition = customTilemapManager.GetNearestValidPosition(transform.position);
                if ((currentPosition - targetPosition).sqrMagnitude > PositionToleranceSquared)
                {
                    List<Vector3> surroundingPositions = GetSurroundingPositions(targetPosition);
                    Vector3? bestPosition = FindBestPosition(surroundingPositions, currentPosition);

                    if (bestPosition.HasValue)
                    {
                        SetNewPath(bestPosition.Value);
                    }
                    else
                    {
                        path = null;
                    }
                }
                else
                {
                    transform.position = currentPosition;
                    path = null;
                }
            }
        }
        lastPathUpdateTime = Time.time;
    }

    private Vector3? FindBestPosition(List<Vector3> positions, Vector3 currentPosition)
    {
        Vector3? bestPosition = null;
        float minSqrDistance = float.MaxValue;

        for (int i = 0; i < positions.Count; i++)
        {
            if (customTilemapManager.IsValidMovePosition(positions[i]))
            {
                float sqrDistance = (currentPosition - positions[i]).sqrMagnitude;
                if (sqrDistance < minSqrDistance)
                {
                    minSqrDistance = sqrDistance;
                    bestPosition = positions[i];
                }
            }
        }

        return bestPosition;
    }

    private List<Vector3> GetSurroundingPositions(Vector3 targetPosition)
    {
        List<Vector3> surroundingPositions = new List<Vector3>(8);
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0) continue;
                surroundingPositions.Add(new Vector3(targetPosition.x + x, targetPosition.y + y, targetPosition.z));
            }
        }
        return surroundingPositions;
    }

    void HandleSelectionAndMovement()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
        if (hit.collider != null && hit.collider.gameObject == gameObject)
        {
            Select();
        }
        else if (isSelected)
        {
            Vector3 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            clickPosition.z = 0;
            nearestValidPosition = customTilemapManager.GetNearestValidPosition(clickPosition);
            if (customTilemapManager.IsValidMovePosition(nearestValidPosition))
            {
                SetNewPath(nearestValidPosition);
                autoMove = false;
            }
        }
        else if (selectedCharacter == this)
        {
            Deselect();
        }
    }

    void Select()
    {
        if (selectedCharacter != null && selectedCharacter != this)
        {
            selectedCharacter.Deselect();
        }
        isSelected = true;
        selectedCharacter = this;
    }

    void Deselect()
    {
        isSelected = false;
        selectedCharacter = null;
    }

    void MoveAlongPath()
    {
        if (path != null && currentPathIndex < path.Count)
        {
            Vector3 targetPosition = path[currentPathIndex];
            if (!customTilemapManager.IsValidMovePosition(targetPosition))
            {
                UpdatePath();
                return;
            }
            if ((transform.position - targetPosition).sqrMagnitude > PositionToleranceSquared)
            {
                Vector3 newPosition = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
                transform.position = newPosition;
            }
            else
            {
                transform.position = targetPosition;
                currentPathIndex++;
            }
            if (currentPathIndex >= path.Count)
            {
                path = null;
                SnapToNearestTileCenter();
                if (autoMove)
                {
                    StartCoroutine(WaitAndFindNewPath());
                }
            }
        }
    }

    private void SnapToNearestTileCenter()
    {
        nearestValidPosition = customTilemapManager.GetNearestValidPosition(transform.position);
        if ((transform.position - nearestValidPosition).sqrMagnitude < PositionToleranceSquared)
        {
            transform.position = nearestValidPosition;
        }
    }

    void SetNewPath(Vector3 target)
    {
        Vector3 start = customTilemapManager.GetNearestValidPosition(transform.position);
        path = customTilemapManager.FindPath(start, target);
        currentPathIndex = 0;

        if (path != null && path.Count > 0)
        {
            TilemapManager.Instance.SetDebugPath(path);
        }
    }

    IEnumerator WaitAndFindNewPath()
    {
        yield return new WaitForSeconds(2f);
        if (detection != null)
        {
            GameObject closestObject = detection.GetClosestObject();
            if (closestObject != null)
            {
                Vector3 targetPosition = closestObject.transform.position;
                SetNewPath(targetPosition);
            }
        }
    }

    IEnumerator AutoMoveCoroutine()
    {
        WaitForSeconds wait = new WaitForSeconds(updatePathInterval);
        while (true)
        {
            yield return wait;
            if (autoMove)
            {
                UpdatePath();
            }
        }
    }
}