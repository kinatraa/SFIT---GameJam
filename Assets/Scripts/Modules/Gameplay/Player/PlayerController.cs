using System;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{
    private Camera _mainCamera;
    private Transform _transform;
    private Rigidbody2D _rb;
    
    private Vector3 _mousePosition;
    private Vector2 _direction;
    private float _mouseDistance;

    private Transform _powerCircle;
    private float _originalCircleScale;
    
    [Header("Stats")]
    public float MaxHealth = 100f;
    public float CurrentHealth = 100f;
    public float PowerPoint;
    public float Speed;

    private float OriginalPowerPoint = 5f;

    private void Awake()
    {
        _mainCamera = Camera.main;
        _transform = transform;
        _rb = GetComponent<Rigidbody2D>();
        CurrentHealth = MaxHealth;
        
        _powerCircle = _transform.GetChild(0);
        _originalCircleScale = _powerCircle.localScale.x;
    }

    void Update()
    {
        MoveToMousePosition();
    }

    private void MoveToMousePosition()
    {
        _mousePosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        _mouseDistance = Vector2.Distance(transform.position, _mousePosition);
        if (_mouseDistance > 0.1f)
        {
            _direction = _mousePosition - transform.position;
            _direction.Normalize();
            transform.Translate(_direction * (Time.deltaTime * Speed));
        }
    }

    public void TakeDamage(AEnemy enemy, float dmg = 10f, float force = 100f)
    {
        var takeDamgeDirection = transform.position - enemy.transform.position;
        takeDamgeDirection.Normalize();
        
        CurrentHealth -= dmg;
        if (CurrentHealth <= 0)
        {
            _rb.AddForce(takeDamgeDirection * 100f, ForceMode2D.Impulse);
        }
        else
        {
            _rb.AddForce(takeDamgeDirection * 5f, ForceMode2D.Impulse);
        }
    }

    public void IncreasePowerPoint(float s = 0.1f)
    {
        _powerCircle.localScale += new Vector3(s, s, 0);
        float ratio = _powerCircle.localScale.x / _originalCircleScale;
        PowerPoint = OriginalPowerPoint * ratio;
    }
}
