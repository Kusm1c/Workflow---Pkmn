using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LoadDataToDisplaySave : MonoBehaviour
{
    // Start is called before the first frame update
    public Color boyFontColor;
    public Color girlFontColor;
    
    public TMP_Text[] textToChangeColor;
    
    public TMP_Text playerName;
    public TMP_Text timePlayed;
    public TMP_Text badges;
    void Start()
    {
        //load data
        LoadData();
    }

    public void LoadData()
    {
        //todo
        //get player gender
        //Color colorGender = (data.getGender=="boy")?boyFontColor:girlFontColor;
        Color colorGender = girlFontColor;
        foreach (var text in textToChangeColor)
        {
           
            text.color = colorGender;
        }
        //change text to matche data
    }
}
