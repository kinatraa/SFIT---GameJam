using System;
using System.Collections;
using System.Collections.Generic;
using Pixelplacement;
using UnityEngine;

public class GameplayManager : Singleton<GameplayManager>, IMessageHandle
{
    public StateMachine StateMachine;
    public StateManager StateManager;
    
    public PoolingEnemy PoolingEnemy;
    public EnemySpawner EnemySpawner;
    
    public PlayerController Player;

    public GameEnum.Color currentColor;
    public GameEnum.Color selectColor;
    public GameEnum.Color colorInMixStack;

    public void Handle(Message message)
    {

    }
}
