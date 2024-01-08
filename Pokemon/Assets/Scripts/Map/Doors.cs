using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Doors
{
    public static Dictionary<Vector3, int> nextDoorFromPosition = new()
    {
        { new Vector3(-31.9199982f,-5.84002495f,0f), 10 }, // out
        { new Vector3(-31.2799988f,-2.96002603f,0f), 4 },
        { new Vector3(-30f,-4.08002663f,0f), 6 }, // out
        { new Vector3(-28.8800011f,-2.96002603f,0f), 2 },
        { new Vector3(-27.2800026f,-4.72002602f,0f), 11 }, // out
        { new Vector3(-22.0000076f, -10.8000202f, 0f), 3 },

        { new Vector3(-20.8800087f, 1.35997403f, 0f), 8 },
        { new Vector3(-20.7200089f, -1.04002571f, 0f), 7 },
        { new Vector3(-19.1200104f, 0.0799741745f, 0f), 12 },

        { new Vector3(-20.560009f, -10.8000202f, 0f), 1 },
        { new Vector3(-20.4000092f, -11.7600193f, 0f), 5 },
    };

    public static Dictionary<int, Vector3> positionFromDoor = new()
    {
        { 1, new Vector3(-31.9199982f,-5.84002495f,0f) },
        { 2, new Vector3(-31.2799988f,-2.96002603f,0f) },
        { 3, new Vector3(-30f,-3.92002654f,0f) },
        { 4, new Vector3(-28.8800011f,-2.96002603f,0f) },
        { 5, new Vector3(-27.2800026f,-4.72002602f,0f) },
        { 6, new Vector3(-22.0000076f, -10.8000202f, 0f) },

        { 7, new Vector3(-20.8800087f, 1.35997403f, 0f) },
        { 8, new Vector3(-20.7200089f, -1.04002571f, 0f) },
        { 9, new Vector3(-19.1200104f, 0.0799741745f, 0f) },

        { 10, new Vector3(-20.560009f, -10.8000202f, 0f) },
        { 11, new Vector3(-20.4000092f, -11.7600193f, 0f) },

        { 12, new Vector3(-19.1200104f, 1.51997399f, 0f) },
    };
    
    public static Dictionary<int, Vector3> blockedCameraPositionWhenTookDoor = new()
    {
        { 1, Vector3.zero },
        { 2, new Vector3(-31.7599983f,-3.2800262f,-10f) },
        { 3, Vector3.zero },
        { 4, Vector3.zero },
        { 5, Vector3.zero },
        { 6, Vector3.zero },

        { 7, Vector3.zero },
        { 8, Vector3.zero },
        { 9, Vector3.zero },

        { 10, Vector3.zero },
        { 11, Vector3.zero },

        { 12, Vector3.zero },
    };
    
    public static Dictionary<int, Vector3> maxBlockedCameraPositionWhenTookDoor = new()
    {
        { 1, Vector3.zero },
        { 2, new Vector3(-31.7599983f,-3.2800262f,-10f) },
        { 3, new Vector3(-29.5200005f,-3.2800262f,-10f) },
        { 4, new Vector3(-29.5200005f,-3.2800262f,-10f) },
        { 5, Vector3.zero },
        { 6, Vector3.zero },

        { 7, Vector3.zero },
        { 8, Vector3.zero },
        { 9, Vector3.zero },

        { 10, Vector3.zero },
        { 11, Vector3.zero },

        { 12, Vector3.zero },
    };
    
    public static Dictionary<int, Vector3> minBlockedCameraPositionWhenTookDoor = new()
    {
        { 1, Vector3.zero },
        { 2, new Vector3(-31.7599983f,-3.2800262f,-10f) },
        { 3, new Vector3(-29.6800003f,-3.2800262f,-10f) },
        { 4, new Vector3(-29.6800003f,-3.2800262f,-10f) },
        { 5, Vector3.zero },
        { 6, Vector3.zero },

        { 7, Vector3.zero },
        { 8, Vector3.zero },
        { 9, Vector3.zero },

        { 10, Vector3.zero },
        { 11, Vector3.zero },

        { 12, Vector3.zero },
    };
}