using System;
using UnityEngine;

[Serializable]
public struct Stats
{
    [Range(0, 255)] public int HP;
    [Range(0, 255)] public int Attack;
    [Range(0, 255)] public int Defense;
    [Range(0, 255)] public int SpAttack;
    [Range(0, 255)] public int SpDefense;
    [Range(0, 255)] public int Speed;
}