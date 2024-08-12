using System.Collections.Generic;
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

    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color selectedColor = Color.yellow;
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
    }
    private void UpdatePotionInfo()
    {
        if (currentHero == null) return;

        // ���⼭ currentHero�� ������ �������� ���� ������ ������Ʈ�մϴ�.
        // ���� ���, ������ �����̳� �Ӽ��� ���� ������ ȿ���� �ٸ��� ������ �� �ֽ��ϴ�.
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

        // ������ ������ ���� ���� ȿ���� ����ϰ� ������ ������Ʈ�մϴ�.
        // �̴� ������ ��ü���� ��Ģ�� ���� �޶��� �� �ֽ��ϴ�.
        switch (index)
        {
            case 0: // Green S
            case 1: // Green M
                int hpRestore = (index == 0) ? 50 : 100;
                potionInfos[index].description = $"HP�� {hpRestore + currentHero.level * 5} ȸ���մϴ�.";
                break;
            case 2: // Yellow S
            case 3: // Yellow M
                int mpRestore = (index == 2) ? 30 : 60;
                potionInfos[index].description = $"MP�� {mpRestore + currentHero.level * 3} ȸ���մϴ�.";
                break;
            case 4: // Red S
            case 5: // Red M
                int atkBoost = (index == 4) ? 10 : 20;
                potionInfos[index].description = $"���ݷ��� {atkBoost + currentHero.attackDamage / 10}��ŭ �Ͻ������� ������ŵ�ϴ�.";
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
            // ����� ������ ������ ���� ���� ���� ���·� ����
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
        }
        if (_potionName != null)
        {
            _potionName.text = "";
        }
        if (_potionDescription != null)
        {
            _potionDescription.text = "";
        }

        // ��� ���� ��ư ���� �ʱ�ȭ
        for (int i = 0; i < _potionButtons.Length; i++)
        {
            Image buttonImage = _potionButtons[i].GetComponent<Image>();
            if (buttonImage != null)
            {
                buttonImage.color = normalColor;
            }
        }
    }
}