using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : Singleton<UIManager>, IMessageHandle
{
    public CanvasGame canvasGame;
    
    public TextMeshProUGUI TimeRemainingText;

    private void UpdateTimeRemainingText()
    {
        var mainState = GameplayManager.Instance.StateManager.MainState as MainState;
        int timeRemaining = (int)mainState.TimeRemaining;
        TimeRemainingText.text = timeRemaining.ToString();
    }
    
    public void Handle(Message message)
    {
        switch (message.type)
        {
            case MessageType.OnTimeChanged:
                UpdateTimeRemainingText();
                break;
        }
    }
    
    private void OnEnable()
    {
        MessageManager.Instance.AddSubcriber(MessageType.OnTimeChanged, this);
    }
    
    private void OnDisable()
    {
        MessageManager.Instance.RemoveSubcriber(MessageType.OnTimeChanged, this);
    }
}