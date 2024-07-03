using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppedItem : MonoBehaviour
{
    public IItem itemData;

    public void OnInteract()
    {
        GameDataManager.ItemAdded?.Invoke(itemData);
        Destroy(gameObject);
    }
}
