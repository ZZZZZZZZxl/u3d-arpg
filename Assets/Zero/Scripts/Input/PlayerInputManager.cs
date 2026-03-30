using System.Collections;
using System.Collections.Generic;
using GGG.Tool.Singleton;
using UnityEngine;

public class PlayerInputManager : Singleton<PlayerInputManager>
{
    private PlayerInputAction _playerInputAction;
    private PlayerInputAction.PlayerActions _playerActions;

    public PlayerInputAction PlayerInputAction => _playerInputAction;
    public PlayerInputAction.PlayerActions PlayerActions => _playerActions;
    
    protected override void Awake()
    {
        base.Awake();

        _playerInputAction ??= new PlayerInputAction();
        _playerActions = _playerInputAction.Player;
    }

    private void OnEnable()
    {
        _playerInputAction.Enable();
    }

    private void OnDisable()
    {
        _playerInputAction.Disable();
    }

}
