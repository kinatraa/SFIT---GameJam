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

    public GameBooster GameBooster;
    
    public PlayerController Player;

    public GameEnum.Color currentColor;
    public GameEnum.Color selectColor;
    public GameEnum.Color colorInMixStack;
    public float enemySpeed;

    public void Handle(Message message)
    {

    }

    public Color GetCurrentColor()
    {
        Color From255(float r, float g, float b) => new Color(r / 255f, g / 255f, b / 255f, 1f);

        switch (currentColor)
        {
            case GameEnum.Color.Red:
                return From255(255, 0, 46);
            case GameEnum.Color.Orange:
                return From255(255, 125, 18);
            case GameEnum.Color.Yellow:
                return From255(255, 217, 34);
            case GameEnum.Color.Green:
                return From255(0, 186, 0);
            case GameEnum.Color.Blue:
                return From255(117, 176, 225);
            case GameEnum.Color.Purple:
                return From255(162, 100, 255);
            case GameEnum.Color.Indigo:
                return From255(43, 36, 158);
        }
        
        return Color.white;
    }
}
