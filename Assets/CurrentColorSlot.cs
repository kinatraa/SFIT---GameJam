using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrentColorSlot : SlotBase
{
    public Image img;
    
    public void ChangeColor(GameEnum.Color c)
    {
        switch (c)
        {
            case GameEnum.Color.Red:
                img.color = new Color(255, 0, 0);
                break;
            case GameEnum.Color.Orange:
                img.color = new Color(255, 165, 0);
                break;
            case GameEnum.Color.Yellow:
                img.color = new Color(255, 255, 0);
                break;
            case GameEnum.Color.Green:
                img.color = new Color(0, 128, 0);
                break;
            case GameEnum.Color.Blue:
                img.color = new Color(0, 0, 255);
                break;
            case GameEnum.Color.Purple:
                img.color = new Color(128, 0, 128);
                break;
            case GameEnum.Color.Indigo:
                img.color = new Color(75, 0, 130);
                break;
        }
    }
}
