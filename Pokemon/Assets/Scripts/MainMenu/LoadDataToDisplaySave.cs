using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        Color colorGender = PlayerPrefs.GetString("Gender") == "boy"? boyFontColor : girlFontColor;
        foreach (var text in textToChangeColor)
        {
           
            text.color = colorGender;
        }
        playerName.text = PlayerPrefs.GetString("PlayerName", "Player");
        timePlayed.text = PlayerPrefs.GetInt("TimePlayed",0).ToString();
        badges.text = PlayerPrefs.GetInt("Badges",0).ToString();
    }

    public void NewGame()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
