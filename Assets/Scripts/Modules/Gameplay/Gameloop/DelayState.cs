using System;
using System.Collections;
using System.Collections.Generic;
using Pixelplacement;
using UnityEngine;

public class DelayState : State
{
    public float Duration;

    private void OnEnable()
    {
        StartCoroutine(WaitForDelayState());
    }

    private IEnumerator WaitForDelayState()
    {
        yield return new WaitForSeconds(Duration);

        this.Next();
    }
}
