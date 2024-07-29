using TMPro;
using UnityEngine;

public class HeroPotion : MonoBehaviour
{
    public static HeroPotion Instance { get; private set; }
    [SerializeField] private TextMeshProUGUI[] countTxts;
    private string[] healPotions = { "Potion_Green_S", "Potion_Green_M", "Potion_Yellow_S", "Potion_Yellow_M", "Potion_Red_S", "Potion_Red_M" };

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void UpdatePotionCount()
    {
        var playerInfo = GameDataManager.instance.playerInfo;

        for (int i = 0; i < countTxts.Length; i++)
        {
            string healPotion = healPotions[i];
            if (playerInfo.trackedItems.TryGetValue(healPotion, out int count))
            {
                countTxts[i].text = count.ToString();
            }
            else
            {
                countTxts[i].text = "0";
            }
        }
    }
}
