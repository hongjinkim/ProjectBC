using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

    public void DeckSetHeroData(HeroManager.Hero hero)
    {
        if (!CheckUIElements()) return;

        if (hero != null)
        {
            id = hero.id;
            heroName = hero.name;
            level = hero.level;
            power = hero.power;
            speed = hero.speed;
            hp = hero.hp;

            heroImage.sprite = hero.sprite;
            heroImage.enabled = true;
            gameObject.SetActive(true);
        }
        else
        {
            ClearSlot();
        }
    }

    private void ClearSlot()
    {
        id = 0;
        heroName = "";
        level = 0;
        power = 0;
        speed = 0;
        hp = 0;

        heroImage.sprite = null;
        heroImage.enabled = false;
        gameObject.SetActive(true);
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