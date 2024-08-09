//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;

//public class TraitSelection : MonoBehaviour
//{
//    public int Level;
//    public GameObject buttonPrefab;
//    public Transform buttonContainer;

//    private List<Trait> availableTraits;
//    private List<Trait> selectedTraits = new List<Trait>();

//    void Start()
//    {
//        SelectTrait();
//    }

//    private void SelectTrait()
//    {
//        availableTraits = GetAvailableTraits(Level);

//        if (availableTraits.Count > 0)
//        {
//            foreach (var trait in availableTraits)
//            {
//                GameObject button = Instantiate(buttonPrefab, buttonContainer);
//                button.GetComponentInChildren<Text>().text = trait.Description;
//                button.GetComponent<Button>().onClick.AddListener(() => OnTraitSelected(trait));
//            }
//        }
//    }

//    private void OnTraitSelected(Trait selectedTrait)
//    {
//        selectedTraits.Add(selectedTrait);
//        selectedTrait.ApplyEffect();

//        // ���� �Ϸ� �� ��ư ��Ȱ��ȭ
//        foreach (Transform child in buttonContainer)
//        {
//            Destroy(child.gameObject);
//        }
//    }

//    private List<Trait> GetAvailableTraits(int level)
//    {
//        List<Trait> traits = new List<Trait>();

//        // TraitType.Concentration
//        if (level == 10)
//        {
//            traits.Add(new Trait(TraitType.Concentration, "��� ħ���� 3% ������ŵ�ϴ�.", () => { /* ��� ħ�� ���� ���� */ }));
//            traits.Add(new Trait(TraitType.Concentration, "���ظ� 3% ������ŵ�ϴ�.", () => { /* ���� ���� ���� */ }));
//        }
//        else if (level == 20)
//        {
//            traits.Add(new Trait(TraitType.Concentration, "���� 5ȸ ���� �� �ļ� ���ݿ� 8%�� �߰� ���ظ� �����ϴ�.", () => { /* �߰� ���� ���� */ }));
//            traits.Add(new Trait(TraitType.Concentration, "���� 5ȸ ���� �� 3�� ���� 20�� �߰� ���� �ӵ��� ����ϴ�.", () => { /* �߰� ���� �ӵ� ���� */ }));
//        }
//        else if (level == 30)
//        {
//            traits.Add(new Trait(TraitType.Concentration, "�Ϲ� ������ Ÿ���� �Ƹӿ� ���� ������ 6% ���Դϴ�.", () => { /* �Ƹ� �� ���� ���� ���� ���� */ }));
//            traits.Add(new Trait(TraitType.Concentration, "�Ϲ� ������ Ÿ���� ü�� ����� 3�� ���� 20���� ���Դϴ�.", () => { /* ü�� ��� ���� ���� */ }));
//        }
//        else if (level == 40)
//        {
//            traits.Add(new Trait(TraitType.Concentration, "���� ���̸� 3�� ���� ���� �ӵ��� 25 �����մϴ�.", () => { /* ���� �ӵ� ���� ���� */ }));
//            traits.Add(new Trait(TraitType.Concentration, "���� ���̸� 3�� ���� ���� ���ذ� 12% �����մϴ�.", () => { /* ���� ���� ���� ���� */ }));
//        }

//        //Plunder
//        if (level == 10)
//        {
//            traits.Add(new Trait(TraitType.Plunder, "�������� ����ġ12% �߰�.", () => { /* ��� ħ�� ���� ���� */ }));
//            traits.Add(new Trait(TraitType.Plunder, "�� ��ü�� ����ġ 7% �߰�.", () => { /* ���� ���� ���� */ }));
//        }
//        else if (level == 20)
//        {
//            traits.Add(new Trait(TraitType.Plunder, "������ ������� 2% �����մϴ�.", () => { /* �߰� ���� ���� */ }));
//            traits.Add(new Trait(TraitType.Plunder, "��� ������� 2% �����մϴ�.", () => { /* �߰� ���� �ӵ� ���� */ }));
//        }
//        else if (level == 30)
//        {
//            traits.Add(new Trait(TraitType.Plunder, "��� ������� 2% �����մϴ�.", () => { /* �Ƹ� �� ���� ���� ���� ���� */ }));
//            traits.Add(new Trait(TraitType.Plunder, "��� ������� 2% �����մϴ�.", () => { /* ü�� ��� ���� ���� */ }));
//        }
//        else if (level == 40)
//        {
//            traits.Add(new Trait(TraitType.Plunder, "����Ǵ� ����� ���� 10% �����մϴ�.", () => { /* ���� �ӵ� ���� ���� */ }));
//            traits.Add(new Trait(TraitType.Plunder, "������ �׻� ��� ����մϴ�.", () => { /* ���� ���� ���� ���� */ }));
//        }
//        //Plunder

//        //Magic
//        if (level == 10)
//        {
//            traits.Add(new Trait(TraitType.Magic, "��ó�� ���� ���϶� 50 ������ ȸ��.", () => { /* ��� ħ�� ���� ���� */ }));
//            traits.Add(new Trait(TraitType.Magic, "�Ϲ� ������ 15 �߰� ������ ����.", () => { /* ���� ���� ���� */ }));
//        }
//        else if (level == 20)
//        {
//            traits.Add(new Trait(TraitType.Magic, "�Ϲ� ������ 50% �ش��ϴ� �������ظ� 10% Ȯ���� ����.", () => { /* �߰� ���� ���� */ }));
//            traits.Add(new Trait(TraitType.Magic, "������ ����(������30%)�� ����.", () => { /* �߰� ���� �ӵ� ���� */ }));
//        }
//        else if (level == 30)
//        {
//            traits.Add(new Trait(TraitType.Magic, "����� ü������� 3�ʵ��� 25����.", () => { /* �Ƹ� �� ���� ���� ���� ���� */ }));
//            traits.Add(new Trait(TraitType.Magic, "��ų�� 10%�� �߰�����.", () => { /* ü�� ��� ���� ���� */ }));
//        }
//        else if (level == 40)
//        {
//            traits.Add(new Trait(TraitType.Magic, "�ñر� ��ų�� 500������(�ִ� ������ 20%)�� ��� ȸ��.", () => { /* ���� �ӵ� ���� ���� */ }));
//            traits.Add(new Trait(TraitType.Magic, "�ñر� ��ų�� ĳ���ý� ���ݼӵ��� 3�� ���� 30����.", () => { /* ���� ���� ���� ���� */ }));
//        }
//        //Magic

