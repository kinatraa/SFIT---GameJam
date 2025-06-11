using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayController : MonoBehaviour
{
    public GameEnum.Color currentColor;
    public GameEnum.Color selectColor;

    public List<SlotBase> slots;
    public Stack<GameEnum.Color> mixColorStack = new Stack<GameEnum.Color>();

    private SlotBase _slotSelected;
    private CurrentColorSlot _currentColorSlot;

    private void Start()
    {
        _currentColorSlot = slots[0] as CurrentColorSlot;
        _slotSelected = slots[1];
        SelectSlot(1);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SetColorCurrentSlot();
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SelectSlot(1);
            selectColor = slots[1].color;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SelectSlot(2);
            selectColor = slots[2].color;

        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SelectSlot(3);
            selectColor = slots[3].color;

        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SelectSlot(4);
            selectColor = slots[4].color;

        }

    }

    private void SelectSlot(int slot)
    {
        _slotSelected.DeSelect();
        _slotSelected = slots[slot];
        slots[slot].Select();
    }

    private void SetColorCurrentSlot()
    {
        currentColor = _slotSelected.color;
        GameplayManager.Instance.currentColor = currentColor;
        MessageManager.Instance.SendMessage(new Message(MessageType.OnSetCurrentColor));
    }
}
