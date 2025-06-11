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
    public GameEnum.Color Color;
    
    protected Transform _powerCircle;
    private float _originalCircleScale;

    private float _randomScale;

    private void Awake()
    {
        _transform = transform;
        _rb = GetComponent<Rigidbody2D>();
        _sr = GetComponent<SpriteRenderer>();
        
        _powerCircle = _transform.GetChild(0);
        _originalCircleScale = _powerCircle.localScale.x;
    }

    protected void OnEnable()
    {
        _randomScale = Random.Range(1f, 3f);
        _powerCircle.localScale = new Vector3(_randomScale * _originalCircleScale, _randomScale * _originalCircleScale, 1);
        PowerPoint *= _randomScale;
        // _powerCircle.localScale = new Vector3(PowerPoint, PowerPoint, 1);
    }

    protected void OnDisable()
    {
        _powerCircle.localScale = new Vector3(_originalCircleScale, _originalCircleScale, 1);
        PowerPoint /= _randomScale;
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
        transform.Translate(new Vector2(-_side, 0) * (Time.deltaTime * Speed));
    }

    public virtual void Born(Vector3 position, int side)
    {
        _side = side;
        // transform.localScale = new Vector3(-_side, transform.localScale.y, transform.localScale.z);
        if (side == 1)
        {
            _sr.flipX = true;
        }
        else
        {
            _sr.flipX = false;
        }
        transform.position = position;
        gameObject.SetActive(true);
    }

    protected virtual void Death()
    {
        gameObject.SetActive(false);
        _dead = false;
        _sr.flipX = false;
        GameplayManager.Instance.PoolingEnemy.BackToPool(this);
    }

    protected virtual void TakeDamage(PlayerController player, float force = 100f)
    {
        _takeDamgeDirection = transform.position - player.transform.position;
        _takeDamgeDirection.Normalize();
        // Debug.Log(_takeDamgeDirection);
        _rb.AddForce(_takeDamgeDirection * force, ForceMode2D.Impulse);
        player.IncreasePowerPoint();
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _dead = true;
            TakeDamage(GameplayManager.Instance.Player);
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("MainCamera"))
        {
            if (_dead)
            {
                Death();
            }
        }
    }
}
