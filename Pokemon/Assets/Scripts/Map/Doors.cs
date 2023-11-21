using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Doors
{
    public static Dictionary<Vector3, int> nextDoorFromPosition = new()
    {
        { new Vector3(-31.9199982f, -5.36002541f, 0f), 10 },
        { new Vector3(-31.2799988f, -2.64002585f, 0f), 4 },
        { new Vector3(-30f, -3.60002637f, 0f), 6 },
        { new Vector3(-28.8800011f, -2.64002585f, 0f), 2 },
        { new Vector3(-27.2800026f, -4.24002647f, 0f), 11 },
        { new Vector3(-22.0000076f, -10.8000202f, 0f), 3 },

        { new Vector3(-20.8800087f, 1.35997403f, 0f), 8 },
        { new Vector3(-20.7200089f, -1.04002571f, 0f), 7 },
        { new Vector3(-19.1200104f, 0.0799741745f, 0f), 12 },

        { new Vector3(-20.560009f, -10.8000202f, 0f), 1 },
        { new Vector3(-20.4000092f, -11.7600193f, 0f), 5 },
    };

    public static Dictionary<int, Vector3> positionFromDoor = new()
    {
        { 1, new Vector3(-31.9199982f, -5.36002541f, 0f) },
        { 2, new Vector3(-31.2799988f, -2.64002585f, 0f) },
        { 3, new Vector3(-30f, -3.60002637f, 0f) },
        { 4, new Vector3(-28.8800011f, -2.64002585f, 0f) },
        { 5, new Vector3(-27.2800026f, -4.24002647f, 0f) },
        { 6, new Vector3(-22.0000076f, -10.8000202f, 0f) },

        { 7, new Vector3(-20.8800087f, 1.35997403f, 0f) },
        { 8, new Vector3(-20.7200089f, -1.04002571f, 0f) },
        { 9, new Vector3(-19.1200104f, 0.0799741745f, 0f) },

        { 10, new Vector3(-20.560009f, -10.8000202f, 0f) },
        { 11, new Vector3(-20.4000092f, -11.7600193f, 0f) },

        { 12, new Vector3(-19.1200104f, 1.51997399f, 0f) },
    };
}