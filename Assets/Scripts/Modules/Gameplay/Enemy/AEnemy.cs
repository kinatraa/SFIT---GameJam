using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public abstract class AEnemy : MonoBehaviour
{
    protected Vector3 _takeDamgeDirection;
    protected Transform _transform;
    protected Rigidbody2D _rb;
    protected SpriteRenderer _sr;
    protected float _side;
    protected bool _dead = false;
    
    [Header("Stats")]
    public float Speed;
    public float PowerPoint;
    public float MinPowerPoint;
    public GameEnum.Color Color;
    
    protected Transform _powerCircle;
    private float _characterScale;

    private void Awake()
    {
        _transform = transform;
        _rb = GetComponent<Rigidbody2D>();
        _sr = GetComponent<SpriteRenderer>();
        
        _powerCircle = _transform.GetChild(0);
        _characterScale = _transform.localScale.x;
    }

    protected void OnEnable()
    {
        if (Random.Range(0f, 100) < 80)
        {
            PowerPoint = MinPowerPoint;
        }
        else
        {
            PowerPoint = Random.Range(MinPowerPoint, MinPowerPoint * 3);
        }
        _powerCircle.localScale = new Vector3(PowerPoint / _characterScale, PowerPoint / _characterScale, 1);
    }

    protected void OnDisable()
    {
        _powerCircle.localScale = new Vector3(MinPowerPoint / _characterScale, MinPowerPoint / _characterScale, 1);
        _rb.velocity = Vector2.zero;
    }

    protected virtual void FixedUpdate()
    {
        if (!_dead)
        {
            Move();
        }
    }

    protected void Move()
    {
        _rb.velocity = new Vector2(-_side * GameplayManager.Instance.enemySpeed * Speed, _rb.velocity.y);
    }

    public virtual void Born(Vector3 position, int side)
    {
        _side = side;
        if(!_sr) _sr = GetComponent<SpriteRenderer>();
        _sr.flipX = side == 1;
        transform.position = position;
        gameObject.SetActive(true);
    }

    protected virtual void Death()
    {
        gameObject.SetActive(false);
        _dead = false;
        _sr.flipX = false;
        _rb.velocity = Vector2.zero;
        GameplayManager.Instance.PoolingEnemy.BackToPool(this);
    }

    protected virtual void TakeDamage(PlayerController player, float force = 100f)
    {
        _takeDamgeDirection = transform.position - player.transform.position;
        _takeDamgeDirection.Normalize();
        _rb.velocity = Vector2.zero;
        _rb.AddForce(_takeDamgeDirection * force, ForceMode2D.Impulse);
        player.IncreasePowerPoint();
    }

    protected virtual void DropItem()
    {
        var num = Random.Range(0, 5);
        Instantiate(GameplayManager.Instance.items[num], transform.position, Quaternion.identity);
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (PowerPoint <= GameplayManager.Instance.Player.PowerPoint || GameplayManager.Instance.Player.IsRainbowMode)
            {
                _dead = true;
                TakeDamage(GameplayManager.Instance.Player);
                if (Random.value <= 0.3f)
                {
                    DropItem();
                }   
            }
            else
            {
                GameplayManager.Instance.Player.TakeDamage(this);
            }
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("MainCamera") && _dead)
        {
            Death();
        }
    }
}
