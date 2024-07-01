using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryBottomButtonHandler : MonoBehaviour
{
    [SerializeField] private GameObject Equip;
    [SerializeField] private GameObject Use;
    [SerializeField] private GameObject Material;
    [SerializeField] private GameObject Crystal;
    private GameObject activeMenu;

    private void Start()
    {
        activeMenu = Equip;
        activeMenu.SetActive(true);
    }

    private void ActivateObject(GameObject obj)
    {
        if (activeMenu != null)
        {
            activeMenu.SetActive(false);
        }
        activeMenu = obj;
        activeMenu.SetActive(true);
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
}
