using System.Collections.Generic;
using UnityEngine;

public class ItemObjectPool : MonoBehaviour
{
    //[SerializeField] private GameObject equipItem;
    [SerializeField] private GameObject materialItem;
    [SerializeField] private int initialPoolSize = 10;

    //private Queue<GameObject> equipItemPool = new Queue<GameObject>();
    private Queue<GameObject> materialItemPool = new Queue<GameObject>();

    private void Awake()
    {
        for (int i = 0; i < initialPoolSize; i++)
        {
            GameObject obj = Instantiate(materialItem);
            obj.SetActive(false);
            materialItemPool.Enqueue(obj);
        }
    }

    public GameObject GetObject()
    {
        if (materialItemPool.Count > 0)
        {
            GameObject obj = materialItemPool.Dequeue();
            obj.SetActive(true);
            return obj;
        }
        else
        {
            GameObject obj = Instantiate(materialItem);
            return obj;
        }
    }

    public void ReturnObject(GameObject obj)
    {
        obj.SetActive(false);
        materialItemPool.Enqueue(obj);
    }
}
