using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CombatSystem : MonoBehaviour
{
    private PokemonSO pokemonSo1;
    private PokemonSO pokemonSo2;
    
    private bool AttackHits(MoveSO attack)
    {
        if (attack.Accuracy > Random.Range(0, 100))
        {
            return true;
        }

        return false;
    }
    
    private PokemonSO CalculatePriority()
    {
        return (pokemonSo1.TotalStats.Speed > pokemonSo2.TotalStats.Speed ? pokemonSo1 : pokemonSo2);
    }

    private float CalculateDamage(MoveSO attack, PokemonSO attacker, PokemonSO defendant)
    {
        var damage =
            Mathf.Floor((((2 * attacker.Level / 5 + 2) * attack.Power * attacker.BaseStats.Attack / defendant.BaseStats.Defense) /
              50) + 2) * Random.Range(0, 100) / 100);
    }
    
    private float GetModifierFromNature(Nature pokemonSoNature)
    {
        switch (pokemonSoNature)
        {
            default:
                return 1.0f;
        }
    }
}
