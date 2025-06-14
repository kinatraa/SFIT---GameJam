using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerController : MonoBehaviour
{
    private Camera _mainCamera;
    private Transform _transform;
    private Rigidbody2D _rb;

    private Vector3 _mousePosition;
    private Vector2 _moveDirection;

    private Transform _powerCircle;
    private SpriteRenderer _powerCircleRenderer;

    [Header("Stats")]
    public float MaxHealth = 100f;
    public float CurrentHealth = 100f;
    public float PowerPoint;
    public float Speed;
    public bool IsRainbowMode = false;

    private float _characterScale;

    private bool _hasTakenDamage = false;
    
    private bool _isKnockback = false;
    private Vector2 _knockbackDirection;
    private float _knockbackTimer = 0f;
    private float _knockbackForce = 10f;

    private bool _dead = false;

    private void Awake()
    {
        _mainCamera = Camera.main;
        _transform = transform;
        _characterScale = transform.localScale.x;
        _rb = GetComponent<Rigidbody2D>();
        CurrentHealth = MaxHealth;

        _powerCircle = _transform.GetChild(0);
        _powerCircleRenderer = _powerCircle.GetComponent<SpriteRenderer>();

        SetCurrentColor();
        IncreaseMaxHealth(0);
    }

    private void Update()
    {
        UpdateMouseDirection();
    }

    private void FixedUpdate()
    {
        if (!_isKnockback && !_dead)
        {
            _rb.velocity = _moveDirection * Speed;
        }
    }

    private void UpdateMouseDirection()
    {
        _mousePosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 currentPosition = _transform.position;
        float distance = Vector2.Distance(currentPosition, _mousePosition);

        if (distance > 0.1f)
        {
            _moveDirection = (_mousePosition - (Vector3)currentPosition).normalized;
        }
        else
        {
            _moveDirection = Vector2.zero;
        }
    }

    public void TakeDamage(AEnemy enemy, float dmg = 10f, float force = 10f)
    {
        if (_hasTakenDamage) return;

        StartCoroutine(ResetTakenDamage());

        Vector2 takeDamageDirection = (transform.position - enemy.transform.position).normalized;

        CurrentHealth -= dmg;
        MessageManager.Instance.SendMessage(new Message(MessageType.OnHPChanged));

        StartCoroutine(StopKnockbackAfterDelay());
        _rb.velocity = Vector2.zero;
        
        if (CurrentHealth <= 0f)
        {
            _dead = true;
            GameplayManager.Instance.CameraStopFollowPlayer();
            _rb.AddForce(takeDamageDirection * 100f, ForceMode2D.Impulse);
        }
        else
        {
            _rb.AddForce(takeDamageDirection * 30f, ForceMode2D.Impulse);
        }
        
        
    }
    
    private IEnumerator StopKnockbackAfterDelay(float delay = 0.3f)
    {
        _isKnockback = true;
        yield return new WaitForSeconds(delay);
        _isKnockback = false;
    }

    private IEnumerator ResetTakenDamage()
    {
        _hasTakenDamage = true;
        yield return new WaitForSeconds(2f);
        _hasTakenDamage = false;
    }

    public void IncreaseMaxHealth(float s = 10)
    {
        MaxHealth += s;
        CurrentHealth += s;
        MessageManager.Instance.SendMessage(new Message(MessageType.OnHPChanged));
    }

    public void SetCurrentColor()
    {
        Color c = GameplayManager.Instance.GetCurrentColor();
        c.a = 33f / 255f;
        _powerCircleRenderer.color = c;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Item"))
        {
            MessageManager.Instance.SendMessage(new Message(MessageType.OnCollectItem));
            BoostItem item = other.GetComponent<BoostItem>();
            if(item.boosterType == GameEnum.BoosterType.Heal)
                GameplayManager.Instance.GameBooster.Heal();
            else if(item.boosterType == GameEnum.BoosterType.IncreaseTime)
                GameplayManager.Instance.GameBooster.IncreaseTime();
            else if(item.boosterType == GameEnum.BoosterType.UpSpeed)
                StartCoroutine(GameplayManager.Instance.GameBooster.UpSpeed(10f));
            else if(item.boosterType == GameEnum.BoosterType.FreezeMode)
                StartCoroutine(GameplayManager.Instance.GameBooster.FreezeMode(10f));
            else StartCoroutine(GameplayManager.Instance.GameBooster.RainbowMode(10f));
            
            Destroy(other.gameObject);
        }
    }
    
    public IEnumerator FlashRandomColor()
    {
        SpriteRenderer sr = _powerCircle.GetComponent<SpriteRenderer>();
        Array colors = Enum.GetValues(typeof(GameEnum.Color));
        
        float flashInterval = 0.05f;

        float timer = 0f;
        while (true)
        {
            int idx = Random.Range(0, 7);
            GameEnum.Color flashColor = (GameEnum.Color)colors.GetValue(idx);
            sr.color = GameplayManager.Instance.GetColor(flashColor);
        
            timer += flashInterval;
            yield return new WaitForSeconds(flashInterval);
        }
    }
}
