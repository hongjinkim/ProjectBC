using UnityEngine;
using UnityEngine.UI;

public class TraitSelectionManager : MonoBehaviour
{
    public Button[] initialTraitButtons; // 처음 특성 선택 버튼
    public GameObject[] traitUIPanels; // 각 특성의 UI 패널
    public Button[][] levelButtons; // 각 특성의 레벨 버튼 배열
    private Player player;
    private TraitType selectedTrait;
    private bool traitSelected = false;

    void Start()
    {
        levelButtons = new Button[traitUIPanels.Length][];
        player = FindObjectOfType<Player>();
        for (int i = 0; i < initialTraitButtons.Length; i++)
        {
            int index = i; // Lambda 캡쳐 문제 해결을 위해 인덱스를 지역 변수로 저장
            initialTraitButtons[i].onClick.AddListener(() => OnInitialTraitSelected(index));
        }

        foreach (GameObject panel in traitUIPanels)
        {
            Button[] buttons = panel.GetComponentsInChildren<Button>();
            for (int j = 0; j < buttons.Length; j++)
            {
                int index = j; // Lambda 캡쳐 문제 해결을 위해 인덱스를 지역 변수로 저장
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

            // 선택된 특성 UI 패널 활성화
            traitUIPanels[index].SetActive(true);

            // 다른 버튼 비활성화
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
            // 플레이어 레벨을 가져와서 필요한 작업을 수행할 수 있습니다.
            int playerLevel = player.Level;
            Debug.Log("Player Level: " + playerLevel);

            // 예를 들어, 특정 레벨에 따라 다른 트레잇을 적용하는 로직을 추가할 수 있습니다.
            if (playerLevel >= 10)
            {
                // 특정 트레잇 적용 로직
            }
        }
        else
        {
            Debug.LogError("Player 객체를 찾을 수 없습니다.");
        }
    }
}