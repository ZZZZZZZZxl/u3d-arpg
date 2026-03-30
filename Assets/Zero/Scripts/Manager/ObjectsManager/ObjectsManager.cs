using System;
using GGG.Tool.Singleton;
using UnityEngine;

public class ObjectsManager : Singleton<ObjectsManager>
{
    private Transform _mainCamera;
    private GameObject[] _players;

    private void OnEnable()
    {
        if (Camera.main != null) _mainCamera = Camera.main.transform;
        _players = GameObject.FindGameObjectsWithTag("Player");
    }
    

    public Transform MainCamera => _mainCamera;
    public GameObject[] Players => _players;
}