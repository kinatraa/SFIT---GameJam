using System;
using System.Collections;
using System.Collections.Generic;
using Pixelplacement;
using UnityEngine;

public class MainState : State
{
    private void OnEnable()
    {
        GameplayManager.Instance.EnemySpawner.StartSpawner();
    }

    private void OnDisable()
    {
        GameplayManager.Instance.EnemySpawner.StopSpawner();
    }
}
