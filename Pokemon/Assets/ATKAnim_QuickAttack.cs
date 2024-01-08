using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ATKAnim_QuickAttack : MonoBehaviour
{
    
    GameObject source;
    GameObject target;
    public GameObject impact;
    void Awake()
    {
        source = GameObject.Find("PlayerSprite");
        target = GameObject.Find("OpponentSprite");
        StartCoroutine(QuickAttackAnim());
    }

    IEnumerator QuickAttackAnim()
    {
        Vector3 basePos = source.transform.position;
        for(int i = 10; i >0; i--)
        {
            float angle = i*(2*Mathf.PI/10);
            Vector3 pos = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0);
            source.transform.position = basePos + pos*30;
            yield return new WaitForSeconds(0.02f);
        }
        source.transform.position = basePos;
        Destroy(Instantiate(impact, target.transform.position, Quaternion.identity, transform),0.1f);
        yield return new WaitForSeconds(0.1f);
    }
}
