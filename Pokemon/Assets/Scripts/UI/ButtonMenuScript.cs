using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonMenuScript : MonoBehaviour, IPointerEnterHandler,IPointerExitHandler
{
    public GameObject button;
    public TMP_Text text;
    public string description;
    public void OnPointerEnter(PointerEventData eventData)
    {
        button.SetActive(true);
        text.text = description;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        button.SetActive(false);
        text.text = "";
    }
    
    public void QuitGame()
    {
        Application.Quit();
    }
    
    //faire en sorte qu'appuiller sur B ferme aussi la fenetre
    
    public void CloseWindow(GameObject window)
    {
        window.SetActive(false);
    }
    public void OpenWindow(GameObject window)
    {
        window.SetActive(true);
    }
    public void SaveGame()
    {
        //implementer la save
    }
}
