using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuAnimation : MonoBehaviour
{
    [Header("Anim 1")]
    public GameObject anim1;
    public GameObject tmScreen,
        background1Up, background1Down,
        star, lilStar,bigStar,StarParent,
        logoText,logoImage;
    [Header("Anim 2")]
    public GameObject anim2;

    public GameObject grassAndTree;
    [Header("secondAct")]
    public GameObject secondAct;
    public GameObject bushes;
    public GameObject trees;
    
    [Header("thirdAct")]
    public GameObject thirdAct;
    public GameObject gengar;
    public GameObject nidoran;
    
    [Header("fourthAct")]
    public GameObject fourthAct;
    public GameObject gengarFourthAct;
    public GameObject nidoranFourthAct;
    public GameObject lilBushe;
    public GameObject treesFourthAct;
    public GameObject gengarAtack;
    public GameObject mask;
    public GameObject mask2;
    public Sprite nMouthClosed, nPrepScream, nScream, nJump;
    public Sprite gBase, gPawUp, gPawDown;
    
    [Header("Anim 3")]
    public GameObject anim3;

    public GameObject shine;
    public GameObject florizarreBW;
    public GameObject OrangeBG;
    public Image bwMask;
    public GameObject greenStripes;
    
    public GameObject florizarreColor;
    public Image colorMask;

    public GameObject logo;
    public Image logoMask;
    
    public GameObject version;
    public Image versionMask;
    
    [Header("mainMenu")]
    public GameObject mainMenuPanel;
    void Start()
    {
        StartCoroutine(StartAnimation1());
    }
    
    IEnumerator StartAnimation1()
    {
        anim1.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        tmScreen.GetComponent<Image>().DOColor(Color.clear, 1f);                                                //premier ecran des trademarks
        yield return new WaitForSeconds(1f);
        background1Down.transform.DOMove(background1Down.transform.position+Vector3.down*155, 0.2f);      //l'ecran se sépare en deux et forme des bordures
        background1Up.transform.DOMove(background1Up.transform.position+Vector3.up*155, 0.2f);
        yield return new WaitForSeconds(0.1f);
        star.transform.DOLocalMove(star.transform.localPosition*-1, 1f).SetEase(Ease.Linear);                   //l'etoile jaune se déplace
        for (int i = 0; i < 50; i++) //des petites etoiles apparaissent dans le chemin de l'etoile jaune
        {
            Vector3 pos = star.transform.position;
            pos.x += Random.Range(-16, 16);
            pos.y += Random.Range(-16, 16);
            GameObject st = Instantiate(lilStar, pos, Quaternion.identity, StarParent.transform);
            Vector2 force = new Vector2(Random.Range(0.2f, 0.5f), Random.Range(-0.5f, 0.5f));
            st.GetComponent<Rigidbody2D>().AddForce(force * 5000); //elles sont propulsées dans une direction aléatoire
            StartCoroutine(StarDeath(st, st.GetComponent<Image>(),
                1.5f + Random.Range(-0.5f, 0.5f))); //elles meurent au bout d'un certain temps
            yield return new WaitForSeconds(0.02f);
        }
        
        for (int i = 0; i < 40; i++)                                                                                     //des petites etoiles apparaissent dans la ou il y a le logo
        {
            Vector3 pos = logoText.transform.position;
            pos.x += Random.Range(-220, 220);
            pos.y += Random.Range(0, 20);
            GameObject st = Instantiate(lilStar, pos, Quaternion.identity, StarParent.transform);
            st.GetComponent<Rigidbody2D>().gravityScale = 5f;
            StartCoroutine(StarDeath(st,st.GetComponent<Image>(), 1.5f+Random.Range(-0.5f,0.5f)));    //elles meurent au bout d'un certain temps
            yield return new WaitForSeconds(0.05f);
        }
        logoText.GetComponent<Image>().DOColor(Color.white, 4f);
        for (int i = 0; i < 40; i++)                                                                                     //des petites etoiles apparaissent dans la ou il y a le logo
        {
            Vector3 pos = logoText.transform.position;
            pos.x += Random.Range(-220, 220);
            pos.y += Random.Range(-20, 20);
            GameObject st = Instantiate(lilStar, pos, Quaternion.identity, StarParent.transform);
            st.GetComponent<Rigidbody2D>().gravityScale = 5f;
            StartCoroutine(StarDeath(st,st.GetComponent<Image>(), 1.5f+Random.Range(-0.5f,0.5f)));   //elles meurent au bout d'un certain temps
            if (i % 5 == 0)                                                                                             // faire apparaitre des grosses etoiles
            {
                Vector3 pos2 = logoText.transform.position;
                pos.x += Random.Range(-150, 150);
                pos.y += Random.Range(-20, 20);
                GameObject st2 = Instantiate(bigStar, pos, Quaternion.identity, StarParent.transform);
                Destroy(st2,1f);
            }
            yield return new WaitForSeconds(0.05f);
        }
        logoImage.GetComponent<Image>().DOColor(Color.white, 2f); 
        yield return new WaitForSeconds(2.5f);
        logoText.GetComponent<Image>().DOColor(Color.clear, 1f);
        logoImage.GetComponent<Image>().DOColor(Color.clear, 1f);
        yield return new WaitForSeconds(1f);
        anim1.SetActive(false);
        StartCoroutine(StartAnimation2());
    }

    IEnumerator StartAnimation2()
    {
        anim2.SetActive(true);
        grassAndTree.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        grassAndTree.transform.DOScale(Vector3.one*2.5f,0.2f);
        foreach (Transform child in grassAndTree.transform)
        {
            child.GetComponent<Image>().DOColor(Color.clear, 0.2f);
        }
        yield return new WaitForSeconds(0.2f);
        grassAndTree.SetActive(false);
        //yield return new WaitForSeconds(0.1f);
        
        secondAct.SetActive(true);
        bushes.transform.DOMove(bushes.transform.position+Vector3.left*240*2,1f).SetEase(Ease.Linear);
        trees.transform.DOMove(trees.transform.position+Vector3.right*240*2,1f).SetEase(Ease.Linear);
        yield return new WaitForSeconds(1f);
        secondAct.SetActive(false);
        
        thirdAct.SetActive(true);
        gengar.transform.DOMove(gengar.transform.position+Vector3.up*20,1f).SetEase(Ease.Linear);
        nidoran.transform.DOMove(nidoran.transform.position+Vector3.down*20,1f).SetEase(Ease.Linear);
        yield return new WaitForSeconds(1f);
        thirdAct.SetActive(false);
        
        fourthAct.SetActive(true);
        gengarFourthAct.transform.DOMove(gengarFourthAct.transform.position+Vector3.left*750,1f).SetEase(Ease.Linear);
        nidoranFourthAct.transform.DOMove(nidoranFourthAct.transform.position+Vector3.right*750,1f).SetEase(Ease.Linear);
        lilBushe.transform.DOMove(lilBushe.transform.position+Vector3.left*1000,1f).SetEase(Ease.Linear);
        treesFourthAct.transform.DOMove(treesFourthAct.transform.position+Vector3.right*1000,1f).SetEase(Ease.Linear);
        yield return new WaitForSeconds(1f);
        lilBushe.transform.DOMove(lilBushe.transform.position+Vector3.left*500,5f).SetEase(Ease.Linear);
        treesFourthAct.transform.DOMove(treesFourthAct.transform.position+Vector3.right*500,6f).SetEase(Ease.Linear);
        Image nidoranImg = nidoranFourthAct.GetComponent<Image>();
        Image gengarImg = gengarFourthAct.GetComponent<Image>();
        yield return new WaitForSeconds(0.5f);
        nidoranImg.sprite = nPrepScream;
        yield return new WaitForSeconds(0.1f);
        nidoranImg.sprite = nScream;
        //make nidoran shake up and down for 0.5sec
        for (int i = 0; i < 10; i++)
        {
            nidoranFourthAct.transform.DOMove(nidoranFourthAct.transform.position+Vector3.up*10,0.05f).SetEase(Ease.Linear);
            yield return new WaitForSeconds(0.05f);
            nidoranFourthAct.transform.DOMove(nidoranFourthAct.transform.position+Vector3.down*10,0.05f).SetEase(Ease.Linear);
            yield return new WaitForSeconds(0.05f);
        }
        nidoranImg.sprite = nMouthClosed;
        yield return new WaitForSeconds(0.5f);
        gengarImg.sprite = gPawUp;
        gengarFourthAct.transform.DOMove(gengarFourthAct.transform.position+Vector3.left*80,0.5f).SetEase(Ease.Linear);
        nidoranImg.sprite = nPrepScream;
        yield return new WaitForSeconds(0.5f);
        gengarImg.sprite = gPawDown;
        nidoranImg.sprite = nJump;
        gengarFourthAct.transform.DOMove(gengarFourthAct.transform.position+Vector3.right*80,0.1f).SetEase(Ease.Linear);
        nidoranFourthAct.transform.DOMove(nidoranFourthAct.transform.position+Vector3.right*50+Vector3.up*70,0.2f).SetEase(Ease.Linear);
        gengarAtack.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        gengarImg.sprite = gBase;
        gengarAtack.SetActive(false);
        yield return new WaitForSeconds(0.1f);
        nidoranFourthAct.transform.DOMove(nidoranFourthAct.transform.position+Vector3.right*50+Vector3.down*70,0.2f).SetEase(Ease.Linear);
        yield return new WaitForSeconds(0.2f);
        nidoranImg.sprite = nPrepScream;
        nidoranFourthAct.transform.DOMove(nidoranFourthAct.transform.position+Vector3.right*50,0.2f).SetEase(Ease.Linear);
        yield return new WaitForSeconds(0.2f);
        nidoranImg.sprite = nMouthClosed;
        yield return new WaitForSeconds(0.5f);
        
        nidoranImg.sprite = nJump;
        nidoranFourthAct.transform.DOMove(nidoranFourthAct.transform.position+Vector3.left*75+Vector3.up*100,0.3f).SetEase(Ease.Linear);
        yield return new WaitForSeconds(0.3f);
        nidoranFourthAct.transform.DOMove(nidoranFourthAct.transform.position+Vector3.left*75+Vector3.down*100,0.3f).SetEase(Ease.Linear);
        yield return new WaitForSeconds(0.3f);
        nidoranImg.sprite = nPrepScream;
        yield return new WaitForSeconds(0.2f);
        nidoranImg.sprite = nMouthClosed;
        
        for(int i = 0; i<2; i++)
        {
            nidoranImg.sprite = nJump;
            nidoranFourthAct.transform.DOMove(nidoranFourthAct.transform.position+Vector3.right*30+Vector3.up*50,0.1f).SetEase(Ease.Linear);
            yield return new WaitForSeconds(0.1f);
            nidoranFourthAct.transform.DOMove(nidoranFourthAct.transform.position+Vector3.right*30+Vector3.down*50,0.1f).SetEase(Ease.Linear);
            yield return new WaitForSeconds(0.1f);
            nidoranImg.sprite = nPrepScream;
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(0.5f);
        nidoranImg.sprite = nJump;
        nidoranFourthAct.transform.DOMove(nidoranFourthAct.transform.position+Vector3.left*150+Vector3.up*100,1f).SetEase(Ease.Linear);
        mask.GetComponent<Image>().DOColor(Color.white, 2f);
        yield return new WaitForSeconds(1f);
        nidoranFourthAct.transform.DOMove(nidoranFourthAct.transform.position+Vector3.left*50+Vector3.down*50,1f);
        gengarFourthAct.transform.DOScale(Vector3.one*2, 1f);
        nidoranFourthAct.transform.DOScale(Vector3.one*2, 1f);
        mask2.GetComponent<Image>().DOColor(Color.black, 1f);
        yield return new WaitForSeconds(1f);
        fourthAct.SetActive(false);
        anim2.SetActive(false);
        StartCoroutine(StartAnimation3());
        
        
        
        
        yield return null;
    }

    IEnumerator StartAnimation3()
    {
        yield return new WaitForSeconds(1f);
        anim3.SetActive(true);
        shine.transform.DOMove(shine.transform.position+Vector3.up*350, 0.5f).SetEase(Ease.Linear);
        yield return new WaitForSeconds(2f);
        florizarreBW.GetComponent<Image>().DOColor(Color.white, 2f);
        yield return new WaitForSeconds(2f);
        
        OrangeBG.transform.DOScale(Vector3.one*1, 0.2f);//oranges stripes appear
        bwMask.DOColor(Color.white, 0.1f);
        yield return new WaitForSeconds(0.1f);
        bwMask.DOColor(Color.clear, 0.1f);
        yield return new WaitForSeconds(0.1f);

        yield return new WaitForSeconds(0.5f);
        
        greenStripes.transform.DOScale(Vector3.one*1, 0.2f);//oranges stripes appear
        bwMask.DOColor(Color.white, 0.1f);
        yield return new WaitForSeconds(0.1f);
        bwMask.DOColor(Color.clear, 0.1f);
        yield return new WaitForSeconds(0.1f);
        
        yield return new WaitForSeconds(0.5f);
        
        bwMask.DOColor(Color.white, 0.1f);
        yield return new WaitForSeconds(0.1f);
        florizarreColor.SetActive(true);
        florizarreBW.SetActive(false);
        
        logo.SetActive(true);
        logoMask.DOColor(Color.clear, 1f);
        version.SetActive(true);
        versionMask.DOColor(Color.clear, 1f);
        colorMask.DOColor(Color.clear, 1f);
        yield return new WaitForSeconds(1f);
        
        yield return new WaitForSeconds(10f);
        
        anim3.SetActive(false);
        SceneManager.LoadScene("MainMenuButtons");
        yield return null;
    }


    IEnumerator StarDeath(GameObject st,Image img, float timeToLive)
    {
        yield return new WaitForSeconds(timeToLive - 0.5f);
        Destroy(st,0.5f);
        for(int k = 0; k<10; k++)
        {
            if(!img) break;
            img.color = Color.clear;
            yield return new WaitForSeconds(0.025f);
            if(!img) break;
            img.color = Color.white;
            yield return new WaitForSeconds(0.025f);
        }
        yield return null;
    }
}
