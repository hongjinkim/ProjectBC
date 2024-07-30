using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HeroPotion : MonoBehaviour
{
    [System.Serializable]
    private struct PotionInfo
    {
        public string name;
        public string description;
    }

    [Header("PosionButton List")]
    public Button[] _potionButtons;

    [Header("PosionText List")]
    [SerializeField] private TextMeshProUGUI _potionName;
    [SerializeField] private TextMeshProUGUI _potionDescription;
    [SerializeField] private PotionInfo[] potionInfos;

    //[Header("SelectedPosion List")]
    //[SerializeField] private Button _selectedPosion;
    //[SerializeField] private TextMeshProUGUI _selectedPotionCount;

    //[Header("etc")]
    //public Button changeInformationButton;

    public static HeroPotion Instance { get; private set; }
    [SerializeField] private TextMeshProUGUI[] countTxts;
    //private string[] healPotions = { "Potion_Green_S", "Potion_Green_M", "Potion_Yellow_S", "Potion_Yellow_M", "Potion_Red_S", "Potion_Red_M" };

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
    private void Start()
    {
        for (int i = 0; i < _potionButtons.Length; i++)
        {
            int index = i;
            _potionButtons[i].onClick.AddListener(() => UpdateDescription(index));
        }

        UpdateDescription(0);
    }

    public void UpdatePotionCount()
    {
        for (int i = 0; i < countTxts.Length; i++)
        {
            countTxts[i].text = string.Empty;
        }


        for (int i = 0; i < GameDataManager.instance.playerInfo.items.Count; i++)
        {
            Item item = GameDataManager.instance.playerInfo.items[i];
            int Count;

            if (item.id == "Potion_Green_S")
            {
                Count = item.Count;
                countTxts[0].text = Count.ToString();
            }
            else if (item.id == "Potion_Green_M")
            {
                Count = item.Count;
                countTxts[1].text = Count.ToString();
            }
            else if (item.id == "Potion_Yellow_S")
            {
                Count = item.Count;
                countTxts[2].text = Count.ToString();
            }
            else if (item.id == "Potion_Yellow_M")
            {
                Count = item.Count;
                countTxts[3].text = Count.ToString();
            }
            else if (item.id == "Potion_Red_S")
            {
                Count = item.Count;
                countTxts[4].text = Count.ToString();
            }
            else if (item.id == "Potion_Red_M")
            {
                Count = item.Count;
                countTxts[5].text = Count.ToString();
            }
        }
    }

    private void UpdateDescription(int potionIndex)
    {
        if (potionIndex >= 0 && potionIndex < potionInfos.Length)
        {
            _potionName.text = potionInfos[potionIndex].name;
            _potionDescription.text = potionInfos[potionIndex].description;
        }
    }

}
