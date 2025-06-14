using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasGame : MonoBehaviour
{
    public CurrentColorSlot currentColorSlot;
    public MixColorSlot mixColorSlot;

    private void Start()
    {
        UIManager.Instance.canvasStart.Hide();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UIManager.Instance.canvasMenu.Hide();
        }
    }
}
