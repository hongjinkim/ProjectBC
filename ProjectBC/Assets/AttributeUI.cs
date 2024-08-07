using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AttributeUI : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button property1Button;
    [SerializeField] private Button property2Button;
    [SerializeField] private Button property3Button;
    [SerializeField] private Button property4Button;

    [Header("Golds")]
    [SerializeField] private TextMeshProUGUI property1Gold;
    [SerializeField] private TextMeshProUGUI property2Gold;
    [SerializeField] private TextMeshProUGUI property3Gold;
    [SerializeField] private TextMeshProUGUI property4Gold;

    [Header("Levels")]
    [SerializeField] private TextMeshProUGUI property1Level;
    [SerializeField] private TextMeshProUGUI property2Level;
    [SerializeField] private TextMeshProUGUI property3Level;
    [SerializeField] private TextMeshProUGUI property4Level;

    [Header("MyGold")]
    [SerializeField] private TextMeshProUGUI myGold;

    private HeroInfo currentHero;

    private void Awake()
    {
        property1Button.onClick.AddListener(() => LevelUp("HP"));
        property2Button.onClick.AddListener(() => LevelUp("Strength"));
        property3Button.onClick.AddListener(() => LevelUp("Agility"));
        property4Button.onClick.AddListener(() => LevelUp("Intelligence"));
    }

    private void OnEnable()
    {
        GameDataManager.FundsUpdated += MyGoldUpdate;
    }

    private void OnDisable()
    {
        GameDataManager.FundsUpdated -= MyGoldUpdate;
    }

    private void MyGoldUpdate(PlayerInfo info)
    {
        myGold.text = info.gold.ToString();
    }

    public void UpdateHeroAttributes(HeroInfo hero)
    {
        if (hero == null)
        {
            Debug.LogError("Received null hero in UpdateHeroAttributes");
            return;
        }
        currentHero = hero;
        UpdateAttributeDisplay();
    }

    private void UpdateAttributeDisplay()
    {
        if (currentHero == null)
        {
            Debug.LogError("currentHero is null in UpdateAttributeDisplay");
            return;
        }

        if (property1Gold == null || property2Gold == null || property3Gold == null || property4Gold == null)
        {
            Debug.LogError("One or more propertyGold TextMeshProUGUI components are null");
            return;
        }

        if (property1Level == null || property2Level == null || property3Level == null || property4Level == null)
        {
            Debug.LogError("One or more propertyLevel TextMeshProUGUI components are null");
            return;
        }

        property1Gold.text = CalculateCost("HP").ToString();
        property2Gold.text = CalculateCost("Strength").ToString();
        property3Gold.text = CalculateCost("Agility").ToString();
        property4Gold.text = CalculateCost("Intelligence").ToString();

        property1Level.text = $"Lv.{currentHero.hpLevel}";
        property2Level.text = $"Lv.{currentHero.strengthLevel}";
        property3Level.text = $"Lv.{currentHero.agilityLevel}";
        property4Level.text = $"Lv.{currentHero.intelligenceLevel}";
    }

    private void LevelUp(string propertyName)
    {
        if (currentHero == null) return;

        int cost = CalculateCost(propertyName);
        if (GameDataManager.instance.playerInfo.gold >= cost)
        {
            GameDataManager.instance.playerInfo.gold -= cost;
            switch (propertyName)
            {
                case "HP":
                    currentHero.hpLevel++;
                    currentHero.hp += 10;
                    break;
                case "Strength":
                    currentHero.strengthLevel++;
                    currentHero.attackDamage += 2;
                    break;
                case "Agility":
                    currentHero.agilityLevel++;
                    currentHero.agility += 2;
                    break;
                case "Intelligence":
                    currentHero.intelligenceLevel++;
                    currentHero.intelligence += 2;
                    break;
            }
            UpdateAttributeDisplay();
            GameDataManager.instance.UpdateFunds();
        }
        else
        {
            Debug.Log("골드가 부족합니다!");
        }
    }

    private int CalculateCost(string propertyName)
    {
        int baseCost = 200;
        int level = 1;
        switch (propertyName)
        {
            case "HP":
                level = currentHero.hpLevel;
                break;
            case "Strength":
                level = currentHero.strengthLevel;
                break;
            case "Agility":
                level = currentHero.agilityLevel;
                break;
            case "Intelligence":
                level = currentHero.intelligenceLevel;
                break;
        }
        return (int)(baseCost * Mathf.Pow(1.2f, level - 1));
    }
}