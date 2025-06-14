using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBooster : MonoBehaviour, IMessageHandle
{

    public void Heal()
    {
        GameplayManager.Instance.Player.CurrentHealth += 5f;
        if (GameplayManager.Instance.Player.CurrentHealth > 100)
            GameplayManager.Instance.Player.CurrentHealth = 100;
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
        var speed = GameplayManager.Instance.Player.Speed;
        GameplayManager.Instance.Player.Speed += 10f;
        yield return new WaitForSeconds(time);
        GameplayManager.Instance.Player.Speed = speed;
    }

    public IEnumerator RainbowMode(float time)
    {
        GameplayManager.Instance.Player.IsRainbowMode = true;
        yield return new WaitForSeconds(time);
        GameplayManager.Instance.Player.IsRainbowMode = false;
    }

    public IEnumerator FreezeMode(float time)
    {
        var speed = GameplayManager.Instance.enemySpeed;
        GameplayManager.Instance.enemySpeed /= 2;
        yield return new WaitForSeconds(time);
        GameplayManager.Instance.enemySpeed = speed;
    }
    
    public void Handle(Message message)
    {
        throw new System.NotImplementedException();
    }
    
}
