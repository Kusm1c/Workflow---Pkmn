using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Doors
{
public static Dictionary<int, int> doors = new Dictionary<int, int>()
    {
        { 11, 5 },
        { 5, 11 },
        { 10, 1 },
        { 1, 10 },
        { 6, 3 },
        { 3, 6 },
        { 4, 2 },
        { 2, 4 },
    };   
}
