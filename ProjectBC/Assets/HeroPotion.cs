using System.Collections.Generic;
using TMPro;
using Unity.Android.Gradle;
using Unity.VisualScripting;
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

    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color selectedColor = Color.yellow;

    public Image potionSelected;
    public TextMeshProUGUI potionSelectedCount;

    private int currentSelectedIndex = -1;

    private HeroInfo currentHero;
    private Dictionary<int, int> heroToPotionIndex = new Dictionary<int, int>();

    public static HeroPotion Instance { get; private set; }

    private List<Item> potions = new List<Item>();

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

        if (GameDataManager.instance.itemDictionary.ContainsKey(ItemType.Usable))
        {
            potions = GameDataManager.instance.itemDictionary[ItemType.Usable];
        }

        foreach(Item item in potions)
        {
            int count = item.count;

            switch (item.Params.Id)
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
    }

    private void SelectPotion(int potionIndex)
    {
        if (potionIndex < 0 || potionIndex >= potionInfos.Length)
        {
            ResetPotionUI();
            return;
        }

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

        if (currentHero != null)
        {
            heroToPotionIndex[currentHero.id] = currentSelectedIndex;
        }
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

            if (potionSelected != null && potionInfos[currentSelectedIndex].icon != null)
            {

                potionSelected.sprite = potionInfos[currentSelectedIndex].icon.sprite;
                potionSelected.color = Color.white;

                currentHero.PotionItem = potions[currentSelectedIndex];
                UpdatePotionSlot();
              
            }
        }
        else
        {
            ResetPotionUI();
        }
    }




    public void UpdateCurrentHero(HeroInfo hero)
    {
        currentHero = hero;
        ResetPotionUI();
        UpdatePotionInfo();
        LoadSelectedPotionForHero();

        UpdatePotionSlot(); // 현재 영웅 변경 시 포션 슬롯 업데이트
    }
    private void UpdatePotionInfo()
    {
        if (currentHero == null) return;

        // 여기서 currentHero의 정보를 바탕으로 포션 정보를 업데이트합니다.
        // 예를 들어, 영웅의 레벨이나 속성에 따라 포션의 효과를 다르게 설정할 수 있습니다.
        for (int i = 0; i < potionInfos.Length; i++)
        {
            UpdatePotionEffect(i);
        }

        UpdatePotionCount();
        if (currentSelectedIndex >= 0 && currentSelectedIndex < potionInfos.Length)
        {
            SelectPotion(currentSelectedIndex);
        }
    }
    private void UpdatePotionEffect(int index)
    {
        if (index < 0 || index >= potionInfos.Length) return;

        // 영웅의 정보에 따라 포션 효과를 계산하고 설명을 업데이트합니다.
        // 이는 게임의 구체적인 규칙에 따라 달라질 수 있습니다.
        switch (index)
        {
            case 0: // Green S
            case 1: // Green M
                int hpRestore = (index == 0) ? 50 : 100;
                potionInfos[index].description = $"HP를 {hpRestore + currentHero.level * 5} 회복합니다.";
                break;
            case 2: // Yellow S
            case 3: // Yellow M
                int mpRestore = (index == 2) ? 30 : 60;
                potionInfos[index].description = $"MP를 {mpRestore + currentHero.level * 3} 회복합니다.";
                break;
            case 4: // Red S
            case 5: // Red M
                int atkBoost = (index == 4) ? 10 : 20;
                potionInfos[index].description = $"공격력을 {atkBoost + currentHero.attackDamage / 10}만큼 일시적으로 증가시킵니다.";
                break;
        }
    }

    private void LoadSelectedPotionForHero()
    {
        if (currentHero == null) return;

        if (heroToPotionIndex.TryGetValue(currentHero.id, out int savedIndex))
        {
            SelectPotion(savedIndex);
            SwapPotion();
        }
        else
        {
            // 저장된 정보가 없으면 포션 선택 해제 상태로 유지
            ResetPotionUI();
        }
    }
    private void ResetPotionUI()
    {
        currentSelectedIndex = -1;
        if (selectedSlotImage != null)
        {
            selectedSlotImage.sprite = null;
            selectedSlotImage.color = Color.clear;

            potionSelected.sprite = null;
            potionSelected.color = Color.clear;

        }
        if (_potionName != null)
        {
            _potionName.text = "";
        }
        if (_potionDescription != null)
        {
            _potionDescription.text = "";
        }

        // 모든 포션 버튼 색상 초기화
        for (int i = 0; i < _potionButtons.Length; i++)
        {
            Image buttonImage = _potionButtons[i].GetComponent<Image>();
            if (buttonImage != null)
            {
                buttonImage.color = normalColor;
            }
        }
    }

    public void UpdatePotionSlot()
    {
        if (currentHero != null && currentHero.PotionItem != null)
        {
            // 현재 영웅이 포션을 가지고 있는 경우
            if (potionSelected != null)
            {
                potionSelected.sprite = ItemCollection.active.GetItemIcon(currentHero.PotionItem)?.sprite;
                potionSelected.color = Color.white;
            }

            if (selectedSlotImage != null)
            {
                //selectedSlotImage.sprite = ItemCollection.active.GetBackground(currentHero.PotionItem) ?? ItemCollection.active.backgroundBrown;
                //selectedSlotImage.color = Color.white;

                selectedSlotImage.sprite = ItemCollection.active.GetItemIcon(currentHero.PotionItem)?.sprite;
                selectedSlotImage.color = Color.white;
            }

            if (potionSelectedCount != null)
            {
                potionSelectedCount.text = currentHero.PotionItem.count.ToString();
                potionSelectedCount.gameObject.SetActive(true);
            }
        }
        else
        {
            // 현재 영웅이 포션을 가지고 있지 않은 경우
            if (potionSelected != null)
            {
                potionSelected.sprite = null;
                potionSelected.color = new Color(1, 1, 1, 0); // 완전히 투명하게 설정
            }

            if (selectedSlotImage != null)
            {
                selectedSlotImage.sprite = ItemCollection.active.backgroundBrown;
                selectedSlotImage.color = new Color(1, 1, 1, 0.5f); // 반투명하게 설정
            }

            if (potionSelectedCount != null)
            {
                potionSelectedCount.text = "";
                potionSelectedCount.gameObject.SetActive(false);
            }
        }
    }
}