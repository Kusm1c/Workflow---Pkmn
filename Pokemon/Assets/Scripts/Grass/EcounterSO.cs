using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Encounter", menuName = "Pokemon/Encounter")]
public class EcounterSO : ScriptableObject
{
    [field:SerializeField] List<PokemonEcounter> pokemonList;
}
