using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(SaveManager))]

public class GameDataManager : MonoBehaviour
{
    [SerializeField] private GameData _gameData;
    public GameData gameData { set => _gameData = value; get => _gameData; }

    private SaveManager _saveManager;

    private void OnEnable()
    {

    }

    private void OnDisable()
    {

    }

    private void Awake()
    {
        _saveManager = GetComponent<SaveManager>();
    }

    void Start()
    {
        //if saved data exists, load saved data
        _saveManager?.LoadGame();
    }
}
