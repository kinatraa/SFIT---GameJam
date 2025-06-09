using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AEnemy : MonoBehaviour
{
    protected Vector3 _takeDamgeDirection;
    protected Rigidbody2D _rb;
    protected float _side;
    public float speed;

    protected virtual void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    protected void Move()
    {
        transform.Translate(new Vector2(-_side, 0) * (Time.deltaTime * speed));
    }

    public virtual void Born(Vector3 position, int side)
    {
        _side = side;
        transform.localScale = new Vector3(-_side, transform.localScale.y, transform.localScale.z);
        transform.position = position;
        gameObject.SetActive(true);
    }

    protected virtual void Death()
    {
        gameObject.SetActive(false);
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
