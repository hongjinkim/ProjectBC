using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MagicPanel : MonoBehaviour, ITraitPanel
{
    public Button[] traitButtons; // 특성 버튼 배열
    public TextMeshProUGUI[] traitDescriptions; // 특성 설명 텍스트 배열
    public Image[] traitIcons; // 특성 아이콘 이미지 배열

    private HeroInfo currentHeroInfo; // 현재 영웅 정보
    private MagicTrait magicTrait; // 마법 특성

    public void Initialize(HeroInfo heroInfo)
    {
        currentHeroInfo = heroInfo;
        magicTrait = heroInfo.traits.Find(t => t is MagicTrait) as MagicTrait;
        if (magicTrait == null)
        {
            Debug.LogError("이 영웅에 대한 MagicTrait를 찾을 수 없습니다.");
            return;
        }
        UpdateUI();
    }

    private void UpdateUI()
    {
        UpdateTraitButtons();
    }

    private void UpdateTraitButtons()
    {
        int[] traitLevels = { 10, 20, 30, 40 };
        string[] traitNames = {
            "에너지 흡수", "에너지 사이펀",
            "에테르 충격", "비전 유성",
            "비전 침식", "금지된 주문",
            "마나 정제", "힘의 충격"
        };

        for (int i = 0; i < traitButtons.Length; i++)
        {
            int level = traitLevels[i / 2];
            bool isLeftTrait = i % 2 == 0;
            bool isSelected = currentHeroInfo.IsTraitSelected(TraitType.Magic, level, isLeftTrait);
            bool isOppositeSelected = currentHeroInfo.IsTraitSelected(TraitType.Magic, level, !isLeftTrait);

            // 버튼 상호작용 가능 여부 업데이트
            traitButtons[i].interactable = currentHeroInfo.level >= level && !isSelected && !isOppositeSelected;

            // 버튼 색상 업데이트
            Color buttonColor = isSelected ? Color.yellow : (traitButtons[i].interactable ? Color.white : Color.gray);
            traitButtons[i].GetComponent<Image>().color = buttonColor;

            if (traitDescriptions != null && i < traitDescriptions.Length)
                traitDescriptions[i].text = traitNames[i];

            int buttonIndex = i;
            traitButtons[i].onClick.RemoveAllListeners();
            traitButtons[i].onClick.AddListener(() => OnTraitButtonClicked(level, isLeftTrait, buttonIndex));
        }
    }

    private void OnTraitButtonClicked(int level, bool isLeftTrait, int buttonIndex)
    {
        magicTrait.ChooseTrait(level, isLeftTrait);


        if (currentHeroInfo.character != null)
        {


            magicTrait.ApplyEffect(currentHeroInfo.character);

        }
        DisableOppositeTraitButton(buttonIndex);
    }
    private void DisableOppositeTraitButton(int selectedIndex)
    {
        int oppositeIndex = selectedIndex % 2 == 0 ? selectedIndex + 1 : selectedIndex - 1;
        if (oppositeIndex >= 0 && oppositeIndex < traitButtons.Length)
        {
            traitButtons[oppositeIndex].interactable = false;
            traitButtons[oppositeIndex].GetComponent<Image>().color = Color.gray;
        }
    }
}