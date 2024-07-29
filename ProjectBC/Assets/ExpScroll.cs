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
            Item item = GameDataManager.instance.playerInfo.items[i];
            int Count;

            if (item.id == "Exp_Basic")
            {
                Count = item.Count;
                countTxts[0].text = Count.ToString();
            }
            else if (item.id == "Exp_Common")
            {
                Count = item.Count;
                countTxts[1].text = Count.ToString();
            }
            else if (item.id == "Exp_Rare")
            {
                Count = item.Count;
                countTxts[2].text = Count.ToString();
            }
            else if (item.id == "Exp_Epic")
            {
                Count = item.Count;
                countTxts[3].text = Count.ToString();
            }
            else if (item.id == "Exp_Legendary")
            {
                Count = item.Count;
                countTxts[4].text = Count.ToString();
            }
        }
    }
}