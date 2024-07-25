using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryBottomButtonHandler : MonoBehaviour
{
    [SerializeField] private GameObject Equip;
    [SerializeField] private GameObject Use;
    [SerializeField] private GameObject Material;
    [SerializeField] private GameObject Crystal;
    [SerializeField] private Button DisassemblyBtn;
    [SerializeField] private Button SlotExpansionBtn;
    private GameObject activeMenu;

    private InventorySlot InventorySlot;

    private void Start()
    {
        ActivateObject(Equip);
    }

    private void ActivateObject(GameObject obj)
    {
        if (activeMenu != null)
        {
            activeMenu.SetActive(false);
        }
        activeMenu = obj;
        activeMenu.SetActive(true);

        bool isEquipActive = (activeMenu == Equip);
        DisassemblyBtn.gameObject.SetActive(isEquipActive);
        SlotExpansionBtn.gameObject.SetActive(isEquipActive);
    }

    public void OnEquipBotton()
    {
        ActivateObject(Equip);
    }

    public void OnUseBotton()
    {
        ActivateObject(Use);
    }

    public void OnMateriarBotton()
    {
        ActivateObject(Material);
    }

    public void OnCrystalBotton()
    {
        ActivateObject(Crystal);
    }

    public void OnDisassemblyBotton()
    {
        Debug.Log("아이템 분해");
    }

    public void OnSlotExpansionBotton()
    {
        Debug.Log("슬롯 확장");
        InventorySlot.ExpandSlot(5);
    }
}
