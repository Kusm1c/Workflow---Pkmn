using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelUpdateFrame : MonoBehaviour
{
    private Image img;
    public void Start()
    {
        OptionManager.OnFenetreStyleChange += UpdateFrame;
        img = GetComponent<Image>();
        img.sprite = OptionManager.Instance.fenetreStyleSprites[OptionManager.Instance.fenetreStyle-1];
        
    }

    private void UpdateFrame(int num)
    {
        img.sprite = OptionManager.Instance.fenetreStyleSprites[num];
    }
}
