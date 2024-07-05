using UnityEngine;
using System.Collections.Generic;
using static Character;
using static UnityEngine.GraphicsBuffer;

public class CharacterMovement : MonoBehaviour
{
    public TilemapManager tilemapManager;
    public float moveSpeed = 5f;
    private List<Vector3> path;
    private int currentPathIndex;
    private bool isSelected = false;
    private static CharacterMovement selectedCharacter = null;

    void Start()
    {
        if (tilemapManager == null)
        {
            tilemapManager = FindObjectOfType<TilemapManager>();
        }
        transform.position = tilemapManager.GetNearestValidPosition(transform.position);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            HandleSelectionAndMovement();
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
                // 선택된 캐릭터가 있고, 빈 공간을 클릭했을 때 이동
                Vector3 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                clickPosition.z = 0;
                Vector3 nearestValidPosition = tilemapManager.GetNearestValidPosition(clickPosition);
                if (tilemapManager.IsValidMovePosition(nearestValidPosition))
                {
                    path = tilemapManager.FindPath(transform.position, nearestValidPosition);
                    currentPathIndex = 0;
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
        // TODO: 선택된 상태를 시각적으로 표시 (예: 색상 변경)
        Debug.Log($"Selected: {gameObject.name}");
    }

    void Deselect()
    {
        isSelected = false;
        selectedCharacter = null;
        // TODO: 선택 해제 상태를 시각적으로 표시
        Debug.Log($"Deselected: {gameObject.name}");
    }

    void MoveAlongPath()
    {
        if (path != null && currentPathIndex < path.Count)
        {
            Vector3 targetPosition = path[currentPathIndex];
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            if ((Vector2)transform.position == (Vector2)targetPosition)
            {
                currentPathIndex++;
            }
        }
    }
}