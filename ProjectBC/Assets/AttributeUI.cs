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

    [Header("Exps")]
    [SerializeField] private TextMeshProUGUI property1Exp;
    [SerializeField] private TextMeshProUGUI property2Exp;
    [SerializeField] private TextMeshProUGUI property3Exp;
    [SerializeField] private TextMeshProUGUI property4Exp;

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
        property3Button.onClick.AddListener(() => LevelUp("Deffense"));
        property4Button.onClick.AddListener(() => LevelUp("MasicResistance"));
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

        UpdatePropertyDisplay("HP", property1Gold, property1Exp, property1Level, currentHero.hpLevel, currentHero.hp);
        UpdatePropertyDisplay("Strength", property2Gold, property2Exp, property2Level, currentHero.strengthLevel, currentHero.attackDamage);
        UpdatePropertyDisplay("Deffense", property3Gold, property3Exp, property3Level, currentHero.defenseLevel, currentHero.defense);
        UpdatePropertyDisplay("MasicResistance", property4Gold, property4Exp, property4Level, currentHero.masicResistanceLevel, currentHero.magicResistance);
    }

    private void UpdatePropertyDisplay(string propertyName, TextMeshProUGUI goldText, TextMeshProUGUI expText, TextMeshProUGUI levelText, int level, int value)
    {
        if (goldText == null || expText == null || levelText == null)
        {
            Debug.LogError($"One or more UI elements for {propertyName} are null");
            return;
        }

        (int goldCost, int expCost) = CalculateCost(propertyName);
        goldText.text = goldCost.ToString();
        expText.text = expCost.ToString();
        levelText.text = $"Lv.{level}";
    }

    private void LevelUp(string propertyName)
    {
        if (currentHero == null) return;

        int currentPropertyLevel = GetCurrentPropertyLevel(propertyName);
        if (currentPropertyLevel >= currentHero.level)
        {
            Debug.Log($"{propertyName} ������ �̹� ���� ������ ���ų� �����ϴ�. �� �̻� �ø� �� �����ϴ�.");
            return;
        }

        (int goldCost, int expCost) = CalculateCost(propertyName);
        if (GameDataManager.instance.playerInfo.gold >= goldCost && currentHero.currentExp >= expCost)
        {
            GameDataManager.instance.playerInfo.gold -= goldCost;
            currentHero.currentExp -= expCost;

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
                case "Deffense":
                    currentHero.defenseLevel++;
                    currentHero.defense += 1;
                    break;
                case "MasicResistance":
                    currentHero.masicResistanceLevel++;
                    currentHero.magicResistance += 1;
                    break;
            }
            UpdateAttributeDisplay();
            GameDataManager.instance.UpdateFunds();
        }
        else
        {
            Debug.Log("Gold �Ǵ� Exp�� �����մϴ�!");
        }
    }

    private (int goldCost, int expCost) CalculateCost(string propertyName)
    {
        int baseGoldCost = 200;
        int baseExpCost =  10;
        int level = 1;

        switch (propertyName)
        {
            case "HP":
                level = currentHero.hpLevel;
                break;
            case "Strength":
                level = currentHero.strengthLevel;
                break;
            case "Deffense":
                level = currentHero.defenseLevel;
                break;
            case "MasicResistance":
                level = currentHero.masicResistanceLevel;
                break;
        }

        int goldCost = (int)(baseGoldCost * Mathf.Pow(1.2f, level - 1));
        int expCost = (int)(baseExpCost * Mathf.Pow(1.1f, level - 1));

        return (goldCost, expCost);
    }
    private int GetCurrentPropertyLevel(string propertyName)
    {
        switch (propertyName)
        {
            case "HP":
                return currentHero.hpLevel;
            case "Strength":
                return currentHero.strengthLevel;
            case "Deffense":
                return currentHero.defenseLevel;
            case "MasicResistance":
                return currentHero.masicResistanceLevel;
            default:
                return 0;
        }
    }
}