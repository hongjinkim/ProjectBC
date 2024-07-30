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
        public Image icon;
        public TextMeshProUGUI count;
    }

    [Header("PotionButton List")]
    public Button[] _potionButtons;

    [Header("PotionText List")]
    [SerializeField] private TextMeshProUGUI _potionName;
    [SerializeField] private TextMeshProUGUI _potionDescription;
    [SerializeField] private PotionInfo[] potionInfos;
    [SerializeField] private Button swapButton;
    [SerializeField] private Image selectedSlotImage;
    [SerializeField] private TextMeshProUGUI selectedPotionCountText;

    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color selectedColor = Color.yellow;
    private int currentSelectedIndex = -1;

    public static HeroPotion Instance { get; private set; }

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
            if (_potionButtons[i] != null)
            {
                _potionButtons[i].onClick.AddListener(() => SelectPotion(index));
            }
        }

        if (swapButton != null)
        {
            swapButton.onClick.AddListener(SwapPotion);
        }

        SelectPotion(0);
    }

    public void UpdatePotionCount()
    {
        for (int i = 0; i < potionInfos.Length; i++)
        {
            potionInfos[i].count.text = string.Empty;
        }

        for (int i = 0; i < GameDataManager.instance.playerInfo.items.Count; i++)
        {
            Item item = GameDataManager.instance.playerInfo.items[i];
            int count = item.Count;

            switch (item.id)
            {
                case "Potion_Green_S":
                    potionInfos[0].count.text = count.ToString();
                    break;
                case "Potion_Green_M":
                    potionInfos[1].count.text = count.ToString();
                    break;
                case "Potion_Yellow_S":
                    potionInfos[2].count.text = count.ToString();
                    break;
                case "Potion_Yellow_M":
                    potionInfos[3].count.text = count.ToString();
                    break;
                case "Potion_Red_S":
                    potionInfos[4].count.text = count.ToString();
                    break;
                case "Potion_Red_M":
                    potionInfos[5].count.text = count.ToString();
                    break;
            }
        }

        if (currentSelectedIndex >= 0 && currentSelectedIndex < potionInfos.Length)
        {
            UpdateSelectedPotionCount();
        }
    }

    private void SelectPotion(int potionIndex)
    {
        if (potionIndex < 0 || potionIndex >= potionInfos.Length)
            return;

        if (currentSelectedIndex >= 0 && currentSelectedIndex < _potionButtons.Length)
        {
            Image currentImage = _potionButtons[currentSelectedIndex].GetComponent<Image>();
            if (currentImage != null)
            {
                currentImage.color = normalColor;
            }
        }

        Image newImage = _potionButtons[potionIndex].GetComponent<Image>();
        if (newImage != null)
        {
            newImage.color = selectedColor;
        }

        if (_potionName != null)
        {
            _potionName.text = potionInfos[potionIndex].name;
        }

        if (_potionDescription != null)
        {
            _potionDescription.text = potionInfos[potionIndex].description;
        }

        currentSelectedIndex = potionIndex;
    }

    private void SwapPotion()
    {
        if (currentSelectedIndex >= 0 && currentSelectedIndex < potionInfos.Length)
        {
            if (selectedSlotImage != null && potionInfos[currentSelectedIndex].icon != null)
            {
                selectedSlotImage.sprite = potionInfos[currentSelectedIndex].icon.sprite;
                selectedSlotImage.color = Color.white;
            }
            UpdateSelectedPotionCount();
        }
    }

    private void UpdateSelectedPotionCount()
    {
        if (selectedPotionCountText != null && currentSelectedIndex >= 0 && currentSelectedIndex < potionInfos.Length)
        {
            selectedPotionCountText.text = potionInfos[currentSelectedIndex].count.text;
        }
    }
}