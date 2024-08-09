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
        currentHeroInfo = heroInfo;
        concentrationTrait = heroInfo.traits.Find(t => t is ConcentrationTrait) as ConcentrationTrait;
        if (concentrationTrait == null)
        {
            Debug.LogError("ConcentrationTrait not found for this hero.");
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
            "막을 수 없는 힘", "잔인한 힘",
            "가죽 벗기기", "무자비",
            "분쇄", "영혼의 수확",
            "전투 능력", "폭력적인 성격"
        };

        for (int i = 0; i < traitButtons.Length; i++)
        {
            int level = traitLevels[i / 2];
            bool isLeftTrait = i % 2 == 0;

            traitButtons[i].interactable = currentHeroInfo.level >= level;
            traitDescriptions[i].text = traitNames[i];

            // 아이콘 설정 (아이콘이 있다고 가정)
            // traitIcons[i].sprite = Resources.Load<Sprite>($"Icons/Concentration/{traitNames[i]}");

            if (concentrationTrait.Level >= level)
            {
                traitButtons[i].interactable = concentrationTrait.IsLeftTrait == isLeftTrait;
                if (concentrationTrait.IsLeftTrait == isLeftTrait)
                {
                    traitButtons[i].GetComponent<Image>().color = Color.green; // 선택된 특성 표시
                }
            }

            int buttonIndex = i;
            traitButtons[i].onClick.RemoveAllListeners();
            traitButtons[i].onClick.AddListener(() => OnTraitButtonClicked(level, isLeftTrait, buttonIndex));
        }
    }

    private void OnTraitButtonClicked(int level, bool isLeftTrait, int buttonIndex)
    {
        concentrationTrait.ChooseTrait(level, isLeftTrait);
        concentrationTrait.ApplyEffect(currentHeroInfo.character);
        UpdateTraitButtons();
    }
}