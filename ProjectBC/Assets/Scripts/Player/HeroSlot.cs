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
        if (hero != null)
        {
            id = hero.id;
            heroName = hero.heroName;
            level = hero.level;
            power = hero.attackDamage;  // 또는 strength, 팀과 상의 필요
            speed = hero.agility;
            hp = hero.hp;
            myHeroIndex = index;
            if (spriteImage != null)
            {
                spriteImage.sprite = Resources.Load<Sprite>($"Images/Heroes/{hero.heroClass}");
                spriteImage.enabled = true;
                Debug.Log($"Setting hero data: {heroName}, Level: {level}, Power: {power}");
            }
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
            Debug.Log("Setting empty hero slot");
            gameObject.SetActive(false);
        }
    }
}