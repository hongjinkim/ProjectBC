using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MagicPanel : MonoBehaviour, ITraitPanel
{
    public TextMeshProUGUI heroNameText;
    public TextMeshProUGUI levelText;
    public Image heroImage;
    public Button[] traitButtons; // 8���� ��ư (������ 2����, 4�� ����)
    public TextMeshProUGUI[] traitDescriptions; // 8���� ���� �ؽ�Ʈ
    public Image[] traitIcons; // 8���� Ư�� ������

    private HeroInfo currentHeroInfo;
    private MagicTrait magicTrait;

    public void Initialize(HeroInfo heroInfo)
    {
        currentHeroInfo = heroInfo;
        magicTrait = heroInfo.traits.Find(t => t is MagicTrait) as MagicTrait;
        if (magicTrait == null)
        {
            Debug.LogError("MagicTrait not found for this hero.");
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
            "������ ���", "������ ������",
            "���׸� ���", "���� ����",
            "���� ħ��", "������ �ֹ�",
            "���� ����", "���� ���"
        };

        for (int i = 0; i < traitButtons.Length; i++)
        {
            int level = traitLevels[i / 2];
            bool isLeftTrait = i % 2 == 0;

            bool isSelected = currentHeroInfo.IsTraitSelected(TraitType.Magic, level, isLeftTrait);
            bool isOppositeSelected = currentHeroInfo.IsTraitSelected(TraitType.Magic, level, !isLeftTrait);

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
        if (!currentHeroInfo.IsTraitApplied(TraitType.Magic, level, isLeftTrait))
        {
            magicTrait.ChooseTrait(level, isLeftTrait);
            if (currentHeroInfo.seungsoo == null) currentHeroInfo.seungsoo = isLeftTrait;
            else
            {
                if (currentHeroInfo.seungsoo != isLeftTrait) return;
            }
            // Character�� ������ ��� ����
            if (currentHeroInfo.character != null)
            {
                magicTrait.ApplyEffect(currentHeroInfo.character);
            }
            else
            {
                // Character�� ������ HeroInfo���� ���
                currentHeroInfo.AddAppliedTrait(TraitType.Magic, level, isLeftTrait);
            }

            UpdateTraitButtons();
        }
        else
        {
            Debug.Log("This trait has already been applied.");
        }
    }
}