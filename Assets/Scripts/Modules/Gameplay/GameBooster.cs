using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBooster : MonoBehaviour, IMessageHandle
{
    private float saveSpeed;
    private float saveEnemySpeed;
    
    private void Start()
    {
        saveSpeed = GameplayManager.Instance.Player.Speed;
        saveEnemySpeed = GameplayManager.Instance.enemySpeed;
    }

    public void Heal()
    {
        GameplayManager.Instance.Player.CurrentHealth += 50f;
        if (GameplayManager.Instance.Player.CurrentHealth >= GameplayManager.Instance.Player.MaxHealth)
            GameplayManager.Instance.Player.CurrentHealth = GameplayManager.Instance.Player.MaxHealth;
        MessageManager.Instance.SendMessage(new Message(MessageType.OnHPChanged));
    }

    public void IncreaseTime()
    {
        var m = GameplayManager.Instance.StateManager.MainState as MainState;
        var newtime = m.TimeRemaining + 5f;
        m.SetTimeRemaing(newtime);
    }

    public IEnumerator UpSpeed(float time)
    {
        GameplayManager.Instance.Player.Speed += 10f;
        yield return new WaitForSeconds(time);
        GameplayManager.Instance.Player.Speed = saveSpeed;
    }

    private Coroutine rainbowCoroutine;
    
    public IEnumerator RainbowMode(float time)
    {
        GameplayManager.Instance.Player.IsRainbowMode = true;
        if (rainbowCoroutine == null)
        {
            rainbowCoroutine = StartCoroutine(GameplayManager.Instance.Player.FlashRandomColor());
        }
        yield return new WaitForSeconds(time);
        if (rainbowCoroutine != null)
        {
            StopCoroutine(rainbowCoroutine);
            rainbowCoroutine = null;
        }
        GameplayManager.Instance.Player.SetCurrentColor();
        GameplayManager.Instance.Player.IsRainbowMode = false;
    }

    public IEnumerator FreezeMode(float time)
    {
        var speed = GameplayManager.Instance.enemySpeed;
        GameplayManager.Instance.enemySpeed /= 2;
        yield return new WaitForSeconds(time);
        GameplayManager.Instance.enemySpeed = saveEnemySpeed;
    }
    
    public void Handle(Message message)
    {
        throw new System.NotImplementedException();
    }
    
}
