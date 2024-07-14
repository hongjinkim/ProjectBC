using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class BattleDeckSlot : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    [SerializeField] private int id;
    [SerializeField] private string heroName;
    [SerializeField] private int level;
    [SerializeField] private int power;
    [SerializeField] private int speed;
    [SerializeField] private int hp;
    [SerializeField] private Image heroImage;

    private Vector3 originalPosition;
    public Canvas canvas;
    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;
    private Camera mainCamera;

    private void Awake()
    {
        Debug.Log($"BattleDeckSlot Awake: {gameObject.name}");
        InitializeComponents();
        mainCamera = Camera.main;
    }
    private void InitializeComponents()
    {
        canvas = GetComponentInParent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();

        if (canvas == null)
            Debug.LogError($"No Canvas found for BattleDeckSlot: {gameObject.name}");
        if (canvasGroup == null)
            Debug.LogWarning($"No CanvasGroup found for BattleDeckSlot: {gameObject.name}. Adding one.");
        canvasGroup = gameObject.AddComponent<CanvasGroup>();
    }


    public void SetHeroData(HeroManager.Hero hero)
    {
        if (hero != null)
        {
            Debug.Log($"Setting hero data. ID: {hero.id}, Name: {hero.name}");
            id = hero.id;
            heroName = hero.name;
            level = hero.level;
            power = hero.power;
            speed = hero.speed;
            hp = hero.hp;
            if (heroImage != null)
            {
                heroImage.sprite = hero.sprite;
                heroImage.enabled = true;
            }
            gameObject.SetActive(true);
        }
        else
        {
            Debug.LogWarning("Attempting to set null hero data.");
            ClearSlot();
        }
    }

    public void ClearSlot()
    {
        id = 0;
        heroName = "";
        level = 0;
        power = 0;
        speed = 0;
        hp = 0;
        if (heroImage != null)
        {
            heroImage.sprite = null;
            heroImage.enabled = false;
        }
        gameObject.SetActive(false);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log($"OnBeginDrag called for BattleDeckSlot: {gameObject.name}, Hero ID: {id}");
        if (id == 0) return;

        originalPosition = rectTransform.position;
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (id == 0) return;
        rectTransform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log($"OnEndDrag called for BattleDeckSlot: {gameObject.name}, Hero ID: {id}");
        if (id == 0) return;

        rectTransform.position = originalPosition;
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        HandleDrop(eventData);
    }

    private void HandleDrop(PointerEventData eventData)
    {
        Debug.Log($"HandleDrop called for BattleDeckSlot: {gameObject.name}, Hero ID: {id}");

        Vector2 worldPoint = mainCamera.ScreenToWorldPoint(eventData.position);
        RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);

        if (hit.collider != null)
        {
            Debug.Log($"Raycast hit: {hit.collider.gameObject.name}, Tag: {hit.collider.gameObject.tag}");

            Tilemap tilemap = hit.collider.GetComponent<Tilemap>();
            if (tilemap != null && hit.collider.CompareTag("BattleField"))
            {
                Vector3Int cellPosition = tilemap.WorldToCell(hit.point);
                Vector3 cellCenter = tilemap.GetCellCenterWorld(cellPosition);

                Debug.Log($"Valid drop target found: {hit.collider.gameObject.name} at cell {cellPosition}");
                GameManager_2.Instance.CreateHeroPrefabAtPosition(cellCenter, id);
            }
            else
            {
                Debug.Log("Hit object is not a tagged Tilemap.");
            }
        }
        else
        {
            Debug.Log("No valid drop target found. Prefab not created.");
        }
    }
}