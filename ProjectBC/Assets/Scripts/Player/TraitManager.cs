using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TraitManager : MonoBehaviour
{
    public Button[] traitButtons;
    private List<Trait> traits = new List<Trait>();

    void Start()
    {
        // 예시로 레벨 10에서 특성을 추가하는 로직
        int level = 10;
        AddTraitsForLevel(level);
    }

    void AddTraitsForLevel(int level)
    {
        traits.Clear();

        if (level == 10)
        {
            traits.Add(new Trait(TraitType.Concentration, "방어 침투를 3% 증가시킵니다.", () => { /* 방어 침투 증가 로직 */ }));
            traits.Add(new Trait(TraitType.Concentration, "피해를 3% 증폭시킵니다.", () => { /* 피해 증폭 로직 */ }));
        }
        else if (level == 20)
        {
            traits.Add(new Trait(TraitType.Concentration, "적을 5회 공격 후 후속 공격에 8%의 추가 피해를 입힙니다.", () => { /* 추가 피해 로직 */ }));
            traits.Add(new Trait(TraitType.Concentration, "적을 5회 공격 후 3초 동안 20의 추가 공격 속도를 얻습니다.", () => { /* 추가 공격 속도 로직 */ }));
        }
        else if (level == 30)
        {
            traits.Add(new Trait(TraitType.Concentration, "일반 공격은 타겟의 아머와 마법 저항을 6% 줄입니다.", () => { /* 아머 및 마법 저항 감소 로직 */ }));
            traits.Add(new Trait(TraitType.Concentration, "일반 공격은 타겟의 체력 재생을 3초 동안 20으로 줄입니다.", () => { /* 체력 재생 감소 로직 */ }));
        }
        else if (level == 40)
        {
            traits.Add(new Trait(TraitType.Concentration, "적을 죽이면 3초 동안 공격 속도가 25 증가합니다.", () => { /* 공격 속도 증가 로직 */ }));
            traits.Add(new Trait(TraitType.Concentration, "적을 죽이면 3초 동안 공격 피해가 12% 증가합니다.", () => { /* 공격 피해 증가 로직 */ }));
        }

        // 버튼 초기화 및 설정
        for (int i = 0; i < traitButtons.Length; i++)
        {
            if (i < traits.Count)
            {
                int index = i;
                traitButtons[i].GetComponentInChildren<Text>().text = traits[i].Description;
                traitButtons[i].onClick.RemoveAllListeners();
                traitButtons[i].onClick.AddListener(() => OnTraitSelected(index));
                traitButtons[i].gameObject.SetActive(true);
            }
            else
            {
                traitButtons[i].gameObject.SetActive(false);
            }
        }
    }

    void OnTraitSelected(int index)
    {
        // 선택된 특성의 효과 실행
        traits[index].Effect.Invoke();

        // 선택되지 않은 버튼 비활성화
        for (int i = 0; i < traitButtons.Length; i++)
        {
            if (i != index)
            {
                traitButtons[i].interactable = false;
            }
        }
    }
}