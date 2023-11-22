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
    [field: SerializeField] public Stats TotalStats { get; set; }
    [field: SerializeField] public int CatchRate { get; private set; }
    [field: SerializeField] public int BaseXP { get; private set; }
    [field: SerializeField] public int EVYield { get; private set; }
    [field: SerializeField] public int BaseFriendship { get; private set; }
    [field: SerializeField] public int GrowthRate { get; private set; }
    [field: SerializeField] public int Exp { get; private set; }
    [field: SerializeField] public List<LearnableMove> LearnableMoves { get; private set; }
    [field: SerializeField] public List<MoveSO> Moves { get; private set; } 
    
    [field: SerializeField] public Sprite FrontSprite { get; private set; }
    [field: SerializeField] public Sprite BackSprite { get; private set; }
    //set the total stats of the pokemon when the value of base stats, evs, ivs, level, and nature changes
    public void SetTotalStats()
    {
        var totalStats = TotalStats;

        totalStats.HP = Mathf.FloorToInt((((BaseStats.HP + IVs.HP) * 2 + Mathf.Floor(Mathf.Ceil(Mathf.Sqrt(EVs.HP)) / 4f)) * Level) / 100f) + Level + 10;
        totalStats.Attack = Mathf.FloorToInt(((BaseStats.Attack + IVs.Attack) * 2 + Mathf.Floor(Mathf.Ceil(Mathf.Sqrt(EVs.Attack))/4))*Level / 100f) + 5;
        totalStats.Defense = Mathf.FloorToInt(((BaseStats.Defense + IVs.Defense) * 2 + Mathf.Floor(Mathf.Ceil(Mathf.Sqrt(EVs.Defense))/4))*Level / 100f) + 5;
        totalStats.SpAttack = Mathf.FloorToInt(((BaseStats.SpAttack + IVs.SpAttack) * 2 + Mathf.Floor(Mathf.Ceil(Mathf.Sqrt(EVs.SpAttack))/4))*Level / 100f) + 5;
        totalStats.SpDefense = Mathf.FloorToInt(((BaseStats.SpDefense + IVs.SpDefense) * 2 + Mathf.Floor(Mathf.Ceil(Mathf.Sqrt(EVs.SpDefense))/4))*Level / 100f) + 5;
        totalStats.Speed = Mathf.FloorToInt(((BaseStats.Speed + IVs.Speed) * 2 + Mathf.Floor(Mathf.Ceil(Mathf.Sqrt(EVs.Speed))/4))*Level / 100f) + 5;
        TotalStats = totalStats;
    }

    private void OnValidate()
    {
        SetTotalStats();
    }
}