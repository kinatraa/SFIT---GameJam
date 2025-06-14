using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : AEnemy
{
    [Header("Stats")] 
    public float MaxHealth = 100f;
    public float CurrentHealth = 100f;

    private bool _isDashing = false;
    private float _dashDuration = 0.6f;
    private float _dashTimer = 0f;
    private float _dashCooldown = 1f;
    private float _cooldownTimer = 0f;

    private Vector2 _dashStartPosition;
    private Vector2 _dashTargetPosition;

    protected override void FixedUpdate()
    {
        if (_dead)
        {
            _rb.velocity = Vector2.zero;
            return;
        }

        if (_isDashing)
        {
            _dashTimer -= Time.fixedDeltaTime;

            float t = 1f - (_dashTimer / _dashDuration);
            Vector2 newPos = Vector2.Lerp(_dashStartPosition, _dashTargetPosition, t);
            _rb.MovePosition(newPos);

            if (_dashTimer <= 0f)
            {
                _isDashing = false;
                _cooldownTimer = _dashCooldown;
            }
        }
        else
        {
            if (_cooldownTimer > 0f)
            {
                _cooldownTimer -= Time.fixedDeltaTime;
            }
            else
            {
                DashToPlayer(GameplayManager.Instance.Player.transform);
            }
        }
    }

    public void DashToPlayer(Transform player)
    {
        if (_isDashing) return;

        _dashStartPosition = transform.position;
        _dashTargetPosition = player.position;
        _dashTimer = _dashDuration;
        _isDashing = true;
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
        if (other.CompareTag("Player") && _isDashing)
        {
            GameplayManager.Instance.Player.TakeDamage(this);
            _isDashing = false;
            _cooldownTimer = _dashCooldown;
        }
    }

    protected override void OnTriggerExit2D(Collider2D other)
    {
        
    }
}
