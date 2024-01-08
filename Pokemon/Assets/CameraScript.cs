using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public Transform target;
    
    public static CameraScript instance;
    
    public Vector3 cameraBlockedPosition;
    public Vector3 maxCameraPosition;
    public Vector3 minCameraPosition;

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
        if (!target) return;
        if (cameraBlockedPosition != Vector3.zero)
        {
            transform.position = cameraBlockedPosition;
            return;
        }
        if (maxCameraPosition != Vector3.zero && minCameraPosition != Vector3.zero)
        {
            transform.position = new Vector3(
                Mathf.Clamp(target.position.x, minCameraPosition.x, maxCameraPosition.x),
                Mathf.Clamp(target.position.y, minCameraPosition.y, maxCameraPosition.y),
                -10
            );
            return;
        }
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
