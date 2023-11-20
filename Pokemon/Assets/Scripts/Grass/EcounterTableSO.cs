using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Encounter", menuName = "Pokemon/Encounter")]
public class EcounterTableSO : ScriptableObject
{
    [field:SerializeField] List<PokemonEcounter> pokemonList;
    [field:SerializeField] List<Vector3> grassPositions;
}
