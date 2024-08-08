using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class BattleScreen : MainScreen
{

    [Header("Object Pool")]
    public ObjectPoolBehaviour objectPool;
    private WaitForSeconds wait = new WaitForSeconds(2.0f);

    private void OnEnable()
    {
        EventManager.StartListening(EventType.ItemPickup, OnPickup);
    }
    private void OnDisable()
    {
        EventManager.StopListening(EventType.ItemPickup, OnPickup);
    }


    void OnPickup(Dictionary<string, object> message)
    {
        StartCoroutine(PickupNotice((string)message["gold"]));
        StartCoroutine(PickupNotice((string)message["item"]));
    }
    

    IEnumerator PickupNotice(string text)
    {
        GameObject textObject = objectPool.GetPooledObject();
        textObject.GetComponent<ItemPickupText>().SetText(text);
        textObject.SetActive(true);

        yield return wait;

        textObject.SetActive(false);
    }
}
