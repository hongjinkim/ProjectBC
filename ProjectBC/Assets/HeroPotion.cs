using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HeroPotion : MonoBehaviour
{
    [Header("PosionButton List")]
    public Button[] _potionButtons;

    [Header("PosionText List")]
    [SerializeField] private TextMeshProUGUI[] _potionDescription;

    [Header("SelectedPosion List")]
    [SerializeField] private Button _selectedPosion;
    [SerializeField] private TextMeshProUGUI _selectedPotionCount;

    [Header("etc")]
    public Button changeInformationButton;



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

    //private void Start()
    //{
    //    Debug.Log("HeroPotion Start method called");
    //    for (int i = 0; i < _potionButtons.Length; i++)
    //    {
    //        int index = i;
    //        if (_potionButtons[i] != null)
    //        {
    //            _potionButtons[i].onClick.AddListener(() => SelectedPotion(index));
    //            Debug.Log($"Added listener to button {i}");
    //        }
    //        else
    //        {
    //            Debug.LogError($"Potion button at index {i} is null");
    //        }
    //    }
    //}

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
                countTxts[i].text = string.Empty;
            }
        }
    }

    public int selectedPotionIndex = -1;  // 선택된 포션의 인덱스

    //public void SelectedPotion(int buttonIndex)
    //{
    //    selectedPotionIndex = buttonIndex;
    //    Debug.Log($"SelectedPotion called. New index: {buttonIndex}");
    //    if (potionSlot != null)
    //    {
    //        //potionSlot.SetButtonInformation();
    //    }
    //    else
    //    {
    //        Debug.LogError("PotionSlot reference is null");
    //    }
    //}

    public Sprite GetSelectedPotionSprite()
    {
        if (selectedPotionIndex >= 0 && selectedPotionIndex < _potionButtons.Length)
        {
            return _potionButtons[selectedPotionIndex].image.sprite;
        }
        return null;
    }
}
