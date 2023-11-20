using System;
using UnityEngine;

[Serializable]
public class PokemonEcounter
{
    [SerializeField] PokemonSO pokemon;
    [SerializeField] int minLevel;
    [SerializeField] int maxLevel;
    [SerializeField] Rarity rarity;
}