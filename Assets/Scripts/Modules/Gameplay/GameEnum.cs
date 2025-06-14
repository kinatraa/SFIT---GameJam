using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEnum : MonoBehaviour
{
    public enum Color
    {
        Red,
        Yellow,
        Blue,
        Green,
        Orange,
        Purple,
        Indigo,
        none,
    }
    
    public enum BoosterType
    {
        IncreaseTime,
        Heal,
        UpSpeed,
        RainbowMode,
        FreezeMode,
    }
}
