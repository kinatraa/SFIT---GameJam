using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Boss : AEnemy
{
    [Header("Stats")] public float MaxHealth = 100f;
    public float CurrentHealth = 100f;

    [Header("AI Behavior")] public float attackRange = 25f;
    public float minAttackDistance = 10f; // Khoảng cách tối thiểu để bắt đầu tấn công
    public float safeDistance = 3f; // Khoảng cách an toàn
    public float circleRadius = 15f; // Bán kính di chuyển vòng tròn

    private bool _isDashing = false;
    private bool _isBackingOff = false;
    private bool _isCircling = false;
    private float _dashCooldown = 0.5f;
    private float _cooldownTimer = 0f;

    private Vector2 _dashDirection;
    private float _dashSpeed = 40f;
    private Vector2 _dashTargetPos;
    private Vector2 _dashStartPos;
    private float _dashDistance = 0f;
    private float _dashTimer = 0f;
    private float _maxDashTime = 1.5f;

    [Header("Movement Settings")] public float backOffDistance = 2f;
    public float backOffSpeed = 18f;
    public float circleSpeed = 12f;
    public float waitTime = 0.2f;

    private bool _hasTakenDamage = false;
    private BossState _currentState = BossState.Observing;
    private float _stateTimer = 0f;
    private Vector2 _circleCenter;
    private float _circleAngle = 0f;
    private int _attackCount = 0;

    private enum BossState
    {
        Observing,
        Circling,
        Preparing,
        Attacking,
        BackingOff,
        Retreating
    }

    protected override void OnEnable()
    {
        RandomColor();
        _currentState = BossState.Observing;
        _stateTimer = 0f;
        _attackCount = 0;
    }

    protected override void FixedUpdate()
    {
        if (_dead)
        {
            _rb.velocity = Vector2.zero;
            return;
        }

        if (_hasTakenDamage) return;

        UpdateAI();
    }

    private void UpdateAI()
    {
        Vector2 playerPos = GameplayManager.Instance.Player.transform.position;
        float distanceToPlayer = Vector2.Distance(transform.position, playerPos);

        _stateTimer += Time.fixedDeltaTime;

        switch (_currentState)
        {
            case BossState.Observing:
                HandleObserving(playerPos, distanceToPlayer);
                break;

            case BossState.Circling:
                HandleCircling(playerPos, distanceToPlayer);
                break;

            case BossState.Preparing:
                HandlePreparing(playerPos, distanceToPlayer);
                break;

            case BossState.Attacking:
                HandleAttacking(playerPos, distanceToPlayer);
                break;

            case BossState.BackingOff:
                break;

            case BossState.Retreating:
                HandleRetreating(playerPos, distanceToPlayer);
                break;
        }
    }

    private void HandleObserving(Vector2 playerPos, float distance)
    {
        _rb.velocity = Vector2.zero;

        if (_stateTimer > 0.5f)
        {
            if (distance < attackRange)
            {
                if (_attackCount >= 4)
                {
                    ChangeState(BossState.Retreating);
                    _attackCount = 0;
                }
                else if (distance >= minAttackDistance)
                {
                    ChangeState(BossState.Preparing);
                }
                else if (distance < safeDistance)
                {
                    ChangeState(BossState.Circling);
                }
                else
                {
                    if (Random.value < 0.6f)
                    {
                        ChangeState(BossState.Preparing);
                    }
                    else
                    {
                        ChangeState(BossState.Circling);
                    }
                }
            }
            else
            {
                Vector2 direction = (playerPos - _rb.position).normalized;
                _rb.velocity = direction * (circleSpeed * 0.6f);
            }
        }
    }

    private void HandleCircling(Vector2 playerPos, float distance)
    {
        _circleCenter = playerPos;
        _circleAngle += circleSpeed * Time.fixedDeltaTime;

        Vector2 offset = new Vector2(Mathf.Cos(_circleAngle), Mathf.Sin(_circleAngle)) * circleRadius;
        Vector2 targetPos = _circleCenter + offset;

        Vector2 direction = (targetPos - _rb.position).normalized;
        _rb.velocity = direction * circleSpeed;

        if (_stateTimer > Random.Range(1f, 2f))
        {
            if (distance >= minAttackDistance && distance <= attackRange && Random.value < 0.7f)
            {
                ChangeState(BossState.Preparing);
            }
            else
            {
                ChangeState(BossState.Observing);
            }
        }
    }

    private void HandlePreparing(Vector2 playerPos, float distance)
    {
        _rb.velocity = Vector2.zero;

        if (_stateTimer > waitTime)
        {
            if (distance <= attackRange && distance >= minAttackDistance)
            {
                DashToPlayer(GameplayManager.Instance.Player.transform);
                ChangeState(BossState.Attacking);
            }
            else if (distance < minAttackDistance)
            {
                ChangeState(BossState.Circling);
            }
            else
            {
                ChangeState(BossState.Observing);
            }
        }
    }

    private void HandleAttacking(Vector2 playerPos, float distance)
    {
        if (_isDashing)
        {
            _dashTimer += Time.fixedDeltaTime;

            float distanceTraveled = Vector2.Distance(_dashStartPos, _rb.position);
            bool reachedMaxDistance = distanceTraveled >= _dashDistance;
            bool nearPlayer = Vector2.Distance(_rb.position, playerPos) <= 2f;
            bool timeOut = _dashTimer >= _maxDashTime;

            if (reachedMaxDistance || nearPlayer || timeOut)
            {
                _isDashing = false;
                _rb.velocity = Vector2.zero;
                _dashTimer = 0f;
                _attackCount++;
                StartCoroutine(SmartBackOff());
            }
            else
            {
                Vector2 move = _dashDirection * (_dashSpeed * Time.fixedDeltaTime);
                _rb.MovePosition(_rb.position + move);
            }
        }
    }

    private void HandleRetreating(Vector2 playerPos, float distance)
    {
        Vector2 retreatDirection = (_rb.position - playerPos).normalized;
        _rb.velocity = retreatDirection * circleSpeed;

        if (_stateTimer > 1.0f || distance > attackRange * 0.8f)
        {
            ChangeState(BossState.Observing);
        }
    }


    private void ChangeState(BossState newState)
    {
        _currentState = newState;
        _stateTimer = 0f;

        if (newState == BossState.Preparing || newState == BossState.BackingOff)
        {
            _rb.velocity = Vector2.zero;
        }
    }

    public void DashToPlayer(Transform player)
    {
        if (_isDashing) return;

        Vector2 direction = (player.position - transform.position).normalized;
        _dashDirection = direction;
        _dashStartPos = _rb.position;
        _dashTimer = 0f;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        _dashDistance = Mathf.Clamp(distanceToPlayer + 2f, 5f, 15f);

        _isDashing = true;
    }

    private IEnumerator SmartBackOff()
    {
        ChangeState(BossState.BackingOff);

        Vector2 playerPos = GameplayManager.Instance.Player.transform.position;
        Vector2 backDirection;

        float distanceToPlayer = Vector2.Distance(transform.position, playerPos);
        if (distanceToPlayer < safeDistance)
        {
            backDirection = (transform.position - (Vector3)playerPos).normalized;
        }
        else
        {
            backDirection = Random.value < 0.4f
                ? -_dashDirection
                : new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
        }

        Vector2 start = _rb.position;
        Vector2 target = start + backDirection * backOffDistance;

        float timer = 0f;
        float duration = backOffDistance / backOffSpeed;

        while (timer < duration)
        {
            Vector2 pos = Vector2.Lerp(start, target, timer / duration);
            _rb.MovePosition(pos);

            timer += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        _rb.MovePosition(target);

        ChangeState(BossState.Observing);
    }

    protected override void TakeDamage(PlayerController player, float force = 100f)
    {
        StartCoroutine(TakenDamageCooldown());

        _takeDamgeDirection = transform.position - player.transform.position;
        _takeDamgeDirection.Normalize();

        CurrentHealth -= 10f;
        if (CurrentHealth <= 0f)
        {
            _rb.AddForce(_takeDamgeDirection * 100f, ForceMode2D.Impulse);
            Death();
        }
        else
        {
            _rb.velocity = Vector2.zero;
            _rb.AddForce(_takeDamgeDirection * 10f, ForceMode2D.Impulse);

            _attackCount = 0;
            ChangeState(BossState.Retreating);
        }
    }

    private IEnumerator TakenDamageCooldown()
    {
        _hasTakenDamage = true;
        _isDashing = false;
        _dashTimer = 0f;

        yield return new WaitForSeconds(0.3f);
        yield return StartCoroutine(FlashRandomColor());
        _hasTakenDamage = false;
        _rb.velocity = Vector2.zero;
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (_hasTakenDamage) return;

            if (!IsEnemyWeaker())
            {
                GameplayManager.Instance.Player.TakeDamage(this, 50f);

                if (_currentState == BossState.Attacking)
                {
                    _isDashing = false;
                    _attackCount++;
                    StartCoroutine(SmartBackOff());
                }
            }
            else
            {
                TakeDamage(GameplayManager.Instance.Player);
            }
        }
    }

    private IEnumerator FlashRandomColor()
    {
        SpriteRenderer sr = _powerCircle.GetComponent<SpriteRenderer>();
        List<GameEnum.Color> colors = new List<GameEnum.Color>()
        {
            GameEnum.Color.Red, GameEnum.Color.Blue, GameEnum.Color.Green, GameEnum.Color.Indigo, GameEnum.Color.Orange,
            GameEnum.Color.Purple, GameEnum.Color.Yellow
        };

        float flashDuration = 0.5f;
        float flashInterval = 0.05f;

        float timer = 0f;
        while (timer < flashDuration)
        {
            int idx = Random.Range(0, colors.Count);
            GameEnum.Color flashColor = colors[idx];
            sr.color = GameplayManager.Instance.GetColor(flashColor);

            timer += flashInterval;
            yield return new WaitForSeconds(flashInterval);
        }

        int finalIdx = Random.Range(0, colors.Count);
        Color = colors[finalIdx];
        sr.color = GameplayManager.Instance.GetColor(Color);
    }

    private void RandomColor()
    {
        Array colors = Enum.GetValues(typeof(GameEnum.Color));
        int idx = Random.Range(0, 7);
        Color = (GameEnum.Color)colors.GetValue(idx);
        _powerCircle.GetComponent<SpriteRenderer>().color = GameplayManager.Instance.GetColor(Color);
    }

    protected override void OnTriggerExit2D(Collider2D other)
    {
    }

    protected override void Death()
    {
        StartCoroutine(WaitForDeath());
    }

    private IEnumerator WaitForDeath()
    {
        yield return new WaitForSeconds(2f);

        gameObject.SetActive(false);
        _dead = false;
        _sr.flipX = false;
        _rb.velocity = Vector2.zero;
    }
}