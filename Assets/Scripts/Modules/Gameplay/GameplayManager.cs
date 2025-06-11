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

    private void OnEnable()
    {
        MessageManager.Instance.AddSubcriber(MessageType.OnSetCurrentColor, this);
    }

    private void OnDisable()
    {
        MessageManager.Instance.RemoveSubcriber(MessageType.OnSetCurrentColor, this);
    }

    public void Handle(Message message)
    {
        switch (message.type)
        {
            case MessageType.OnSetCurrentColor:
                UIManager.Instance.canvasGame.currentColorSlot.ChangeColor(currentColor);
                break;
        }
    }
}
