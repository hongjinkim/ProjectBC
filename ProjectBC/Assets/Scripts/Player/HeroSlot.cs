using UnityEngine;
using UnityEngine.UI;

public class HeroSlot : MonoBehaviour
{
    [SerializeField] private int id;
    [SerializeField] private string heroName;
    [SerializeField] private int level;
    [SerializeField] private int power;
    [SerializeField] private int speed;
    [SerializeField] private int hp;
    [SerializeField] private Button equipButton;
    [SerializeField] private Image spriteImage;
    private HeroManager heroManager;
    private int myHeroIndex;

    private void Awake()
    {
        heroManager = FindObjectOfType<HeroManager>();
        equipButton.onClick.AddListener(OnEquipButtonClick);
    }

    private void OnEquipButtonClick()
    {
        heroManager.AddHeroToDeck(myHeroIndex);
    }
    public void SetHeroData(HeroInfo hero, int index)
    {
        Debug.Log($"Setting hero data in slot: {hero.heroName}");
        if (hero != null)
        {
            id = hero.id;
            heroName = hero.heroName;
            level = hero.level;
            power = hero.attackDamage;
            speed = hero.agility;
            hp = hero.hp;
            myHeroIndex = index;
            if (spriteImage != null)
            {
                spriteImage.sprite = hero.Sprite;
                spriteImage.enabled = hero.Sprite != null;
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
        myHeroIndex = -1;
        if (spriteImage != null)
        {
            spriteImage.sprite = null;
            spriteImage.enabled = false;
        }
        gameObject.SetActive(true);
    }
}
