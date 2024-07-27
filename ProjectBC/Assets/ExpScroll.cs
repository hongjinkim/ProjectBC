using TMPro;
using UnityEngine;

public class ExpScroll : MonoBehaviour
{
    public static ExpScroll Instance { get; private set; }
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

    public void UpdateExpScrollCount()
    {
        for (int i = 0; i < countTxts.Length; i++)
        {
            countTxts[i].text = "0";
        }

        for (int i = 0; i < GameDataManager.instance.playerInfo.items.Count; i++)
        {
            int Count;
            if (GameDataManager.instance.playerInfo.items[i].id == "Exp_Basic")
            {
                Count = GameDataManager.instance.playerInfo.items[i].Count;
                countTxts[0].text = Count.ToString();
            }
            else if (GameDataManager.instance.playerInfo.items[i].id == "Exp_Common")
            {
                Count = GameDataManager.instance.playerInfo.items[i].Count;
                countTxts[1].text = Count.ToString();
            }
            else if (GameDataManager.instance.playerInfo.items[i].id == "Exp_Rare")
            {
                Count = GameDataManager.instance.playerInfo.items[i].Count;
                countTxts[2].text = Count.ToString();
            }
            else if (GameDataManager.instance.playerInfo.items[i].id == "Exp_Epic")
            {
                Count = GameDataManager.instance.playerInfo.items[i].Count;
                countTxts[3].text = Count.ToString();
            }
            else if (GameDataManager.instance.playerInfo.items[i].id == "Exp_Legendary")
            {
                Count = GameDataManager.instance.playerInfo.items[i].Count;
                countTxts[4].text = Count.ToString();
            }
        }
    }
}