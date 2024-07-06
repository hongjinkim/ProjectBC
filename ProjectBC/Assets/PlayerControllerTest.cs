using System.Collections;
using System.Collections.Generic;
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

    void Start()
    {
        tilemapManager = TilemapManager.Instance;
        customTilemapManager = new CustomTilemapManager(tilemapManager, this);
        transform.position = tilemapManager.GetNearestValidPosition(transform.position);
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
            Debug.Log("Auto move: " + (autoMove ? "On" : "Off"));
        }
        MoveAlongPath();

        // 이동이 멈췄을 때 스냅 확인
        if (path == null || path.Count == 0)
        {
            SnapToNearestTileCenter();
        }

        // 주기적으로 경로 업데이트
        if (autoMove && Time.time - lastPathUpdateTime >= updatePathInterval)
        {
            UpdatePath();
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

                // 현재 위치와 목표 위치가 다른 경우에만 새 경로 설정
                if (Vector3.Distance(currentPosition, targetPosition) > 0.1f)
                {
                    SetNewPath(targetPosition);
                }
                else
                {
                    // 목표에 도달했을 때 정확한 타일 중앙으로 스냅
                    transform.position = currentPosition;
                    path = null;
                }
            }
            else
            {
                Debug.LogWarning("No closest object found.");
            }
        }
        lastPathUpdateTime = Time.time;
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
            if (Vector3.Distance(transform.position, targetPosition) > 0.01f)
            {
                Vector3 direction = (targetPosition - transform.position).normalized;
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
        else
        {
            SnapToNearestTileCenter(); 
        }
    }
    private void SnapToNearestTileCenter()
    {
        Vector3 nearestCenter = customTilemapManager.GetNearestValidPosition(transform.position);
        if (Vector3.Distance(transform.position, nearestCenter) < 0.1f)
        {
            transform.position = nearestCenter;
        }
    }

    void SetNewPath(Vector3 target)
    {
        Vector3 nearestValidTarget = customTilemapManager.GetNearestValidPosition(target);
        path = customTilemapManager.FindPath(transform.position, nearestValidTarget);
        currentPathIndex = 0;

        if (path != null && path.Count > 1 && customTilemapManager.IsObstacle(path[path.Count - 1]))
        {
            path.RemoveAt(path.Count - 1);
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
            if (autoMove && detection != null)
            {
                GameObject closestObject = detection.GetClosestObject();
                if (closestObject != null)
                {
                    Vector3 targetPosition = closestObject.transform.position;
                    SetNewPath(targetPosition);
                }
                else
                {
                    Debug.LogWarning("No closest object found.");
                }
            }
        }
    }
}