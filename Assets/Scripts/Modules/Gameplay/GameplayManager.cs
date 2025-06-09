using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : Singleton<GameplayManager>, IMessageHandle
{
    public PoolingEnemy PoolingEnemy;
    
    public void Handle(Message message)
    {
        throw new System.NotImplementedException();
    }
}
