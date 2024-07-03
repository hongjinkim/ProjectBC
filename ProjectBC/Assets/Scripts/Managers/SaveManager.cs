using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System;
using UnityEditor.PackageManager;

[RequireComponent(typeof(GameDataManager))]
public class SaveManager : MonoBehaviour
{
    private GameDataManager _gameDataManager;
    

    [SerializeField] private string _saveFilename = "savegame.dat";


    [SerializeField] private bool _debugValues;
    [SerializeField] private bool _resetGame;

    public static event Action<PlayerInfo> GameDataLoaded;

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
    public PlayerInfo NewGame()
    {
        return new PlayerInfo();
    }

    public void LoadGame()
    {
        // load saved data from FileDataHandler

        if (_gameDataManager.playerInfo == null || _resetGame)
        {
            if (_debugValues)
            {
                Debug.Log("GAME DATA MANAGER LoadGame: Initializing game data.");
            }

            _gameDataManager.playerInfo = NewGame();
        }
        else if (FileManager.LoadFromFile(_saveFilename, out var jsonString))
        {
            _gameDataManager.playerInfo.LoadJson(jsonString);

            if (_debugValues)
            {
                Debug.Log("SaveManager.LoadGame: " + _saveFilename + " json string: " + jsonString);
            }
        }

        // notify other game objects 
        if (_gameDataManager.playerInfo != null)
        {
            GameDataLoaded?.Invoke(_gameDataManager.playerInfo);
        }
    }

    public void SaveGame()
    {
        string jsonFile = _gameDataManager.playerInfo.ToJson();

        // save to disk with FileDataHandler
        if (FileManager.WriteToFile(_saveFilename, jsonFile) && _debugValues)
        {
            Debug.Log("SaveManager.SaveGame: " + _saveFilename + " json string: " + jsonFile);
        }
    }

    void OnSettingsShown()
    {
        // pass the GameData to the Settings Screen
        if (_gameDataManager.playerInfo != null)
        {
            GameDataLoaded?.Invoke(_gameDataManager.playerInfo);
        }
    }

    void OnSettingsUpdated(PlayerInfo playerInfo)
    {
        _gameDataManager.playerInfo = playerInfo;
        SaveGame();
    }
}
