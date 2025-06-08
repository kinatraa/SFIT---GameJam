using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PoolingEnemy : Singleton<PoolingEnemy>
{
    [SerializeField] private PlayerController _player;
    
    [SerializeField] private Enemy1 _enemy1Prefab;

    private Queue<Enemy1> _enemy1Queue = new Queue<Enemy1>();
    
    
    public void SpawnEnemy(string _nameEnemy, Vector3 _position)
    {
        if (_nameEnemy == Define.ENEMY_1)
        {
            if (_enemy1Queue.Count == 0)
            {
                Enemy1 _newEnemy1 = Instantiate(_enemy1Prefab, _position, Quaternion.identity);
                _enemy1Queue.Enqueue(_newEnemy1);
            }
            _enemy1Queue.Dequeue().Born(_position);
        }
    }

    
    public void BackToPool(AEnemy enemy)
    {
        switch (enemy)
        {
            case Enemy1 _enemy1:
                _enemy1Queue.Enqueue(_enemy1);
                break;
        }
    }
    
    
    //Test

    private void Update()
    {
        Spawn();
    }

    private void Spawn()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            Vector3 spawnPosition = new Vector3(_player.transform.position.x + 15f, _player.transform.position.y,
                _player.transform.position.z);
            SpawnEnemy("Enemy1", spawnPosition);
        }
    }
}

