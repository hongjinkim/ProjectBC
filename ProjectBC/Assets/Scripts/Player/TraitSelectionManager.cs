//using UnityEngine;
//using UnityEngine.UI;
//using System.Linq;
//public class TraitSelectionManager : MonoBehaviour
//{
//    public Button[] initialTraitButtons; // �ʱ� Ư�� ���� ��ư��
//    public GameObject[] traitUI; // �� Ư���� ���� UI �г�
//    public Button[][] levelButtons; // �� Ư���� ������ ��ư ��
//    private TraitType selectedTrait; // ���� ���õ� Ư��
//    private bool traitSelected = false; // Ư���� ���õǾ����� ����
//    private Player player; // �÷��̾� ����

//    void Start()
//    {
//        player = FindObjectOfType<Player>(); // ������ Player ��ü ã��
//        InitializeLevelButtons(); // ���� ��ư �ʱ�ȭ
//        InitializeInitialTraitButtons(); // �ʱ� Ư�� ��ư �ʱ�ȭ
//    }

//    void InitializeLevelButtons()
//    {
//        // TraitType enum�� ������ŭ 2���� �迭 ����
//        levelButtons = new Button[System.Enum.GetValues(typeof(TraitType)).Length][];
//        for (int i = 0; i < levelButtons.Length; i++)
//        {
//            // �� Ư�� UI���� ��� ��ư ������Ʈ�� ������ �Ҵ�
//            levelButtons[i] = traitUI[i].GetComponentsInChildren<Button>();
//        }
//    }

//    void InitializeInitialTraitButtons()
//    {
//        for (int i = 0; i < initialTraitButtons.Length; i++)
//        {
//            int index = i;
//            initialTraitButtons[i].onClick.AddListener(() => OnInitialTraitSelected(index));
//        }
//    }
//    public void OnInitialTraitSelected(int traitTypeIndex)
//    {
//        TraitType traitType = (TraitType)traitTypeIndex;

//        if (!traitSelected)
//        {
//            traitSelected = true;
//            selectedTrait = traitType;
//            traitUI[(int)selectedTrait].SetActive(true); // ���õ� Ư�� UI Ȱ��ȭ

//            // ���õ� ��ư �� ��� �ʱ� Ư�� ��ư ��Ȱ��ȭ
//            foreach (Button button in initialTraitButtons)
//            {
//                button.interactable = button == initialTraitButtons[(int)traitType];
//            }

//            InitializeLevelButtonsForSelectedTrait();
//            OnLevelUp(player.Level); // ���� �÷��̾� ������ �°� ��ư Ȱ��ȭ
//        }
//    }

//    void InitializeLevelButtonsForSelectedTrait()
//    {
//        // ���õ� Ư���� ���� ��ư�鿡 ���� 2���� ¦���� ó��
//        for (int i = 0; i < levelButtons[(int)selectedTrait].Length; i += 2)
//        {
//            int index = i;
//            // �� ��ư �ֿ� Ŭ�� �̺�Ʈ ������ �߰�
//            levelButtons[(int)selectedTrait][i].onClick.AddListener(() => OnLevelTraitSelected(index));
//            levelButtons[(int)selectedTrait][i + 1].onClick.AddListener(() => OnLevelTraitSelected(index + 1));
//        }
//    }

//    void OnLevelTraitSelected(int buttonIndex)
//    {
//        // ���õ� ��ư�� �� ã��
//        int pairStartIndex = (buttonIndex / 2) * 2;
//        // �ش� ���� �� ��ư ��� ��Ȱ��ȭ
//        levelButtons[(int)selectedTrait][pairStartIndex].interactable = false;
//        levelButtons[(int)selectedTrait][pairStartIndex + 1].interactable = false;
//        // ���õ� ��ư�� �ٽ� Ȱ��ȭ
//        levelButtons[(int)selectedTrait][buttonIndex].interactable = true;
//    }

//    public void OnLevelUp(int level)
//    {
//        // �� ���� ��ư �ֿ� ����
//        for (int i = 0; i < levelButtons[(int)selectedTrait].Length; i += 2)
//        {
//            int buttonLevel = (i / 2 + 1) * 10; // ��ư ���� ��� (10, 20, 30, 40)
//            bool shouldBeActive = level >= buttonLevel; // Ȱ��ȭ ���� ����
//            // ���� ������ �����ϸ� ��ư �� Ȱ��ȭ
//            levelButtons[(int)selectedTrait][i].interactable = shouldBeActive;
//            levelButtons[(int)selectedTrait][i + 1].interactable = shouldBeActive;
//        }
//    }
//}