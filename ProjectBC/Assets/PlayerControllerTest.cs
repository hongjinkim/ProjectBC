using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public TilemapManager tilemapManager;
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

    void Start()
    {
        tilemapManager = TilemapManager.Instance;
        customTilemapManager = new CustomTilemapManager(tilemapManager, this);
        transform.position = customTilemapManager.GetNearestValidPosition(transform.position);
        detection = GetComponent<Detection>();
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
                Vector3 targetPosition = closestObject.transform.position;
                Vector3 currentPosition = customTilemapManager.GetNearestValidPosition(transform.position);
                if (Vector3.Distance(currentPosition, targetPosition) > PositionTolerance)
                {
                    List<Vector3> surroundingPositions = GetSurroundingPositions(targetPosition);
                    Vector3? bestPosition = surroundingPositions
                        .Where(pos => customTilemapManager.IsValidMovePosition(pos))
                        .OrderBy(pos => Vector3.Distance(currentPosition, pos))
                        .FirstOrDefault();

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
    private List<Vector3> GetSurroundingPositions(Vector3 targetPosition)
    {
        List<Vector3> surroundingPositions = new List<Vector3>();
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0) continue;
                Vector3 position = new Vector3(targetPosition.x + x, targetPosition.y + y, targetPosition.z);
                surroundingPositions.Add(position);
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
        else
        {
            if (isSelected)
            {
                Vector3 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                clickPosition.z = 0;
                Vector3 nearestValidPosition = customTilemapManager.GetNearestValidPosition(clickPosition);
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
            if (Vector3.Distance(transform.position, targetPosition) > PositionTolerance)
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
        Vector3 nearestCenter = customTilemapManager.GetNearestValidPosition(transform.position);
        if (Vector3.Distance(transform.position, nearestCenter) < PositionTolerance)
        {
            transform.position = nearestCenter;
        }
    }
    void SetNewPath(Vector3 target)
    {
        Vector3 start = customTilemapManager.GetNearestValidPosition(transform.position);
        path = customTilemapManager.FindPath(start, target);
        currentPathIndex = 0;

        if (path != null && path.Count > 0)
        {
            tilemapManager.SetDebugPath(path);
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
        while (true)
        {
            yield return new WaitForSeconds(updatePathInterval);
            if (autoMove)
            {
                UpdatePath();
            }
        }
    }
}