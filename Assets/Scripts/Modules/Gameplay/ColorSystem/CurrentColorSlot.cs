using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrentColorSlot : SlotBase
{
    public Image img;
    
    public void ChangeColor(GameEnum.Color c)
    {
        Color From255(float r, float g, float b) => new Color(r / 255f, g / 255f, b / 255f, 1f);

        switch (c)
        {
            case GameEnum.Color.Red:
                img.color = From255(255, 0, 46);
                break;
            case GameEnum.Color.Orange:
                img.color = From255(255, 125, 18);
                break;
            case GameEnum.Color.Yellow:
                img.color = From255(255, 217, 34);
                break;
            case GameEnum.Color.Green:
                img.color = From255(0, 186, 0);
                break;
            case GameEnum.Color.Blue:
                img.color = From255(117, 176, 225);
                break;
            case GameEnum.Color.Purple:
                img.color = From255(162, 100, 255);
                break;
            case GameEnum.Color.Indigo:
                img.color = From255(43, 36, 158);
                break;
        }
    }
}
