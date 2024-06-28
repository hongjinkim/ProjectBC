using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(SaveManager))]
public class GameDataManager : MonoBehaviour
{
    [SerializeField] private PlayerInfo _playerinfo;
    public PlayerInfo playerInfo { set => _playerinfo = value; get => _playerinfo; }

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
