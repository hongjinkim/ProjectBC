using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MyHeroSlot : MonoBehaviour
{
    [SerializeField] private int id;
    [SerializeField] private string heroName;
    [SerializeField] private int level;
    [SerializeField] private int power;
    [SerializeField] private int speed;
    [SerializeField] private int hp;
    [SerializeField] private Image spriteImage;

    private HeroManager heroManager;
    private int myHeroIndex;

    private void Awake()
    {
        heroManager = FindObjectOfType<HeroManager>();
    }

    public void SetMyHeroData(HeroInfo hero, int index)
    {
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
                spriteImage.enabled = true;
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
        gameObject.SetActive(false);
    }
}
