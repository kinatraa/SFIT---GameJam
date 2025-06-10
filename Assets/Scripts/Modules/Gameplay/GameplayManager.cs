using System.Collections;
using System.Collections.Generic;
using Pixelplacement;
using UnityEngine;

public class GameplayManager : Singleton<GameplayManager>, IMessageHandle
{
    public StateMachine StateMachine;
    
    public PoolingEnemy PoolingEnemy;
    public EnemySpawner EnemySpawner;
    
    public PlayerController Player;
    
    public void Handle(Message message)
    {
        
    }
}
