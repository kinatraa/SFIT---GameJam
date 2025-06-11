using System;
using System.Collections;
using UnityEngine;
using Pixelplacement;

public class MainState : State
{
    public float Duration;
    private float _timeRemaining;
    private Coroutine _mainCoroutine;

    public float TimeRemaining => _timeRemaining;

    private void OnEnable()
    {
        GameplayManager.Instance.EnemySpawner.StartSpawner();
        _mainCoroutine = StartCoroutine(WaitForMainState());
    }

    private IEnumerator WaitForMainState()
    {
        _timeRemaining = Duration;

        while (_timeRemaining > 0f)
        {
            _timeRemaining -= Time.deltaTime;
            MessageManager.Instance.SendMessage(new Message(MessageType.OnTimeChanged));
            yield return null;
        }

        this.Next();
    }

    private void OnDisable()
    {
        if (_mainCoroutine != null)
        {
            StopCoroutine(_mainCoroutine);
        }

        GameplayManager.Instance.EnemySpawner.StopSpawner();
    }
}