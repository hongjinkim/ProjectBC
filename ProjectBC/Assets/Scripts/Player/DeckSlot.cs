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

    public void DeckSetHeroData(HeroInfo hero, int index)
    {
        deckIndex = index;
<<<<<<< HEAD
        if (!CheckUIElements())
            return;

=======
>>>>>>> dev
        if (hero != null)
        {
            id = hero.id;
            heroName = hero.heroName;
            level = hero.level;
            power = hero.attackDamage;  // 또는 strength, 팀과 상의 필요
            speed = hero.agility;
            hp = hero.hp;
<<<<<<< HEAD
            heroImage.sprite = Resources.Load<Sprite>($"Images/Heroes/{hero.heroClass}");
            heroImage.enabled = true;
=======
            if (heroImage != null)
            {
                heroImage.sprite = hero.sprite;
                heroImage.enabled = true;
            }
>>>>>>> dev
            gameObject.SetActive(true);
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
        gameObject.SetActive(true);  // 비어있는 슬롯도 보이도록 함
    }

    public bool CheckUIElements()
    {
        if (heroImage == null)
        {
            return false;
        }
        return true;
    }
}