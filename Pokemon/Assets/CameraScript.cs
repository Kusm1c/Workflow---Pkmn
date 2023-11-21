using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public Transform target;
    
    public static CameraScript instance;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void LateUpdate()
    {
        var nextPos = new Vector3(target.position.x, target.position.y, -10);
        // if (CheckIfNextPosIsBorder(nextPos)) return;
        transform.position = nextPos;
    }

    // private bool CheckIfNextPosIsBorder(Vector3 nextPos)
    // {
    //     
    //     
    // }
}
