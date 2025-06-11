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
                img.color = new Color(1f, 0f, 0f, 1f);
                break;
            case GameEnum.Color.Orange:
                img.color = new Color(1f, 0.65f, 0f, 1f);
                break;
            case GameEnum.Color.Yellow:
                img.color = new Color(1f, 1f, 0f, 1f);
                break;
            case GameEnum.Color.Green:
                img.color = new Color(0f, 0.5f, 0f, 1f);
                break;
            case GameEnum.Color.Blue:
                img.color = new Color(0f, 0f, 1f, 1f);
                break;
            case GameEnum.Color.Purple:
                img.color = new Color(0.5f, 0f, 0.5f, 1f);
                break;
            case GameEnum.Color.Indigo:
                img.color = new Color(0.29f, 0f, 0.51f, 1f);
                break;
        }
    }
}
