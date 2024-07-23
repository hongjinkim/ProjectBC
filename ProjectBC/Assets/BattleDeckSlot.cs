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

    // /////////
    [SerializeField] private int slotIndex;

    private void Awake()
    {
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


    public void SetHeroData(HeroInfo hero, int index)
    {
        slotIndex = index;
        if (hero != null)
        {
            id = hero.id;
            heroName = hero.heroName;
            level = hero.level;
            power = hero.attackDamage;
            speed = hero.agility;
            hp = hero.hp;
            if (heroImage != null)
            {
                heroImage.sprite = hero.Sprite;
                heroImage.enabled = hero.Sprite != null;
            }
            gameObject.SetActive(true);
        }
        else
        {
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
        if (id == 0) return;

        rectTransform.position = originalPosition;
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        HandleDrop(eventData);
    }

    private void HandleDrop(PointerEventData eventData)
    {
        Vector2 worldPoint = mainCamera.ScreenToWorldPoint(eventData.position);
        RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);

        if (hit.collider != null)
        {
            Tilemap tilemap = hit.collider.GetComponent<Tilemap>();
            if (tilemap != null && hit.collider.CompareTag("BattleField"))
            {
                Vector3Int cellPosition = tilemap.WorldToCell(hit.point);
                Vector3 cellCenter = tilemap.GetCellCenterWorld(cellPosition);

                GameManager_2.Instance.CreateHeroPrefabAtPosition(cellCenter, slotIndex);
            }

        }

    }
}
