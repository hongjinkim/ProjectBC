using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EventType
{
    FundsUpdated,
    LevelUpdated,
    BattlePointUpdated,
    ItemUpdated,
    HeroUpdated
}

public class EventManager : MonoSingleton<EventManager>
{
    private Dictionary<EventType, Action<Dictionary<string, object>>> eventDictionary;

    private void Awake()
    {
        if (eventDictionary == null)
        {
            eventDictionary = new Dictionary<EventType, Action<Dictionary<string, object>>>();
        }
    }

    public void StartListening(EventType eventName, Action<Dictionary<string, object>> listener)
    {
        Action<Dictionary<string, object>> thisEvent;

        if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent += listener;
            instance.eventDictionary[eventName] = thisEvent;
        }
        else
        {
            thisEvent += listener;
            instance.eventDictionary.Add(eventName, thisEvent);
        }
    }

    public void StopListening(EventType eventName, Action<Dictionary<string, object>> listener)
    {
        Action<Dictionary<string, object>> thisEvent;
        if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent -= listener;
            instance.eventDictionary[eventName] = thisEvent;
        }
    }

    public void TriggerEvent(EventType eventName, Dictionary<string, object> message)
    {
        Action<Dictionary<string, object>> thisEvent = null;
        if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.Invoke(message);
        }
    }

    //// 사용 예시
    //EventManager.TriggerEvent("addCoins", new Dictionary<string, object> { { "amount", 1 } });
    //void OnEnable()
    //{
    //    EventManager.StartListening("addCoins", OnAddCoins);
    //}

    //void OnDisable()
    //{
    //    EventManager.StopListening("addCoins", OnAddCoins);
    //}

    //void OnAddCoins(Dictionary<string, object> message)
    //{
    //    var amount = (int)message["amount"];
    //    coins += amount;
    //}
}
