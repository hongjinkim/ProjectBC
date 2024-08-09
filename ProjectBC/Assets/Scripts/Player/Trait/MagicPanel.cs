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

            traitButtons[i].interactable = currentHeroInfo.level >= level;
            traitDescriptions[i].text = traitNames[i];

            if (magicTrait.Level >= level)
            {
                traitButtons[i].interactable = magicTrait.IsLeftTrait == isLeftTrait;
                if (magicTrait.IsLeftTrait == isLeftTrait)
                {
                    traitButtons[i].GetComponent<Image>().color = Color.green; // ���õ� Ư�� ǥ��
                }
            }

            int buttonIndex = i;
            traitButtons[i].onClick.RemoveAllListeners();
            traitButtons[i].onClick.AddListener(() => OnTraitButtonClicked(level, isLeftTrait, buttonIndex));
        }
    }

    private void OnTraitButtonClicked(int level, bool isLeftTrait, int buttonIndex)
    {
        magicTrait.ChooseTrait(level, isLeftTrait);
        magicTrait.ApplyEffect(currentHeroInfo.character);
        UpdateTraitButtons();
    }
}