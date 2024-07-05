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

    private CustomTilemapManager customTilemapManager;

    void Start()
    {
        tilemapManager = TilemapManager.Instance;
        customTilemapManager = new CustomTilemapManager(tilemapManager, this);
        transform.position = tilemapManager.GetNearestValidPosition(transform.position);
        detection = GetComponent<Detection>();
        if (detection == null)
        {
            Debug.LogError("¿©±â¾ß");
        }
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
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
            {
                currentPathIndex++;
            }
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

    IEnumerator AutoMoveCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(updatePathInterval);
            if (autoMove)
            {
                GameObject closestObject = detection.GetClosestObject();
                if (closestObject != null)
                {
                    Vector3 targetPosition = closestObject.transform.position;
                    SetNewPath(targetPosition);
                }
            }
        }
    }
}