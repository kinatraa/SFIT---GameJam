using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>, IMessageHandle
{
    public CanvasGame canvasGame;
    
    public void Handle(Message message)
    {
        throw new System.NotImplementedException();
    }
}
