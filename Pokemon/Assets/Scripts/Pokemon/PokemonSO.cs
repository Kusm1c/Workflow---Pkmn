using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Pokemon", menuName = "Pokemon/Create new Pokemon")]
public class PokemonSO : ScriptableObject
{
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public int ID { get; private set; }
    [field: SerializeField] public Gender Gender { get; private set; }
    [field: SerializeField] public int Level { get; set; }
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
    [field: SerializeField] public int Exp { get; set; }
    [field: SerializeField] public List<LearnableMove> LearnableMoves { get; private set; }
    [field: SerializeField] public List<MoveSO> Moves { get; private set; } 
    
    [field: SerializeField] public Sprite FrontSprite { get; private set; }
    [field: SerializeField] public Sprite BackSprite { get; private set; }
    //set the total stats of the pokemon when the value of base stats, evs, ivs, level, and nature changes
    public void SetTotalStats()
    {
        var totalStats = TotalStats;

        totalStats.MaxHP = Mathf.FloorToInt((((BaseStats.HP + IVs.HP) * 2 + Mathf.Floor(Mathf.Ceil(Mathf.Sqrt(EVs.HP)) / 4f)) * Level) / 100f) + Level + 10;
        totalStats.HP = totalStats.MaxHP;
        totalStats.Attack = Mathf.FloorToInt(((BaseStats.Attack + IVs.Attack) * 2 + Mathf.Floor(Mathf.Ceil(Mathf.Sqrt(EVs.Attack))/4))*Level / 100f) + 5;
        totalStats.Defense = Mathf.FloorToInt(((BaseStats.Defense + IVs.Defense) * 2 + Mathf.Floor(Mathf.Ceil(Mathf.Sqrt(EVs.Defense))/4))*Level / 100f) + 5;
        totalStats.SpAttack = Mathf.FloorToInt(((BaseStats.SpAttack + IVs.SpAttack) * 2 + Mathf.Floor(Mathf.Ceil(Mathf.Sqrt(EVs.SpAttack))/4))*Level / 100f) + 5;
        totalStats.SpDefense = Mathf.FloorToInt(((BaseStats.SpDefense + IVs.SpDefense) * 2 + Mathf.Floor(Mathf.Ceil(Mathf.Sqrt(EVs.SpDefense))/4))*Level / 100f) + 5;
        totalStats.Speed = Mathf.FloorToInt(((BaseStats.Speed + IVs.Speed) * 2 + Mathf.Floor(Mathf.Ceil(Mathf.Sqrt(EVs.Speed))/4))*Level / 100f) + 5;
        switch (Nature)
        {
            case Nature.Bashful or Nature.Docile or Nature.Hardy or Nature.Quirky or Nature.Serious:
                break;
            case Nature.Adamant:
                totalStats.Attack = Mathf.FloorToInt(totalStats.Attack * 1.1f);
                totalStats.SpAttack = Mathf.FloorToInt(totalStats.SpAttack * 0.9f);
                break;
            case Nature.Bold:
                totalStats.Defense = Mathf.FloorToInt(totalStats.Defense * 1.1f);
                totalStats.Attack = Mathf.FloorToInt(totalStats.Attack * 0.9f);
                break;
            case Nature.Brave:
                totalStats.Attack = Mathf.FloorToInt(totalStats.Attack * 1.1f);
                totalStats.Speed = Mathf.FloorToInt(totalStats.Speed * 0.9f);
                break;
            case Nature.Calm:
                totalStats.SpDefense = Mathf.FloorToInt(totalStats.SpDefense * 1.1f);
                totalStats.Attack = Mathf.FloorToInt(totalStats.Attack * 0.9f);
                break;
            case Nature.Careful:
                totalStats.SpDefense = Mathf.FloorToInt(totalStats.SpDefense * 1.1f);
                totalStats.SpAttack = Mathf.FloorToInt(totalStats.SpAttack * 0.9f);
                break;
            case Nature.Gentle:
                totalStats.SpDefense = Mathf.FloorToInt(totalStats.SpDefense * 1.1f);
                totalStats.Defense = Mathf.FloorToInt(totalStats.Defense * 0.9f);
                break;
            case Nature.Hasty:
                totalStats.Speed = Mathf.FloorToInt(totalStats.Speed * 1.1f);
                totalStats.Defense = Mathf.FloorToInt(totalStats.Defense * 0.9f);
                break;
            case Nature.Impish:
                totalStats.Defense = Mathf.FloorToInt(totalStats.Defense * 1.1f);
                totalStats.SpAttack = Mathf.FloorToInt(totalStats.SpAttack * 0.9f);
                break;
            case Nature.Jolly:
                totalStats.Speed = Mathf.FloorToInt(totalStats.Speed * 1.1f);
                totalStats.SpAttack = Mathf.FloorToInt(totalStats.SpAttack * 0.9f);
                break;
            case Nature.Lax:
                totalStats.Defense = Mathf.FloorToInt(totalStats.Defense * 1.1f);
                totalStats.SpDefense = Mathf.FloorToInt(totalStats.SpDefense * 0.9f);
                break;
            case Nature.Lonely:
                totalStats.Attack = Mathf.FloorToInt(totalStats.Attack * 1.1f);
                totalStats.Defense = Mathf.FloorToInt(totalStats.Defense * 0.9f);
                break;
            case Nature.Mild:
                totalStats.SpAttack = Mathf.FloorToInt(totalStats.SpAttack * 1.1f);
                totalStats.Defense = Mathf.FloorToInt(totalStats.Defense * 0.9f);
                break;
            case Nature.Modest:
                totalStats.SpAttack = Mathf.FloorToInt(totalStats.SpAttack * 1.1f);
                totalStats.Attack = Mathf.FloorToInt(totalStats.Attack * 0.9f);
                break;
            case Nature.Naive:
                totalStats.Speed = Mathf.FloorToInt(totalStats.Speed * 1.1f);
                totalStats.SpDefense = Mathf.FloorToInt(totalStats.SpDefense * 0.9f);
                break;
            case Nature.Naughty:
                totalStats.Attack = Mathf.FloorToInt(totalStats.Attack * 1.1f);
                totalStats.SpDefense = Mathf.FloorToInt(totalStats.SpDefense * 0.9f);
                break;
            case Nature.Quiet:
                totalStats.SpAttack = Mathf.FloorToInt(totalStats.SpAttack * 1.1f);
                totalStats.Speed = Mathf.FloorToInt(totalStats.Speed * 0.9f);
                break;
            case Nature.Rash:
                totalStats.SpAttack = Mathf.FloorToInt(totalStats.SpAttack * 1.1f);
                totalStats.SpDefense = Mathf.FloorToInt(totalStats.SpDefense * 0.9f);
                break;
            case Nature.Relaxed:
                totalStats.Defense = Mathf.FloorToInt(totalStats.Defense * 1.1f);
                totalStats.Speed = Mathf.FloorToInt(totalStats.Speed * 0.9f);
                break;
            case Nature.Sassy:
                totalStats.SpDefense = Mathf.FloorToInt(totalStats.SpDefense * 1.1f);
                totalStats.Speed = Mathf.FloorToInt(totalStats.Speed * 0.9f);
                break;
            case Nature.Timid:
                totalStats.Speed = Mathf.FloorToInt(totalStats.Speed * 1.1f);
                totalStats.Attack = Mathf.FloorToInt(totalStats.Attack * 0.9f);
                break;
            default:
                throw new System.Exception("Invalid nature");
        }
        
        
        TotalStats = totalStats;
    }

    private void OnValidate()
    {
        SetTotalStats();
    }
}