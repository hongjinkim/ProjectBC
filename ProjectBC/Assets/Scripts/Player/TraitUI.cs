//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using UnityEngine;
//using UnityEngine.UI;

//public class TraitUI : MonoBehaviour
//{
//    public HeroInfo currentHero;
//    public TraitType currentTraitType;

//    public GameObject traitButtonPrefab;
//    public Transform leftTraitContainer;
//    public Transform rightTraitContainer;

//    private void OnEnable()
//    {
//        UpdateTraitUI();
//    }

//    public void SetHero(HeroInfo hero)
//    {
//        currentHero = hero;
//        currentTraitType = GetTraitTypeForHeroClass(hero.heroClass);
//        UpdateTraitUI();
//    }

//    private TraitType GetTraitTypeForHeroClass(HeroClass heroClass)
//    {
//        switch (heroClass)
//        {
//            case HeroClass.Knight: return TraitType.Protection;
//            case HeroClass.Archer: return TraitType.Concentration;
//            case HeroClass.Wizard: return TraitType.Magic;
//            case HeroClass.Priest: return TraitType.Plunder;
//            default: return TraitType.Concentration;
//        }
//    }

//    private void UpdateTraitUI()
//    {
//        ClearTraitContainers();

//        var traits = TraitManager.Instance.GetTraitsForType(currentTraitType);

//        foreach (var trait in traits)
//        {
//            var container = trait.IsLeftTrait ? leftTraitContainer : rightTraitContainer;
//            CreateTraitButton(trait, container);
//        }
//    }

//    private void ClearTraitContainers()
//    {
//        foreach (Transform child in leftTraitContainer)
//        {
//            Destroy(child.gameObject);
//        }
//        foreach (Transform child in rightTraitContainer)
//        {
//            Destroy(child.gameObject);
//        }
//    }

//    private void CreateTraitButton(Trait trait, Transform container)
//    {
//        var buttonObj = Instantiate(traitButtonPrefab, container);
//        var button = buttonObj.GetComponent<Button>();
//        var text = buttonObj.GetComponentInChildren<Text>();

//        text.text = trait.Name;
//        button.onClick.AddListener(() => OnTraitSelected(trait));

//        // ������ ���� ��ư Ȱ��ȭ/��Ȱ��ȭ
//        button.interactable = currentHero.level >= trait.Level;
//    }

//    private void OnTraitSelected(Trait trait)
//    {
//        // �̹� ���õ� Ư������ Ȯ��
//        if (currentHero.traits.Any(t => t.Level == trait.Level))
//        {
//            Debug.Log("�̹� �� ������ Ư���� �����߽��ϴ�.");
//            return;
//        }

//        // �ݴ��� Ư�� ���� �Ұ����ϰ� �����
//        DisableOppositeTraits(trait.Level, trait.IsLeftTrait);

//        // Ư�� ����
//        trait.ApplyEffect(currentHero);
//        currentHero.traits.Add(trait);

//        UpdateTraitUI();
//    }

//    private void DisableOppositeTraits(int level, bool isLeft)
//    {
//        var container = isLeft ? rightTraitContainer : leftTraitContainer;
//        foreach (Transform child in container)
//        {
//            var button = child.GetComponent<Button>();
//            if (button != null)
//            {
//                var trait = TraitManager.Instance.GetTraitsForType(currentTraitType)
//                    .FirstOrDefault(t => t.Level == level && t.IsLeftTrait != isLeft);
//                if (trait != null)
//                {
//                    button.interactable = false;
//                }
//            }
//        }
//    }
//}