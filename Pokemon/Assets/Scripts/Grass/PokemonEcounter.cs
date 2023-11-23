using System;
using UnityEngine;

[Serializable]
public class PokemonEcounter
{
    [SerializeField] public PokemonSO pokemon;
    [SerializeField] public int minLevel;
    [SerializeField] public int maxLevel;
    [SerializeField] public Rarity rarity;
}