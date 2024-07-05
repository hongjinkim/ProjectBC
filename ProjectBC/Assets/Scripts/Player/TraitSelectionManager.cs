using UnityEngine;
using UnityEngine.UI;

public class TraitSelectionManager : MonoBehaviour
{
    public Button[] initialTraitButtons; // ó�� Ư�� ���� ��ư
    public GameObject[] traitUIPanels; // �� Ư���� UI �г�
    public Button[][] levelButtons; // �� Ư���� ���� ��ư �迭
    private Player player;
    private TraitType selectedTrait;
    private bool traitSelected = false;

    void Start()
    {
        levelButtons = new Button[traitUIPanels.Length][];
        player = FindObjectOfType<Player>();
        for (int i = 0; i < initialTraitButtons.Length; i++)
        {
            int index = i; // Lambda ĸ�� ���� �ذ��� ���� �ε����� ���� ������ ����
            initialTraitButtons[i].onClick.AddListener(() => OnInitialTraitSelected(index));
        }

        foreach (GameObject panel in traitUIPanels)
        {
            Button[] buttons = panel.GetComponentsInChildren<Button>();
            for (int j = 0; j < buttons.Length; j++)
            {
                int index = j; // Lambda ĸ�� ���� �ذ��� ���� �ε����� ���� ������ ����
                buttons[j].onClick.AddListener(() => OnLevelTraitSelected(buttons[index], index * 10 + 10));
            }
        }
    }

    void OnInitialTraitSelected(int index)
    {
        if (!traitSelected)
        {
            traitSelected = true;
            selectedTrait = (TraitType)index;

            // ���õ� Ư�� UI �г� Ȱ��ȭ
            traitUIPanels[index].SetActive(true);

            // �ٸ� ��ư ��Ȱ��ȭ
            foreach (Button button in initialTraitButtons)
            {
                button.interactable = false;
            }
        }
    }

    void OnLevelTraitSelected(Button selectedButton, int level)
    {
        Button[] buttons = traitUIPanels[(int)selectedTrait].GetComponentsInChildren<Button>();

        foreach (Button button in buttons)
        {
            if (button != selectedButton && button.name.Contains(level.ToString()))
            {
                button.interactable = false;
            }
        }
    }

    public void OnLevelUp(int level)
    {
        if (traitSelected && level % 10 == 0)
        {
            Button[] buttons = traitUIPanels[(int)selectedTrait].GetComponentsInChildren<Button>();
            foreach (Button button in buttons)
            {
                if (button.name.Contains(level.ToString()))
                {
                    button.interactable = true;
                }
            }
        }
    }
    public void ApplyTrait(TraitType traitType)
    {
        if (player != null)
        {
            player.SelectTraitType(traitType);
            // �÷��̾� ������ �����ͼ� �ʿ��� �۾��� ������ �� �ֽ��ϴ�.
            int playerLevel = player.Level;
            Debug.Log("Player Level: " + playerLevel);

            // ���� ���, Ư�� ������ ���� �ٸ� Ʈ������ �����ϴ� ������ �߰��� �� �ֽ��ϴ�.
            if (playerLevel >= 10)
            {
                // Ư�� Ʈ���� ���� ����
            }
        }
        else
        {
            Debug.LogError("Player ��ü�� ã�� �� �����ϴ�.");
        }
    }
}