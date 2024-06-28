using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    [SerializeField] private GameObject itemPrefab;

    public int itemNumber;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(collision.gameObject);

        GameObject item = Instantiate(itemPrefab, transform.position, Quaternion.identity);
        ItemComponent itemComponent = item.GetComponent<ItemComponent>();
        if (itemComponent != null)
        {
            itemComponent.index = ItemNumber();
        }
        else
        {
            Debug.LogWarning("ItemComponent가 프리팹에 없습니다.");
        }

    }

    public int ItemNumber()
    {
        return itemNumber;
    }
}