using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Pokemon", menuName = "Pokemon/Create new Pokemon")]
public class PokemonSO : ScriptableObject
{
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public int ID { get; private set; }
    [field: SerializeField] public Gender Gender { get; private set; }
    [field: SerializeField] public int Level { get; private set; }
    [field: SerializeField] public Nature Nature { get; private set; }
    [field: SerializeField] public PokemonType Type { get; private set; }
    [field: SerializeField] public PokemonType Type2 { get; private set; }
    [field: SerializeField] public Stats BaseStats { get; private set; }
    [field: SerializeField] public Stats EVs { get; private set; }
    [field: SerializeField] public Stats IVs { get; private set; }
    [field: SerializeField] public Stats TotalStats { get; private set; }
    [field: SerializeField] public int CatchRate { get; private set; }
    [field: SerializeField] public int BaseXP { get; private set; }
    [field: SerializeField] public int EVYield { get; private set; }
    [field: SerializeField] public int BaseFriendship { get; private set; }
    [field: SerializeField] public int GrowthRate { get; private set; }
    [field: SerializeField] public int Exp { get; private set; }
    [field: SerializeField] public List<LearnableMove> LearnableMoves { get; private set; }

    //set the total stats of the pokemon when the value of base stats, evs, ivs, level, and nature changes
    public void SetTotalStats()
    {
        var totalStats = TotalStats;
        
        totalStats.HP = Mathf.FloorToInt((
            (BaseStats.HP + IVs.HP)
            * 2 + Mathf.FloorToInt
            (
                Mathf.CeilToInt(
                    Mathf.Sqrt(EVs.HP) / 4)
            ) * Level) / 100f) + Level + 10;
        
        totalStats.Attack =
            Mathf.FloorToInt((2 * BaseStats.Attack + IVs.Attack + Mathf.FloorToInt(EVs.Attack / 4)) * Level / 100f) + 5;
        totalStats.Defense =
            Mathf.FloorToInt((2 * BaseStats.Defense + IVs.Defense + Mathf.FloorToInt(EVs.Defense / 4)) * Level / 100f) +
            5;
        totalStats.SpAttack =
            Mathf.FloorToInt(
                (2 * BaseStats.SpAttack + IVs.SpAttack + Mathf.FloorToInt(EVs.SpAttack / 4)) * Level / 100f) + 5;
        totalStats.SpDefense =
            Mathf.FloorToInt((2 * BaseStats.SpDefense + IVs.SpDefense + Mathf.FloorToInt(EVs.SpDefense / 4)) * Level /
                             100f) + 5;
        totalStats.Speed =
            Mathf.FloorToInt((2 * BaseStats.Speed + IVs.Speed + Mathf.FloorToInt(EVs.Speed / 4)) * Level / 100f) + 5;
        TotalStats = totalStats;
    }

    private void OnValidate()
    {
        SetTotalStats();
    }
}