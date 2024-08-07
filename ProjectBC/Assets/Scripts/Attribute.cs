using UnityEngine;

public class Attribute : MonoBehaviour
{
    [Header("PropertyLevels")]
    public int property1Level = 1;
    public int property2Level = 1;
    public int property3Level = 1;
    public int property4Level = 1;

    public void HpUpButtonClicked()
    {
        LevelUp("HP");
    }

    public void StrengthUpButtonClicked()
    {
        LevelUp("Strength");
    }

    public void AgilltyUpButtonClicked()
    {
        LevelUp("Agility");
    }

    public void IntelligenceUpButtonClicked()
    {
        LevelUp("Intelligence");
    }

    public void LevelUp(string propertyName)
    {
        int cost = CalculateCost(propertyName);
        if (GameDataManager.instance.playerInfo.gold >= cost)
        {
            GameDataManager.instance.playerInfo.gold -= cost;

            switch (propertyName)
            {
                case "HP":
                    property1Level++;
                    break;
                case "Strength":
                    property2Level++;
                    break;
                case "Agility":
                    property3Level++;
                    break;
                case "Intelligence":
                    property4Level++;
                    break;
            }
        }
        else
        {
            Debug.Log("골드가 부족합니다!");
        }
    }

    private int CalculateCost(string propertyName)
    {
        int baseCost = 200;
        int level;

        switch (propertyName)
        {
            case "HP":
                level = property1Level;
                break;
            case "Strength":
                level = property2Level;
                break;
            case "Agility":
                level = property3Level;
                break;
            case "Intelligence":
                level = property4Level;
                break;
            default:
                return 0;
        }


        return (int)(baseCost * Mathf.Pow(1.2f, property1Level - 1));
    }

}
