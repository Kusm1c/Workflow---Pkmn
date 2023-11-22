using System;
using UnityEngine;

[Serializable]
public class LearnableMove
{
    [field: SerializeField] public int Level { get; private set; }
    [field: SerializeField] public MoveSO Move { get; private set; }
}