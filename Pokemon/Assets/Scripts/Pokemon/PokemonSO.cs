using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Pokemon", menuName = "Pokemon/Create new Pokemon")]
public class PokemonSO : ScriptableObject
{
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public int ID { get; private set; }
    [field: SerializeField] public PokemonType Type { get; private set; }
    [field: SerializeField] public PokemonType Type2 { get; private set; }
    [field: SerializeField] public int HP { get; private set; }
    [field: SerializeField] public int Attack { get; private set; }
    [field: SerializeField] public int Defense { get; private set; }
    [field: SerializeField] public int SpAttack { get; private set; }
    [field: SerializeField] public int SpDefense { get; private set; }
    [field: SerializeField] public int Speed { get; private set; }
    [field: SerializeField] public int Total { get; private set; }
    [field: SerializeField] public int CatchRate { get; private set; }
    [field: SerializeField] public int BaseXP { get; private set; }
    [field: SerializeField] public int EVYield { get; private set; }
    [field: SerializeField] public int BaseFriendship { get; private set; }
    [field: SerializeField] public int GrowthRate { get; private set; }
    [field: SerializeField] public int Exp { get; private set; }
    [field: SerializeField] public int EggCycles { get; private set; }
    [field: SerializeField] public int EVs { get; private set; }
    [field: SerializeField] public List<LearnableMoveSO> LearnableMoves { get; private set; }
}