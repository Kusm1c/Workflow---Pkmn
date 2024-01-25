using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.Video;

public class VideoManager : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public VideoClip boyCase;
    public VideoClip girlCase;
    public GameObject genderChoice;
    public TMP_InputField nameInput;
    
    
    private bool isWaitingForInput = false;
    public int segment = 0;
    [FormerlySerializedAs("timestamps")] public List<float> baseTimestamps;
    public List<float> boyTimestamps;
    public List<float> girlTimestamps;
    
    private List<float> currentTimestamps;
    private void Start()
    {
        currentTimestamps = baseTimestamps;
        StartCoroutine(PlayForSeconds(currentTimestamps[segment]));
        segment++;
    }

    void Update()
    {
        if(isWaitingForInput && Input.GetKeyDown(KeyCode.Space))
        {
            isWaitingForInput = false;
            StartCoroutine(PlayForSeconds(currentTimestamps[segment]));
            segment++;
        }
    }
    
    IEnumerator PlayForSeconds(float seconds)
    {
        videoPlayer.Play();
        yield return new WaitForSeconds(seconds);
        videoPlayer.Pause();
        isWaitingForInput = true;
        switch (segment)
        {
            default:
                break;
            case 10:
                genderChoice.SetActive(true);
                isWaitingForInput = false;
                break;
            case 12:
                nameInput.gameObject.SetActive(true);
                nameInput.Select();
                break;
            case 15:
                SceneManager.LoadScene("SampleScene");
                break;
        }
        
        
        
    }

    public void BoyChoice()
    {
        print("test");
        currentTimestamps.AddRange(boyTimestamps);
        videoPlayer.clip = boyCase;
        StartCoroutine( PlayForSeconds(currentTimestamps[segment]));
        segment++;
        PlayerPrefs.SetString("Gender", "boy");
        genderChoice.SetActive(false);
    }

    public void nameTyped()
    {
        PlayerPrefs.SetString("PlayerName", nameInput.text);
    }
    
}
