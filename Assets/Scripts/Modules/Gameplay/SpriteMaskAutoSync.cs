using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteMaskAutoSync : MonoBehaviour
{
    private SpriteRenderer _sr;
    private SpriteMask _mask;

    private void Awake()
    {
        _sr = GetComponent<SpriteRenderer>();
        _mask = GetComponent<SpriteMask>();
    }

    private void Update()
    {
        _mask.sprite = _sr.sprite;
    }
}
