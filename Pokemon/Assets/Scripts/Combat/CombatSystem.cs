using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class CombatSystem : MonoBehaviour
{
    [SerializeField] private PokemonSO pokemonSo1;
    [SerializeField] private PokemonSO pokemonSo2;

    private Stats p1CurrentStats;
    private Stats p2CurrentStats;
    
    private bool onePlaysFirst = true;
    private bool nextStep = false;
    
    // private PlayerControls input;
    
    private void Awake()
    {
        // input = new();
    }

    private void OnEnable()
    {
        // input.Menu.Enable();
    }

    private void TestFunction()
    {
        if (p1CurrentStats.HP <= 0.0f)
        {
            Debug.Log("P2 win");
            return;
        }

        if (p2CurrentStats.HP <= 0.0f)
        {
            Debug.Log("P1 win");
            return;
        }
        
        //Get copies of pokemon stats
        p1CurrentStats = pokemonSo1.TotalStats;
        p2CurrentStats = pokemonSo2.TotalStats;
        
        CalculatePriority();
        
        //Get PokemonMoves
        MoveSO firstMove = new();
        MoveSO secondMove;

        if (onePlaysFirst)
        {
            p2CurrentStats.HP -= Attack(firstMove, pokemonSo1, pokemonSo2);
            p1CurrentStats.HP -= Attack(firstMove, pokemonSo2, pokemonSo1);
        }
        
    }

    private int Attack(MoveSO attack, PokemonSO attacker, PokemonSO defender)
    {
        if (!AttackHits(attack)) return 0;

        return CalculateDamage(attack, attacker, defender);
    }
    
    private bool AttackHits(MoveSO attack)
    {
        if (attack.Accuracy > Random.Range(0, 100))
        {
            return true;
        }

        return false;
    }
    
    private void CalculatePriority()
    {
        onePlaysFirst = pokemonSo1.TotalStats.Speed > pokemonSo2.TotalStats.Speed;
    }

    private int CalculateDamage(MoveSO attack, PokemonSO attacker, PokemonSO defender)
    {
        return Mathf.FloorToInt(((((2 * attacker.Level / 5 + 2) * attack.Power * attacker.BaseStats.Attack /
                             defender.BaseStats.Defense) /
                            50) + 2) * TypeTable.GetTypeDamageMultiplier(attack.Type, defender.Type) * Random.Range(0, 100) / 100);
    }
    
    private float GetModifierFromNature(Nature pokemonSoNature)
    {
        switch (pokemonSoNature)
        {
            default:
                return 1.0f;
        }
    }

    private void WaitForNextStep()
    {
        while(!nextStep)
        {
            // if (input.Menu.Validate.triggered) nextStep = true;
        }

        nextStep = false;
    }
}
