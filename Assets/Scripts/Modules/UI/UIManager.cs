using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>, IMessageHandle
{
    public CanvasGame canvasGame;
    
    private void OnEnable()
    {
        MessageManager.Instance.AddSubcriber(MessageType.OnSetCurrentColor, this);
        MessageManager.Instance.AddSubcriber(MessageType.OnMixColor, this);

    }

    private void OnDisable()
    {
        MessageManager.Instance.RemoveSubcriber(MessageType.OnSetCurrentColor, this);
        MessageManager.Instance.RemoveSubcriber(MessageType.OnMixColor, this);

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
        }
    }
}
