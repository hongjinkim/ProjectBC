using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryScreen : MenuScreen
{
    //prefab
    public GameObject slotPrefab;

    //slots
    public Transform EquipableSlot;
    public Transform UsableSlot;
    public Transform MaterialSlot;
    public Transform CrystalSlot;


    private void OnEnable()
    {
        GameDataManager.EquipableInventoryUpdated += OnEquipableUpdate;
        GameDataManager.UsableInventoryUpdated += OnUsableUpdate;
        GameDataManager.MaterialInventoryUpdated += OnMaterialUpdate;
        GameDataManager.CrystalInventoryUpdated += OnCrystalUpdate;

    }

    private void OnDisable()
    {
        GameDataManager.EquipableInventoryUpdated -= OnEquipableUpdate;
        GameDataManager.UsableInventoryUpdated -= OnUsableUpdate;
        GameDataManager.MaterialInventoryUpdated -= OnMaterialUpdate;
        GameDataManager.CrystalInventoryUpdated -= OnCrystalUpdate;

    }

    void OnEquipableUpdate(PlayerInfo info)
    {

    }
    void OnUsableUpdate(PlayerInfo info)
    {

    }
    void OnMaterialUpdate(PlayerInfo info)
    {

    }
    void OnCrystalUpdate(PlayerInfo info)
    {

    }

    void FreshSlot()
    {

    }

    void MakeSlot()
    {
        
    }
}
