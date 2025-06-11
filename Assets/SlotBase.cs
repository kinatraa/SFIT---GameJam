using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SlotBase : MonoBehaviour
{
    public GameEnum.Color color;
    public GameObject backgroundSelect;
    
     
     public void Select()
     {
         backgroundSelect.SetActive(true);
     }
    
    
     public void DeSelect() 
     {
         backgroundSelect.SetActive(false);
     }
}
