using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class ATKAnim_Ember : MonoBehaviour
{
    public GameObject flame1;
    public GameObject flame2;

    public Sprite frame0;
    public Sprite frame1;
    public Sprite frame2;
    public Sprite frame3;
    public Sprite frame4;

    private Vector3 posSource;
    private Vector3 posTarget;

    private void Awake()
    {
        posSource = GameObject.Find("PlayerSprite").transform.position;
        posTarget = GameObject.Find("OpponentSprite").transform.position;
        StartCoroutine(EmberAnim());
    }

    IEnumerator EmberAnim()
    {
        Debug.Log("anim Ember");
        
        for (int i = 0; i < 3; i++)
        {
            GameObject flame = Instantiate(flame1, posSource, Quaternion.identity, transform);
            flame.transform.DOMove(posTarget, 0.5f);
            Destroy(flame,0.51f);
            yield return new WaitForSeconds(0.05f);
        }
        yield return new WaitForSeconds(0.5f);
        Vector3 otherFlamePos = new Vector3(posTarget.x-50, posTarget.y-50, posTarget.z);
        for(int i = 0; i < 3; i++)
        {
            GameObject flame = Instantiate(flame2, otherFlamePos, Quaternion.identity, transform);
            flame.transform.DOMove(posTarget+Vector3.right*100+Vector3.down*50, 0.4f).SetEase(Ease.Linear);
            StartCoroutine(Flame2Anim(flame));
            yield return new WaitForSeconds(0.1f);
        }
    }

    IEnumerator Flame2Anim(GameObject flame)
    {
        Image img = flame.GetComponent<Image>();
        img.sprite = frame4; img.SetNativeSize();
        yield return new WaitForSeconds(0.066f);
        img.sprite = frame0; img.SetNativeSize();
        yield return new WaitForSeconds(0.066f);
        img.sprite = frame1; img.SetNativeSize();
        yield return new WaitForSeconds(0.066f); 
        img.sprite = frame2; img.SetNativeSize();
        yield return new WaitForSeconds(0.066f);
        img.sprite = frame3; img.SetNativeSize();
        yield return new WaitForSeconds(0.066f);
        img.sprite = frame4; img.SetNativeSize();
        yield return new WaitForSeconds(0.066f);
        Destroy(flame);
    }
}


