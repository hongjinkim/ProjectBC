using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroSlotUI : MonoBehaviour
{
    public Image heroIcon;
    public Text heroName;
    public Text heroLevel;
    public Button button;

    private Player hero;

    public void SetHero(Player hero)
    {
        this.hero = hero;
        UpdateUI();
    }

    private void UpdateUI()
    {
        heroName.text = hero.GetType().Name;
        heroLevel.text = "Lv. " + hero.Level;
        // 영웅 아이콘 설정 (Resources 폴더에서 로드 가정)
        heroIcon.sprite = Resources.Load<Sprite>("HeroIcons/" + hero.GetType().Name);
    }
}
