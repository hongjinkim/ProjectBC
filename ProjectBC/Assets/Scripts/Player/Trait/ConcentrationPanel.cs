using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConcentrationPanel : MonoBehaviour, ITraitPanel
{
    [SerializeField]
    List<GameObject> level10 = new List<GameObject>();
    [SerializeField]
    List<GameObject> level20 = new List<GameObject>();
    [SerializeField]
    List<GameObject> level30 = new List<GameObject>();
    [SerializeField]
    List<GameObject> level40 = new List<GameObject>();
    public TextMeshProUGUI heroNameText;
    public TextMeshProUGUI levelText;
    public Image heroImage;
    public Button[] traitButtons; // 8���� ��ư (������ 2����, 4�� ����)
    public TextMeshProUGUI[] traitDescriptions; // 8���� ���� �ؽ�Ʈ
    public Image[] traitIcons; // 8���� Ư�� ������

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
        if (currentHeroInfo == null || concentrationTrait == null)
        {
            Debug.LogError("currentHeroInfo or concentrationTrait is null in UpdateTraitButtons");
            return;
        }

        int[] traitLevels = { 10, 20, 30, 40 };
        string[] traitNames = {
            "���� �� ���� ��", "������ ��",
            "���� �����", "���ں�",
            "�м�", "��ȥ�� ��Ȯ",
            "���� �ɷ�", "�������� ����"
        };

        for (int i = 0; i < traitButtons.Length; i++)
        {
            int level = traitLevels[i / 2];
            bool isLeftTrait = i % 2 == 0;
            //��� �ٵ���Ѵ����� ���ִ� ���
            //��带 �ϳ� ���������� �������� ������ �� �ִ� ����Ʈ���� ��带 �� ���ָ� �� ��带 ����ϸ� �ٽ� �־��ְ�
            bool isSelected = currentHeroInfo.IsTraitSelected(TraitType.Concentration, level, isLeftTrait);
            bool isOppositeSelected = currentHeroInfo.IsTraitSelected(TraitType.Concentration, level, !isLeftTrait);

            // ������ �κ�: ���� ����, ���� Ư�� ���� ����, �ݴ� Ư�� ���� ���θ� ��� ����Ͽ� ��ư Ȱ��ȭ ���� ����
            bool canSelectTrait = currentHeroInfo.level >= level && !isSelected && !isOppositeSelected;
            traitButtons[i].interactable = canSelectTrait;

            // ������ �κ�: ���õ� Ư���� ���� �ð��� �ǵ�� ����
            Color buttonColor = isSelected ? Color.yellow : (canSelectTrait ? Color.white : Color.gray);
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
        if (!currentHeroInfo.IsTraitApplied(TraitType.Concentration, level, isLeftTrait))
        {
            concentrationTrait.ChooseTrait(level, isLeftTrait);
            
            if (currentHeroInfo.seungsoo == null) currentHeroInfo.seungsoo = isLeftTrait;
            else
            {
                if (currentHeroInfo.seungsoo != isLeftTrait) return;
            }
            if (currentHeroInfo.character != null)
            {
                
                
                concentrationTrait.ApplyEffect(currentHeroInfo.character);
            }
           

            // Ư�� ���� �� UI ����
            UpdateTraitButtons();
        }
        else
        {
            Debug.Log("This trait has already been applied.");
        }
    }
}