//        //Protection
//        if (level == 10)
//        {
//            traits.Add(new Trait(TraitType.Protection, "�ִ� HP�� 3% ������ŵ�ϴ�.", () => { /* ��� ħ�� ���� ���� */ }));
//            traits.Add(new Trait(TraitType.Protection, "�ƸӸ� 4% ������ŵ�ϴ�.", () => { /* ���� ���� ���� */ }));
//        }
//        else if (level == 20)
//        {
//            traits.Add(new Trait(TraitType.Protection, "���� �ƸӸ� 0.1����.", () => { /* �߰� ���� ���� */ }));
//            traits.Add(new Trait(TraitType.Protection, "���� �������� 0.1����.", () => { /* �߰� ���� �ӵ� ���� */ }));
//        }
//        else if (level == 30)
//        {
//            traits.Add(new Trait(TraitType.Protection, "5%�� ���� ������ ����.", () => { /* �Ƹ� �� ���� ���� ���� ���� */ }));
//            traits.Add(new Trait(TraitType.Protection, "5%�� ���� ��������.", () => { /* ü�� ��� ���� ���� */ }));
//        }
//        else if (level == 40)
//        {
//            traits.Add(new Trait(TraitType.Protection, "�������ݿ� ���� 8%����.", () => { /* ���� �ӵ� ���� ���� */ }));
//            traits.Add(new Trait(TraitType.Protection, "���Ÿ����ݿ� ���� 8%����.", () => { /* ���� ���� ���� ���� */ }));
//        }
//        //Protection

//        //Life
//        if (level == 10)
//        {
//            traits.Add(new Trait(TraitType.Life, "������ �ִ� �������� 300 Ȯ���մϴ�.", () => { /* ��� ħ�� ���� ���� */ }));
//            traits.Add(new Trait(TraitType.Life, "���� ���� ��� �ð��� 10% ���Դϴ�.", () => { /* ���� ���� ���� */ }));
//        }
//        else if (level == 20)
//        {
//            traits.Add(new Trait(TraitType.Life, "������ 12% �� ���� HP�� ȸ���մϴ�.", () => { /* �߰� ���� ���� */ }));
//            traits.Add(new Trait(TraitType.Life, "ü�� ���+7.", () => { /* �߰� ���� �ӵ� ���� */ }));
//        }
//        else if (level == 30)
//        {
//            traits.Add(new Trait(TraitType.Life, "ġ������ ���ظ� ������ �ִ� HP�� 5% ȸ���մϴ�.", () => { /* �Ƹ� �� ���� ���� ���� ���� */ }));
//            traits.Add(new Trait(TraitType.Life, "���� ����ġ�� �ִ� HP�� 3% ȸ���մϴ�.", () => { /* ü�� ��� ���� ���� */ }));
//        }
//        else if (level == 40)
//        {
//            traits.Add(new Trait(TraitType.Life, "???.", () => { /* ���� �ӵ� ���� ���� */ }));
//            traits.Add(new Trait(TraitType.Life, "???.", () => { /* ���� ���� ���� ���� */ }));
//        }
//        //Life

//        //Explosion
//        if (level == 10)
//        {
//            traits.Add(new Trait(TraitType.Explosion, "ũ��Ƽ�� Ȯ�� +3%.", () => { /* ��� ħ�� ���� ���� */ }));
//            traits.Add(new Trait(TraitType.Explosion, "ũ��Ƽ�� ������ +12%", () => { /* ���� ���� ���� */ }));
//        }
//        else if (level == 20)
//        {
//            traits.Add(new Trait(TraitType.Explosion, "���ظ� ���� �� 6�� �߰� �������� ȸ���մϴ�.", () => { /* �߰� ���� ���� */ }));
//            traits.Add(new Trait(TraitType.Explosion, "������ HP�� ������ ���� �ִ� 8%�� �߰� ���ظ� �����ϴ�.", () => { /* �߰� ���� �ӵ� ���� */ }));
//        }
//        else if (level == 30)
//        {
//            traits.Add(new Trait(TraitType.Explosion, "HP�� 50%�̸��� ������ 6%�� �߰� ���ظ� �����ϴ�.", () => { /* �Ƹ� �� ���� ���� ���� ���� */ }));
//            traits.Add(new Trait(TraitType.Explosion, "HP�� 50%�̻��� ������ 6%�� �߰� ���ظ� �����ϴ�.", () => { /* ü�� ��� ���� ���� */ }));
//        }
//        else if (level == 40)
//        {
//            traits.Add(new Trait(TraitType.Explosion, "���� ���̸� 3�� ���� ���� �ӵ��� 25 �����մϴ�.", () => { /* ���� �ӵ� ���� ���� */ }));
//            traits.Add(new Trait(TraitType.Explosion, "���� ���̸� 3�� ���� ���� ���ذ� 12% �����մϴ�.", () => { /* ���� ���� ���� ���� */ }));
//        }
//        //Explosion
//        return traits;
//    }
//}

