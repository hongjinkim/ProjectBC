using UnityEngine;

public class HeroSlotUI : MonoBehaviour
{
    [System.Serializable]
    public class HeroData
    {
        public int id;
        public string name;
        public int level;
        public int power;
        public int speed;
        public int hp;
    }

    [SerializeField] private HeroData currentHeroData;
    [SerializeField] private int slotIndex;

    public void SetHeroData(HeroManager.Hero hero)
    {
        if (hero != null)
        {
            currentHeroData = new HeroData
            {
                id = hero.id,
                name = hero.name,
                level = hero.level,
                power = hero.power,
                speed = hero.speed,
                hp = hero.hp
            };
        }
        else
        {
            currentHeroData = new HeroData
            {
                id = slotIndex,
                name = "Empty",
                level = 0,
                power = 0,
                speed = 0,
                hp = 0
            };
        }
    }

    public void SetSlotIndex(int index)
    {
        slotIndex = index;
    }
    public HeroManager.Hero GetHeroData()
    {
        if (currentHeroData != null)
        {
            return new HeroManager.Hero
            {
                id = currentHeroData.id,
                name = currentHeroData.name,
                level = currentHeroData.level,
                power = currentHeroData.power,
                speed = currentHeroData.speed,
                hp = currentHeroData.hp
            };
        }
        return null;
    }

    public void ClearHeroData()
    {
        currentHeroData = null;
    }

    public void OnHeroSlotClicked()
    {
        if (currentHeroData != null)
        {
            Debug.Log($"Selected hero: {currentHeroData.name}, Power: {currentHeroData.power}, Speed: {currentHeroData.speed}, HP: {currentHeroData.hp}");
        }
    }
}