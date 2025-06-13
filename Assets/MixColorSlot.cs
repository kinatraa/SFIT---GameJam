using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MixColorSlot : SlotBase
{
    public Image img;
    private GameEnum.Color _color1;
    private GameEnum.Color _color2;
    

    public void MixColor()
    {
        _color1 = GameplayManager.Instance.colorInMixStack;
        _color2 = GameplayManager.Instance.selectColor;
        Debug.Log("1" + _color1);
        Debug.Log("2" + _color2);

        if (_color1 == GameEnum.Color.none)
        {
            ChangeColor(_color2);
        }
        else
        {

            if (_color1 == GameEnum.Color.Red)
            {
                if(_color2 == GameEnum.Color.Yellow) ChangeColor(GameEnum.Color.Orange);
                else if(_color2 == GameEnum.Color.Blue) ChangeColor(GameEnum.Color.Purple);
                else ChangeColor(_color2);
            }
            else if (_color1 == GameEnum.Color.Blue)
            {
                if(_color2 == GameEnum.Color.Yellow) ChangeColor(GameEnum.Color.Green);
                else if(_color2 == GameEnum.Color.Red) ChangeColor(GameEnum.Color.Purple);
                else ChangeColor(_color2);
            }
            else if (_color1 == GameEnum.Color.Yellow)
            {
                if(_color2 == GameEnum.Color.Blue) ChangeColor(GameEnum.Color.Green);
                else if(_color2 == GameEnum.Color.Red) ChangeColor(GameEnum.Color.Orange);
                else ChangeColor(_color2);
            }
            else if(_color1 == GameEnum.Color.Purple)
            {
                if(_color2 == GameEnum.Color.Blue) ChangeColor(GameEnum.Color.Indigo);
                else ChangeColor(_color2);
            }
            else  ChangeColor(_color2);

            Debug.Log(GameplayManager.Instance.colorInMixStack);
        }
    }
    
    public void ChangeColor(GameEnum.Color c)
    {
        Color From255(float r, float g, float b) => new Color(r / 255f, g / 255f, b / 255f, 1f);
        
        switch (c)
        {
            case GameEnum.Color.Red:
                img.color = From255(255, 0, 46);
                GameplayManager.Instance.colorInMixStack = GameEnum.Color.Red;
                color = GameEnum.Color.Red;
                break;
            case GameEnum.Color.Orange:
                img.color = From255(255, 125, 18);
                GameplayManager.Instance.colorInMixStack = GameEnum.Color.Orange;
                color = GameEnum.Color.Orange;
                break;
            case GameEnum.Color.Yellow:
                img.color = From255(255, 217, 34);
                GameplayManager.Instance.colorInMixStack = GameEnum.Color.Yellow;
                color = GameEnum.Color.Yellow;
                break;
            case GameEnum.Color.Green:
                img.color = From255(0, 186, 0);
                GameplayManager.Instance.colorInMixStack = GameEnum.Color.Green;
                color = GameEnum.Color.Green;
                break;
            case GameEnum.Color.Blue:
                img.color = From255(117, 176, 225);
                GameplayManager.Instance.colorInMixStack = GameEnum.Color.Blue;
                color = GameEnum.Color.Blue;
                break;
            case GameEnum.Color.Purple:
                img.color = From255(162, 100, 255);
                GameplayManager.Instance.colorInMixStack = GameEnum.Color.Purple;
                color = GameEnum.Color.Purple;
                break;
            case GameEnum.Color.Indigo:
                img.color = From255(43, 36, 158);
                GameplayManager.Instance.colorInMixStack = GameEnum.Color.Indigo;
                color = GameEnum.Color.Indigo;
                break;
            case GameEnum.Color.none:
                img.color = new Color(1,1,1,1);
                //_color1 = GameEnum.Color.Red;
                break;
        }
    }
}
