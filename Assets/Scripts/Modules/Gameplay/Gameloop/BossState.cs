using System;
using System.Collections;
using System.Collections.Generic;
using Pixelplacement;
using UnityEngine;

public class BossState : State
{
    private void OnEnable()
    {
        GameplayManager.Instance.EnemySpawner.SpawnBoss();
    }

    private void OnDisable()
    {
        
    }
}
