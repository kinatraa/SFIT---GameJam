using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class AEnemy : MonoBehaviour
{
    protected Vector3 _takeDamgeDirection;
    protected Rigidbody2D _rb;
    protected SpriteRenderer _sr;
    protected float _side;
    protected bool _dead = false;
    
    [Header("Stats")]
    public float Speed;
    public float PowerPoint;
    public GameEnum.Color Color;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _sr = GetComponent<SpriteRenderer>();
    }

    protected virtual void Update()
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

    protected virtual void TakeDamage(PlayerController player)
    {
        _takeDamgeDirection = transform.position - player.transform.position;
        _takeDamgeDirection.Normalize();
        Debug.Log(_takeDamgeDirection);
        _rb.AddForce(_takeDamgeDirection * 1000f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _dead = true;
            TakeDamage(other.GetComponentInParent<PlayerController>());
        }
    }

    protected void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("MainCamera"))
        {
            Death();
        }
    }
}
