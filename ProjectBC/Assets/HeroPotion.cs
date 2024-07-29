using TMPro;
using UnityEngine;

public class HeroPotion : MonoBehaviour
{
    public static HeroPotion Instance { get; private set; }
    [SerializeField] private TextMeshProUGUI[] countTxts;

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
        for (int i = 0; i < countTxts.Length; i++)
        {
            countTxts[i].text = "0";
        }

        for (int i = 0; i < GameDataManager.instance.playerInfo.items.Count; i++)
        {
            Item item = GameDataManager.instance.playerInfo.items[i];
            int Count;

            if (item.id == "Potion_Green_S")
            {
                countTxts[0].text = item.Count.ToString();
            }
            else if (item.id == "Potion_Green_M")
            {
                countTxts[1].text = item.Count.ToString();
            }
            else if (item.id == "Potion_Yellow_S")
            {
                countTxts[2].text = item.Count.ToString();
            }
            else if (item.id == "Potion_Yellow_M")
            {
                countTxts[3].text = item.Count.ToString();
            }
            else if (item.id == "Potion_Red_S")
            {
                countTxts[4].text = item.Count.ToString();
            }
            else if (item.id == "Potion_Red_M")
            {
                countTxts[5].text = item.Count.ToString();
            }
        }
    }
}
