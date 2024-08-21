using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProtectionPanel : MonoBehaviour, ITraitPanel
{
    public Button[] traitButtons; // 특성 버튼 배열
    public TextMeshProUGUI[] traitDescriptions; // 특성 설명 텍스트 배열
    public Image[] traitIcons; // 특성 아이콘 이미지 배열

    private HeroInfo currentHeroInfo; // 현재 영웅 정보
    private ProtectionTrait protectionTrait; // 보호 특성

    public void Initialize(HeroInfo heroInfo)
    {
        currentHeroInfo = heroInfo;
        protectionTrait = heroInfo.traits.Find(t => t is ProtectionTrait) as ProtectionTrait;
        if (protectionTrait == null)
        {
            Debug.LogError("이 영웅에 대한 ProtectionTrait를 찾을 수 없습니다.");
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
            "거대한 혈통", "강화된 피부",
            "무한한 활력", "강인함",
            "파괴불가", "마법 저항",
            "방패 들기", "견고한 지원"
        };

        for (int i = 0; i < traitButtons.Length; i++)
        {
            int level = traitLevels[i / 2];
            bool isLeftTrait = i % 2 == 0;
            bool isSelected = currentHeroInfo.IsTraitSelected(TraitType.Protection, level, isLeftTrait);
            bool isOppositeSelected = currentHeroInfo.IsTraitSelected(TraitType.Protection, level, !isLeftTrait);

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
        protectionTrait.ChooseTrait(level, isLeftTrait);


        if (currentHeroInfo.character != null)
        {


            protectionTrait.ApplyEffect(currentHeroInfo.character);

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