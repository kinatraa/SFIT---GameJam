using System;
using UnityEngine;
using Random = UnityEngine.Random;


public class BoostItem : MonoBehaviour
{
    public GameEnum.BoosterType boosterType;
    private Rigidbody2D rb;

    public void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Vector2 randomDirection = Random.insideUnitCircle.normalized;
        rb.AddForce(randomDirection * 2f, ForceMode2D.Impulse);
    }
}
