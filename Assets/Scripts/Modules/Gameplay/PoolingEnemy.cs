using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PoolingEnemy : MonoBehaviour
{
    public List<GameObject> EnemyPrefabs = new List<GameObject>();
    public Dictionary<GameEnum.Color, Queue<AEnemy>> EnemyPool = new Dictionary<GameEnum.Color, Queue<AEnemy>>();
    
    private int _sideDirection;

    private void Awake()
    {
        foreach (var enemy in EnemyPrefabs)
        {
            for (int i = 0; i < 20; i++)
            {
                AEnemy newEnemy = Instantiate(enemy, transform).GetComponent<AEnemy>();
                newEnemy.gameObject.SetActive(false);
                if (!EnemyPool.ContainsKey(newEnemy.Color))
                {
                    EnemyPool.Add(newEnemy.Color, new Queue<AEnemy>());
                }
                EnemyPool[newEnemy.Color].Enqueue(newEnemy);
            }
        }
    }

    public AEnemy GetFromPool(GameEnum.Color color)
    {
        if (EnemyPool[color].Count > 0)
        {
            return EnemyPool[color].Dequeue();
        }

        return null;
    }

    public void BackToPool(AEnemy enemy)
    {
        EnemyPool[enemy.Color].Enqueue(enemy);
    }

    // private void Spawn(int side)
    // {
    //     Transform _player = GameplayManager.Instance.Player.transform;
    //
    //     if (side == 1)
    //     {
    //         Vector3 spawnPosition = new Vector3(_player.position.x + 25f, _player.position.y, _player.position.z);
    //         SpawnEnemy("Enemy1", spawnPosition, side);
    //     }
    //     else if (side == -1)
    //     {
    //         Vector3 spawnPosition = new Vector3(_player.position.x - 25f, _player.position.y, _player.position.z);
    //         SpawnEnemy("Enemy1", spawnPosition, side);
    //     }
    // }
    //
    // public void SpawnEnemy(string _nameEnemy, Vector3 _position, int side)
    // {
    //     if (_nameEnemy == Define.ENEMY_1)
    //     {
    //         if (_enemy1Queue.Count == 0)
    //         {
    //             Enemy1 _newEnemy1 = Instantiate(_enemy1Prefab, _position, Quaternion.identity);
    //             _enemy1Queue.Enqueue(_newEnemy1);
    //         }
    //
    //         _enemy1Queue.Dequeue().Born(_position, side);
    //     }
    // }
}