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
        // ���÷� ���� 10���� Ư���� �߰��ϴ� ����
        int level = 10;
        AddTraitsForLevel(level);
    }

    void AddTraitsForLevel(int level)
    {
        traits.Clear();

        if (level == 10)
        {
            traits.Add(new Trait(TraitType.Concentration, "��� ħ���� 3% ������ŵ�ϴ�.", () => { /* ��� ħ�� ���� ���� */ }));
            traits.Add(new Trait(TraitType.Concentration, "���ظ� 3% ������ŵ�ϴ�.", () => { /* ���� ���� ���� */ }));
        }
        else if (level == 20)
        {
            traits.Add(new Trait(TraitType.Concentration, "���� 5ȸ ���� �� �ļ� ���ݿ� 8%�� �߰� ���ظ� �����ϴ�.", () => { /* �߰� ���� ���� */ }));
            traits.Add(new Trait(TraitType.Concentration, "���� 5ȸ ���� �� 3�� ���� 20�� �߰� ���� �ӵ��� ����ϴ�.", () => { /* �߰� ���� �ӵ� ���� */ }));
        }
        else if (level == 30)
        {
            traits.Add(new Trait(TraitType.Concentration, "�Ϲ� ������ Ÿ���� �Ƹӿ� ���� ������ 6% ���Դϴ�.", () => { /* �Ƹ� �� ���� ���� ���� ���� */ }));
            traits.Add(new Trait(TraitType.Concentration, "�Ϲ� ������ Ÿ���� ü�� ����� 3�� ���� 20���� ���Դϴ�.", () => { /* ü�� ��� ���� ���� */ }));
        }
        else if (level == 40)
        {
            traits.Add(new Trait(TraitType.Concentration, "���� ���̸� 3�� ���� ���� �ӵ��� 25 �����մϴ�.", () => { /* ���� �ӵ� ���� ���� */ }));
            traits.Add(new Trait(TraitType.Concentration, "���� ���̸� 3�� ���� ���� ���ذ� 12% �����մϴ�.", () => { /* ���� ���� ���� ���� */ }));
        }

        // ��ư �ʱ�ȭ �� ����
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
        // ���õ� Ư���� ȿ�� ����
        traits[index].Effect.Invoke();

        // ���õ��� ���� ��ư ��Ȱ��ȭ
        for (int i = 0; i < traitButtons.Length; i++)
        {
            if (i != index)
            {
                traitButtons[i].interactable = false;
            }
        }
    }
}