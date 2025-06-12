using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayController : MonoBehaviour
{
    public List<SlotBase> slots;
    public Stack<GameEnum.Color> mixColorStack = new Stack<GameEnum.Color>();

    private SlotBase _slotSelected;

    private void Start()
    {
        _slotSelected = slots[1];
        SelectSlot(1);
    }

    public void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Mix();
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SelectSlot(1);
            SetColorCurrentSlot();
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SelectSlot(2);
            SetColorCurrentSlot();

        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SelectSlot(3);
            SetColorCurrentSlot();
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SelectSlot(4);
            SetColorCurrentSlot();
        }

    }

    private void SelectSlot(int slot)
    {
        _slotSelected.DeSelect();
        _slotSelected = slots[slot];
        GameplayManager.Instance.selectColor = _slotSelected.color;
        slots[slot].Select();
    }

    private void SetColorCurrentSlot()
    {
        GameplayManager.Instance.currentColor = _slotSelected.color;
        MessageManager.Instance.SendMessage(new Message(MessageType.OnSetCurrentColor));
    }

    private void Mix()
    {
        if (mixColorStack.Count != 0)
        {
            GameplayManager.Instance.colorInMixStack = mixColorStack.Pop();
            MessageManager.Instance.SendMessage(new Message(MessageType.OnMixColor));
            mixColorStack.Push(GameplayManager.Instance.colorInMixStack);
        }
        else
        {
            mixColorStack.Push(_slotSelected.color);
            GameplayManager.Instance.colorInMixStack = GameEnum.Color.none;
            MessageManager.Instance.SendMessage(new Message(MessageType.OnMixColor));

        }
    }
}
