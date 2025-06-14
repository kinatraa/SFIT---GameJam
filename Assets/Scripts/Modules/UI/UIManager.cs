using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : Singleton<UIManager>, IMessageHandle
{
    public CanvasGame canvasGame;
    public HPBarController hpBarController;
    public TextMeshProUGUI TimeRemainingText;
    
    private void OnEnable()
    {
        MessageManager.Instance.AddSubcriber(MessageType.OnTimeChanged, this);
        MessageManager.Instance.AddSubcriber(MessageType.OnSetCurrentColor, this);
        MessageManager.Instance.AddSubcriber(MessageType.OnMixColor, this);
        MessageManager.Instance.AddSubcriber(MessageType.OnHPChanged, this);

    }

    private void OnDisable()
    {
        MessageManager.Instance.RemoveSubcriber(MessageType.OnTimeChanged, this);
        MessageManager.Instance.RemoveSubcriber(MessageType.OnSetCurrentColor, this);
        MessageManager.Instance.RemoveSubcriber(MessageType.OnMixColor, this);
        MessageManager.Instance.RemoveSubcriber(MessageType.OnHPChanged, this);

    }

    private void UpdateTimeText()
    {
        var m = GameplayManager.Instance.StateManager.MainState as MainState;
        TimeRemainingText.text = TimeSpan.FromSeconds(m.TimeRemaining).ToString(@"mm\:ss");
    }

    public void Handle(Message message)
    {
        switch (message.type)
        {
            case MessageType.OnSetCurrentColor:
                canvasGame.currentColorSlot.ChangeColor(GameplayManager.Instance.currentColor);
                break;
            case MessageType.OnMixColor:
                canvasGame.mixColorSlot.MixColor();
                break;
            case MessageType.OnTimeChanged:
                UpdateTimeText();
                break;
            case MessageType.OnHPChanged:
                hpBarController.UpdateHPBar(GameplayManager.Instance.Player.CurrentHealth, GameplayManager.Instance.Player.MaxHealth);
                break;
        }
    }
}