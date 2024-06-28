using UnityEngine;
using UnityEngine.UI;

public class ItemDisplayer : MonoBehaviour
{
    [System.Serializable]
    public class ItemData
    {
        public int index;
        public string itemId;
        public string itemName;
        public string itemQuality;
        [TextArea(3, 10)]
        public string itemDescription;
        public Sprite itemImage;
    }

    public ItemData item;
    public int itemIndex;
    private Text itemNameText;
    private Text itemQualityText;
    private Text itemDescriptionText;
    private SpriteRenderer itemSpriteRenderer;


    private void Start()
    {
        itemNameText = transform.Find("ItemNameText")?.GetComponent<Text>();
        itemQualityText = transform.Find("ItemQualityText")?.GetComponent<Text>();
        itemDescriptionText = transform.Find("ItemDescriptionText")?.GetComponent<Text>();
        itemSpriteRenderer = GetComponent<SpriteRenderer>();

        LoadFromCSV(itemIndex);
    }

    public void DisplayItem()
    {
        if (item == null)
        {
            Debug.LogWarning("Item is null. Cannot display.");
            return;
        }

        if (itemNameText != null) itemNameText.text = item.itemName;
        if (itemQualityText != null) itemQualityText.text = item.itemQuality;
        if (itemDescriptionText != null) itemDescriptionText.text = item.itemDescription;
        if (itemSpriteRenderer != null)
        {
            itemSpriteRenderer.sprite = item.itemImage;
        }

        Debug.Log($"Displaying item: {item.itemName}, Quality: {item.itemQuality}, Description: {item.itemDescription}");
    }

    public void LoadFromCSV(int index)
    {
        if (CsvLoader.Instance == null || CsvLoader.Instance.itemDataList == null)
        {
            Debug.LogError("CsvLoader or itemDataList is null. Make sure CSV data is loaded properly.");
            return;
        }

        var csvItem = CsvLoader.Instance.itemDataList.Find(x => x.index == index);
        if (csvItem != null)
        {
            item = new ItemData
            {
                index = csvItem.index,
                itemId = csvItem.itemId,
                itemName = csvItem.itemName,
                itemQuality = csvItem.itemQuality,
                itemDescription = csvItem.itemDescription,
                itemImage = LoadItemSprite(index.ToString())
            };
            DisplayItem();
        }
        else
        {
            Debug.LogError($"Item with index {index} not found in CSV data.");
        }
    }

    private Sprite LoadItemSprite(string itemId)
    {
        Sprite sprite = Resources.Load<Sprite>($"MaterialItemImages/{itemId}");
        if (sprite == null)
        {
            Debug.LogWarning($"Image for item ID {itemId} not found. Using default image.");
            sprite = Resources.Load<Sprite>("MaterialItemImages/default");
        }
        return sprite;
    }
}
