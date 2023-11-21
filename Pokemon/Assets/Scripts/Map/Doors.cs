using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Doors
{
    public static Dictionary<Vector3, int> nextDoorFromPosition = new()
    {
        { new Vector3(-31.9199982f,-5.04002571f,0f), 10 },
        { new Vector3(-31.2799988f,-2.64002585f,0f), 4 },
        { new Vector3(-30.3199997f,-3.60002637f,0f), 6 },
        { new Vector3(-29.2000008f,-2.64002585f,0f), 2 },
        { new Vector3(-27.920002f,-4.24002647f,0f), 11 },
        { new Vector3(-22.0000076f,-10.8000202f,0f), 3 },
        
        { new Vector3(-20.8800087f,0.239974171f,0f), 8 },
        { new Vector3(-20.8800087f,1.35997403f,0f), 7 },
        { new Vector3(-20.7200089f,-1.04002571f,0f), 12 },
        
        { new Vector3(-20.560009f,-10.8000202f,0f), 1 },
        { new Vector3(-20.4000092f,-11.7600193f,0f), 5 },
    };
}
