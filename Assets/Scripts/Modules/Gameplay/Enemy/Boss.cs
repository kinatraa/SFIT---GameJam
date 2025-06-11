using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : AEnemy
{
    [Header("Stats")] public float MaxHealth = 100f;
    public float CurrentHealth = 100f;

    private void Attack()
    {
        
    }
    
    protected override void TakeDamage(PlayerController player, float force = 100f)
    {
        _takeDamgeDirection = transform.position - player.transform.position;
        _takeDamgeDirection.Normalize();

        CurrentHealth -= 10f;
        if (CurrentHealth <= 0f)
        {
            _rb.AddForce(_takeDamgeDirection * 100f, ForceMode2D.Impulse);
        }
        else
        {
            _rb.AddForce(_takeDamgeDirection * 5f, ForceMode2D.Impulse);
        }
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            TakeDamage(other.GetComponentInParent<PlayerController>());
        }
    }
}