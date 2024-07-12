using UnityEngine;
using UnityEngine.UI;

public class BattleDeckSlot : MonoBehaviour
{
    [SerializeField] private int id;
    [SerializeField] private string heroName;
    [SerializeField] private int level;
    [SerializeField] private int power;
    [SerializeField] private int speed;
    [SerializeField] private int hp;
    [SerializeField] private Image heroImage;

    public void SetHeroData(HeroManager.Hero hero)
    {
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

}