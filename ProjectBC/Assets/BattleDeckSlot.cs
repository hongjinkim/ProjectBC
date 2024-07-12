using UnityEngine;

public class BattleDeckSlot : MonoBehaviour
{
    [SerializeField] private int id;
    [SerializeField] private string heroName;
    [SerializeField] private int level;
    [SerializeField] private int power;
    [SerializeField] private int speed;
    [SerializeField] private int hp;
    [SerializeField] private Sprite heroSprite;

    private int deckIndex;

    public void SetHeroData(HeroManager.Hero hero, int index)
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
            heroSprite = hero.sprite;

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
        heroSprite = null;

        gameObject.SetActive(false);
    }

    // Getter methods if needed
    public int GetId() => id;
    public string GetHeroName() => heroName;
    public int GetLevel() => level;
    public int GetPower() => power;
    public int GetSpeed() => speed;
    public int GetHp() => hp;
    public int GetDeckIndex() => deckIndex;
    public Sprite GetHeroSprite() => heroSprite;
}