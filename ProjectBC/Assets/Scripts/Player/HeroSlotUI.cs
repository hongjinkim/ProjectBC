using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class HeroSlotUI : MonoBehaviour
{
    [SerializeField] private int id;
    [SerializeField] private string heroName;
    [SerializeField] private int level;
    [SerializeField] private int power;
    [SerializeField] private int speed;
    [SerializeField] private int hp;
    public Image spriteImage;

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

            if (spriteImage != null)
            {
                spriteImage.sprite = hero.sprite;
                spriteImage.enabled = true;
            }
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}