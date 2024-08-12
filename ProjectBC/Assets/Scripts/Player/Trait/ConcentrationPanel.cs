using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConcentrationPanel : MonoBehaviour, ITraitPanel
{
    public TextMeshProUGUI heroNameText;
    public TextMeshProUGUI levelText;
    public Image heroImage;
    public Button[] traitButtons; // 8개의 버튼 (레벨당 2개씩, 4개 레벨)
    public TextMeshProUGUI[] traitDescriptions; // 8개의 설명 텍스트
    public Image[] traitIcons; // 8개의 특성 아이콘

    private HeroInfo currentHeroInfo;
    private ConcentrationTrait concentrationTrait;

    public void Initialize(HeroInfo heroInfo)
    {
        if (heroInfo == null)
        {
            Debug.LogError("Received null heroInfo in ConcentrationPanel.Initialize");
            return;
        }
        currentHeroInfo = heroInfo;
        concentrationTrait = heroInfo.traits.Find(t => t is ConcentrationTrait) as ConcentrationTrait;
        if (concentrationTrait == null)
        {
            Debug.LogError("ConcentrationTrait not found for this hero.");
            return;
        }
        Debug.Log($"Initialized ConcentrationPanel with hero: {heroInfo.heroName}");
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (currentHeroInfo == null)
        {
            Debug.LogError("currentHeroInfo is null in ConcentrationPanel");
            return;
        }

        if (heroNameText != null)
            heroNameText.text = currentHeroInfo.heroName;
        else
            Debug.LogWarning("heroNameText is not assigned in ConcentrationPanel");

        if (levelText != null)
            levelText.text = "Level: " + currentHeroInfo.level.ToString();
        else
            Debug.LogWarning("levelText is not assigned in ConcentrationPanel");

        if (heroImage != null && currentHeroInfo.Sprite != null)
            heroImage.sprite = currentHeroInfo.Sprite;
        else
            Debug.LogWarning("heroImage or currentHeroInfo.Sprite is not assigned in ConcentrationPanel");

        UpdateTraitButtons();
    }

    private void UpdateTraitButtons()
    {
        if (currentHeroInfo == null)
        {
            Debug.LogError("currentHeroInfo is null in UpdateTraitButtons");
            return;
        }
        if (concentrationTrait == null)
        {
            Debug.LogError("concentrationTrait is null in UpdateTraitButtons");
            return;
        }
        int[] traitLevels = { 10, 20, 30, 40 };
        string[] traitNames = {
            "막을 수 없는 힘", "잔인한 힘",
            "가죽 벗기기", "무자비",
            "분쇄", "영혼의 수확",
            "전투 능력", "폭력적인 성격"
        };

        for (int i = 0; i < traitButtons.Length; i++)
        {
            int level = traitLevels[i / 2];
            bool isLeftTrait = i % 2 == 0;

            bool isSelected = currentHeroInfo.IsTraitSelected(TraitType.Concentration, level, isLeftTrait);
            bool isOppositeSelected = currentHeroInfo.IsTraitSelected(TraitType.Concentration, level, !isLeftTrait);

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
            concentrationTrait.ChooseTrait(level, isLeftTrait);

            // Character가 없어도 효과를 저장
            currentHeroInfo.SelectTrait(TraitType.Concentration, level, isLeftTrait);
            currentHeroInfo.AddTraitEffect(TraitType.Concentration, level, isLeftTrait,
                character => concentrationTrait.ApplyEffect(character));

            // Character가 있으면 즉시 적용
            if (currentHeroInfo.character != null)
            {
                concentrationTrait.ApplyEffect(currentHeroInfo.character);
            }

            UpdateTraitButtons();
        }
        else
        {
            Debug.Log("This trait has already been applied.");
        }
    }
}