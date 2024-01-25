using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OptionManager : MonoBehaviour
{
    public static OptionManager Instance;
    
    private int vitesseText;
    public int fenetreStyle;
    public GameObject menuActionPanel;
    public GameObject optionPanel;

    public TMP_Text vitesseTMP;
    public TMP_Text fenetreStyleTMP;
    
    public static event Action<int> OnFenetreStyleChange;
    
    public Sprite[] fenetreStyleSprites;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("Tentative de création d'une autre instance de OptionManager, détruite immédiatement.");
            Destroy(gameObject);
            return;
        }
        vitesseText = PlayerPrefs.GetInt("vitesseText", 2);
        fenetreStyle = PlayerPrefs.GetInt("fenetreStyle", 1);
        if (vitesseTMP && fenetreStyleTMP)
        {
            
            vitesseTMP.text = vitesseText.ToString();
            
            fenetreStyleTMP.text = fenetreStyle.ToString();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (menuActionPanel.activeSelf)
            {
                menuActionPanel.SetActive(false);
            }
            else
            {
                menuActionPanel.SetActive(true);
            }
        }
    }

    public void ChangeVitesseText()
    {
        vitesseText++;
        if(vitesseText>3) vitesseText = 1;
        vitesseTMP.text = vitesseText.ToString();
        PlayerPrefs.SetInt("vitesseText", vitesseText);
    }

    public void ChangeFenetreStyle()
    {
        fenetreStyle++;
        if(fenetreStyle>10) fenetreStyle = 1;
        fenetreStyleTMP.text = fenetreStyle.ToString();
        PlayerPrefs.SetInt("fenetreStyle", fenetreStyle);
        OnFenetreStyleChange?.Invoke(fenetreStyle-1);
    }
    
    public void CloseOption()
    {
        optionPanel.SetActive(false);
    }
}
