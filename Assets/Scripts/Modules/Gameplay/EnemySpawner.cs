using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    public void StartSpawner()
    {
        StartCoroutine(SpawnEnemy());
    }

    private IEnumerator SpawnEnemy()
    {
        var stateMachine = GameplayManager.Instance.StateMachine;
        var pool = GameplayManager.Instance.PoolingEnemy;
        Array colors = Enum.GetValues(typeof(GameEnum.Color));
        Transform _player = GameplayManager.Instance.Player.transform;
        while (true)
        {
            int idx = Random.Range(0, 7);
            var e = pool.GetFromPool((GameEnum.Color)colors.GetValue(idx));
            
            if (Random.Range(0, 2) <= 0)
            {
                Vector3 spawnPosition = new Vector3(_player.position.x + 25f, _player.position.y, _player.position.z);
                e.Born(spawnPosition, 1);
            }
            else
            {
                Vector3 spawnPosition = new Vector3(_player.position.x - 25f, _player.position.y, _player.position.z);
                e.Born(spawnPosition, -1);
            }
            yield return new WaitForSeconds(1f);
        }
    }

    public void StopSpawner()
    {
        StopAllCoroutines();
    }

    public void SpawnBoss()
    {
        var pool = GameplayManager.Instance.PoolingEnemy.BossPool;
        Transform player = GameplayManager.Instance.Player.transform;
        int idx = Random.Range(0, pool.Count);
        if (Random.Range(0, 2) <= 0)
        {
            Vector3 spawnPosition = new Vector3(player.position.x + 25f, player.position.y, player.position.z);
            pool[idx].Born(spawnPosition, 1);
        }
        else
        {
            Vector3 spawnPosition = new Vector3(player.position.x - 25f, player.position.y, player.position.z);
            pool[idx].Born(spawnPosition, -1);
        }
    }
}
