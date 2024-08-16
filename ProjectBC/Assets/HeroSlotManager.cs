using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HeroSlotManager : MonoBehaviour
{
    public HeroManager heroManager;
    public HeroPage heroPage;
    public HeroInfo heroInfo;

    public List<MyHeroSlot> slots;

    private List<HeroInfo> hero;

    [SerializeField] private AttributeUI attributeUI;

    public void OnValidate()
    {
        if (Application.isPlaying) return;

        slots = GetComponentsInChildren<MyHeroSlot>(true).ToList();
    }

    void OnEnable()
    {
        Initialize();
    }

    private void Initialize()
    {
        hero = heroManager.AllHeroes;
        for (int i = 0; i < hero.Count(); i++)
        {
            slots[i].SetMyHeroData(hero[i], i);
        }
    }

    public void NextHeroSelect()
    {
        int idx = heroPage._idx;
        int max = hero.Count();

        if (idx == max - 1)
            idx = 0;
        else
            idx++;

        heroPage.OnHeroSelected(hero[idx], idx);
        UpdateHeroSubscription(idx);

        
    }

    public void PrevHeroSelect()
    {
        int idx = heroPage._idx;
        int max = hero.Count();

        if (idx == 0)
            idx = max - 1;
        else
            idx--;

        heroPage.OnHeroSelected(hero[idx], idx);
        UpdateHeroSubscription(idx);
    }

    private void UpdateHeroSubscription(int newIdx)
    {
        if (heroInfo != null)
        {
            heroInfo.OnExperienceChanged -= heroPage.GaugeBarUpdate;
            heroInfo.OnLevelUp -= heroPage.GaugeBarUpdate;

        }

        heroInfo = hero[newIdx];
        heroInfo.OnExperienceChanged += heroPage.GaugeBarUpdate;
        heroInfo.OnLevelUp += heroPage.GaugeBarUpdate;

        heroPage.GaugeBarUpdate();

        attributeUI.UpdateHeroAttributes(heroInfo);

    }

}
