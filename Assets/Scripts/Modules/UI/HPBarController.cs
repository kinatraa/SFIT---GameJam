using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HPBarController : MonoBehaviour
{
    public Slider HPBar;
    public TextMeshProUGUI _hpText;

    public void UpdateHPBar(float hp)
    {
        _hpText.text = hp.ToString();
        HPBar.value = (hp / 100f);
    }
}
