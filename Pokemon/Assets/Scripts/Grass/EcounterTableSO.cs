using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Encounter", menuName = "Pokemon/Encounter")]
public class EcounterTableSO : ScriptableObject
{
    [field:SerializeField] public List<PokemonEcounter> pokemonList;
    [field:SerializeField] public Vector2 grassPositionmin, grassPositionmax;
    [field:SerializeField] public float encounterChance;
}
