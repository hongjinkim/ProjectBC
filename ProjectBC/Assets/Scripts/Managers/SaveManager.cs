using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System;

[RequireComponent(typeof(GameDataManager))]
public class SaveManager : MonoBehaviour
{
    private GameDataManager _gameDataManager;
    [SerializeField] private string _saveFilename = "savegame.dat";
    [Tooltip("Show Debug messages.")]
    [SerializeField] private bool _debugValues;

    public static event Action<GameData> GameDataLoaded;

    void Awake()
    {
        _gameDataManager = GetComponent<GameDataManager>();
    }
    void OnApplicationQuit()
    {
        SaveGame();
    }

    void OnEnable()
    {

    }

    void OnDisable()
    {

    }
    public GameData NewGame()
    {
        return new GameData();
    }

    public void LoadGame()
    {
        // load saved data from FileDataHandler

        if (_gameDataManager.gameData == null)
        {
            if (_debugValues)
            {
                Debug.Log("GAME DATA MANAGER LoadGame: Initializing game data.");
            }

            _gameDataManager.gameData = NewGame();
        }
        else if (FileManager.LoadFromFile(_saveFilename, out var jsonString))
        {
            _gameDataManager.gameData.LoadJson(jsonString);

            if (_debugValues)
            {
                Debug.Log("SaveManager.LoadGame: " + _saveFilename + " json string: " + jsonString);
            }
        }

        // notify other game objects 
        if (_gameDataManager.gameData != null)
        {
            GameDataLoaded?.Invoke(_gameDataManager.gameData);
        }
    }

    public void SaveGame()
    {
        string jsonFile = _gameDataManager.gameData.ToJson();

        // save to disk with FileDataHandler
        if (FileManager.WriteToFile(_saveFilename, jsonFile) && _debugValues)
        {
            Debug.Log("SaveManager.SaveGame: " + _saveFilename + " json string: " + jsonFile);
        }
    }

    void OnSettingsShown()
    {
        // pass the GameData to the Settings Screen
        if (_gameDataManager.gameData != null)
        {
            GameDataLoaded?.Invoke(_gameDataManager.gameData);
        }
    }

    void OnSettingsUpdated(GameData gameData)
    {
        _gameDataManager.gameData = gameData;
        SaveGame();
    }
}
