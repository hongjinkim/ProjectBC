using UnityEngine;
using UnityEngine.UI;

public class DeckSlot : MonoBehaviour
{
    [SerializeField] private Image heroImage;
    [SerializeField] private int id;
    [SerializeField] private string heroName;
    [SerializeField] private int level;
    [SerializeField] private int power;
    [SerializeField] private int speed;
    [SerializeField] private int hp;

    [SerializeField] private Button unequipButton;
    private HeroManager heroManager;
    private int deckIndex;

    private void Awake()
    {
        heroManager = FindObjectOfType<HeroManager>();
        unequipButton.onClick.AddListener(OnUnequipButtonClick);
    }

    private void OnUnequipButtonClick()
    {
        heroManager.RemoveHeroFromDeck(deckIndex);
    }

    public void DeckSetHeroData(HeroManager.Hero hero, int index)
    {
        deckIndex = index;
        if (hero != null)
        {
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
            Debug.Log($"DeckSlot {index}: Set hero data for {heroName}");
        }
        else
        {
            ClearSlot();
        }
    }
    private void SetHeroImage(Sprite sprite)
    {
        if (heroImage != null)
        {
            heroImage.sprite = sprite;
            heroImage.enabled = sprite != null;
            Debug.Log($"DeckSlot: Setting hero image. Sprite null? {sprite == null}, Image enabled: {heroImage.enabled}");
        }
        else
        {
            Debug.LogError("DeckSlot: Cannot set hero image. Image component is null.");
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
        SetHeroImage(null);
        gameObject.SetActive(true);  // ����ִ� ���Ե� ���̵��� ��
    }

    public bool CheckUIElements()
    {
        if (heroImage == null)
        {
            Debug.LogError("DeckSlot: Image component is missing during UI check!");
            return false;
        }
        return true;
    }
}