using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProtectionPanel : MonoBehaviour, ITraitPanel
{
    public TextMeshProUGUI heroNameText;
    public TextMeshProUGUI levelText;
    public Image heroImage;
    public Button[] traitButtons; // 8���� ��ư (������ 2����, 4�� ����)
    public TextMeshProUGUI[] traitDescriptions; // 8���� ���� �ؽ�Ʈ
    public Image[] traitIcons; // 8���� Ư�� ������

    private HeroInfo currentHeroInfo;
    private ProtectionTrait protectionTrait;

    public void Initialize(HeroInfo heroInfo)
    {
        currentHeroInfo = heroInfo;
        protectionTrait = heroInfo.traits.Find(t => t is ProtectionTrait) as ProtectionTrait;
        if (protectionTrait == null)
        {
            Debug.LogError("ProtectionTrait not found for this hero.");
            return;
        }
        UpdateUI();
    }

    private void UpdateUI()
    {
        heroNameText.text = currentHeroInfo.heroName;
        levelText.text = "Level: " + currentHeroInfo.level.ToString();
        heroImage.sprite = currentHeroInfo.Sprite;
        UpdateTraitButtons();
    }

    private void UpdateTraitButtons()
    {
        int[] traitLevels = { 10, 20, 30, 40 };
        string[] traitNames = {
            "�Ŵ��� ����", "��ȭ�� �Ǻ�",
            "������ Ȱ��", "������",
            "�ı��Ұ�", "���� ����",
            "���� ���", "�߰��� ����"
        };

        for (int i = 0; i < traitButtons.Length; i++)
        {
            int level = traitLevels[i / 2];
            bool isLeftTrait = i % 2 == 0;

            bool isSelected = currentHeroInfo.IsTraitSelected(TraitType.Protection, level, isLeftTrait);
            bool isOppositeSelected = currentHeroInfo.IsTraitSelected(TraitType.Protection, level, !isLeftTrait);

            traitButtons[i].interactable = currentHeroInfo.level >= level && !isSelected && !isOppositeSelected;

            if (isSelected)
            {
                traitButtons[i].GetComponent<Image>().color = Color.yellow;
            }
            else
            {
                traitButtons[i].GetComponent<Image>().color = Color.white;
            }

            if (traitDescriptions != null && i < traitDescriptions.Length)
                traitDescriptions[i].text = traitNames[i];

            int buttonIndex = i;
            traitButtons[i].onClick.RemoveAllListeners();
            traitButtons[i].onClick.AddListener(() => OnTraitButtonClicked(level, isLeftTrait, buttonIndex));
        }
    }

    private void OnTraitButtonClicked(int level, bool isLeftTrait, int buttonIndex)
    {
        if (!currentHeroInfo.IsTraitSelected(TraitType.Concentration, level, isLeftTrait))
        {
            protectionTrait.ChooseTrait(level, isLeftTrait);

            // Character�� ��� ȿ���� ����
            currentHeroInfo.SelectTrait(TraitType.Protection, level, isLeftTrait);
            currentHeroInfo.AddTraitEffect(TraitType.Protection, level, isLeftTrait,
                character => protectionTrait.ApplyEffect(character));

            // Character�� ������ ��� ����
            if (currentHeroInfo.character != null)
            {
                protectionTrait.ApplyEffect(currentHeroInfo.character);
            }

            UpdateTraitButtons();
        }
        else
        {
            Debug.Log("This trait has already been applied.");
        }
    }
}