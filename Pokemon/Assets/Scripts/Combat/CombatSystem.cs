using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

public class CombatSystem : MonoBehaviour
{
    [SerializeField] private PokemonSO pokemonSo1;
    [SerializeField] private PokemonSO pokemonSo2;

    [SerializeField] private PokemonSO[] playerPokemons;
    
    [SerializeField] private MoveSO p1Move;
    [SerializeField] private MoveSO p2Move;
    
    private Stats p1CurrentStats;
    private Stats p2CurrentStats;

    private PlayerMove nextPlayerMove = PlayerMove.None;
    
    private int fleeAttempts = 0;
    
    private bool playerPlaysFirst;
    private bool skipPlayerTurn;
    private bool nextStep;
    private bool fightOngoing;
    private bool playerHitLastAction;
    private bool opponentHitLastAction;
    private bool fleeResult;
    
    private void Awake()
    {
        
    }

    private void OnEnable()
    {
        StartFight();
    }

    private void Update()
    {
        Turn();
    }

    private void Turn()
    {
        if (!nextStep) return;
        
        PlayTurn(nextPlayerMove);
    }

    private void StartFight()
    {
        Debug.LogWarning("Start Fight !");
        //Get copies of pokemon stats
        p1CurrentStats = pokemonSo1.TotalStats;
        p2CurrentStats = pokemonSo2.TotalStats;
        fightOngoing = true;
    }

    private void EndFight()
    {
        Debug.LogWarning("Fight Over !");
        //pokemonSo1.TotalStats = p1CurrentStats;
        //pokemonSo2.TotalStats = p2CurrentStats;
        fightOngoing = false;
    }
    
    private void PlayTurn(PlayerMove playerMove)
    {
        Debug.Log("Playing turn");
        GetOpponentNextMove(pokemonSo2);
        var p2Damage = 0;
        var p1Damage = 0;
        
        switch (nextPlayerMove)
        {
            case PlayerMove.Attack:
                //Attack selection implemented in button
                break;
            case PlayerMove.Item:
                //Implement Later
                break;
            case PlayerMove.SwitchPokemon:
                //Pokemon switch implemented in button
                skipPlayerTurn = true;
                break;
            case PlayerMove.Flee:
                if (Flee(p1CurrentStats, p2CurrentStats, ref fleeResult))
                {
                    Debug.LogWarning("Fled Successfully");
                    EndFight();
                }
                else
                {
                    Debug.LogWarning("Failed to flee...");
                    skipPlayerTurn = true;
                }
                break;
        }
        
        CalculatePriority();
        
        if (playerPlaysFirst)
        {
            if(!skipPlayerTurn)
                Attack(p1Move, pokemonSo1, pokemonSo2, ref p2CurrentStats, ref playerHitLastAction);
            Attack(p2Move, pokemonSo2, pokemonSo1, ref p1CurrentStats, ref opponentHitLastAction);
        }
        else
        {
            Attack(p2Move, pokemonSo2, pokemonSo1, ref p1CurrentStats, ref opponentHitLastAction);
            if(!skipPlayerTurn)
                Attack(p1Move, pokemonSo1, pokemonSo2, ref p2CurrentStats, ref playerHitLastAction);
        }
        nextStep = false;
    }

    private void Attack(MoveSO attack, PokemonSO attacker, PokemonSO defender, ref Stats defenderStats, ref bool actorMissed)
    {
        var attackerDamage = UseMove(attack, pokemonSo1, pokemonSo2, ref actorMissed);
        defenderStats.HP -= attackerDamage;
        Debug.Log($"{attacker.Name} uses {attack.Name}");
        Debug.Log($"{defender.Name} takes {attackerDamage}");
        if(EndCondition())
            EndFight();
    }
    
    private int UseMove(MoveSO move, PokemonSO attacker, PokemonSO defender, ref bool actorMissed)
    {
        if (move.PP == 0) return 0;
        move.PP--;
        actorMissed = AttackHits(move);
        return actorMissed ? CalculateDamage(move, attacker, defender) : 0;
    }
    
    private bool AttackHits(MoveSO attack)
    {
        if (attack.Accuracy > Random.Range(0, 100))
        {
            return true;
        }
        Debug.LogWarning("attack missed");
        return false;
    }
    
    private void GetOpponentNextMove(PokemonSO pokemon)
    {
        p2Move = pokemon.Moves[Random.Range(0, 3)];
    }
    
    private void CalculatePriority()
    {
        if (p1Move.MoveType == MoveType.Status)
        {
            playerPlaysFirst = true;
            return;
        }
        else if (p2Move.MoveType == MoveType.Status)
        {
            playerPlaysFirst = false;
            return;
        }
        
        if (p1Move.Priority == p2Move.Priority)
            playerPlaysFirst = pokemonSo1.TotalStats.Speed > pokemonSo2.TotalStats.Speed;
        else
            playerPlaysFirst = p1Move.Priority > p2Move.Priority;
    }

    private int CalculateDamage(MoveSO attack, PokemonSO attacker, PokemonSO defender)
    {
        float attackValue = (2.0f * attacker.Level / 5.0f + 2.0f) * attack.Power * attacker.TotalStats.Attack;
        float defenseValue = defender.TotalStats.Defense;
        float typeDamageMultiplier = TypeTable.GetTypeDamageMultiplier(attack.Type, defender.Type);
        float attackEfficiency = Random.Range(85, 100) / 100.0f;
        
        return Mathf.FloorToInt((((attackValue / defenseValue) / 50) + 2) * typeDamageMultiplier * attackEfficiency);
    }

    private bool Flee(Stats playerStats, Stats wildStats, ref bool fled)
    {
        var playerVal = playerStats.Speed * 32;
        var wildVal = Mathf.FloorToInt(wildStats.Speed / 4.0f) % 256;

        var odds = playerVal / wildVal + 30 * fleeAttempts;

        if (odds > 255) fled = true;
        else
        {
            if (odds < Random.Range(0, 255)) fled = true;
            else
                fled = false;
        }

        return fled;
    }

    private bool EndCondition()
    {
        return p1CurrentStats.HP <= 0 || p2CurrentStats.HP <= 0;
    }

    public void ChooseNextMove(int moveIndex)
    {
        nextStep = true;
        p1Move = pokemonSo1.Moves[moveIndex];
    }

    public void ChoseNewPokemon(int pokemonIndex)
    {
        //Register new stats for leaving pokemon
        //pokemonSo1.TotalStats = p1CurrentStats; TotalStats is currently readonly

        pokemonSo1 = playerPokemons[pokemonIndex];
        p1CurrentStats = pokemonSo1.TotalStats;
    }
    
    public void ChoseNextPlayerAction(int actionIndex)
    {
        switch (actionIndex)
        {
            case 0:
                nextPlayerMove = PlayerMove.Attack;
                break;
            case 1:
                nextPlayerMove = PlayerMove.Item;
                break;
            case 2:
                nextPlayerMove = PlayerMove.SwitchPokemon;
                break;
            case 3:
                nextPlayerMove = PlayerMove.Flee;
                break;
        }
    }

    public void NextStep()
    {
        nextStep = true;
    }

    #region Getters
    
    public MoveSO[] GetPlayerMoveArray()
    {
        return pokemonSo1.Moves.ToArray();
    }

    public PokemonSO[] GetPlayerPokemons()
    {
        return playerPokemons;
    }

    public String GetPlayerPokemonName()
    {
        return pokemonSo1.Name;
    }
    public String GetOpponentPokemonName()
    {
        return pokemonSo2.Name;
    }

    public int GetPlayerPokemonLevel()
    {
        return pokemonSo1.Level;
    }
    public int GetOpponentPokemonLevel()
    {
        return pokemonSo2.Level;
    }

    public int GetPlayerPokemonHp()
    {
        return p1CurrentStats.HP;
    }
    public int GetOpponentPokemonHp()
    {
        return p2CurrentStats.HP;
    }
    
    public MoveSO GetPlayerMove()
    {
        return p1Move;
    }

    public MoveSO GetOpponentMove()
    {
        return p2Move;
    }

    public PokemonType GetPlayerPokemonType()
    {
        return pokemonSo1.Type;
    }
    
    public PokemonType GetOpponentType()
    {
        return pokemonSo2.Type;
    }

    public bool PlayerMissed()
    {
        return !playerHitLastAction;
    }

    public bool OpponentMissed()
    {
        return !opponentHitLastAction;
    }
    
    public bool PlayerFainted()
    {
        return p1CurrentStats.HP <= 0;
    }
    
    public bool OpponentFainted()
    {
        return p2CurrentStats.HP <= 0;
    }

    public bool PlayerSwitched()
    {
        return nextPlayerMove == PlayerMove.SwitchPokemon;
    }
    
    public bool PlayerTriedFlee()
    {
        return nextPlayerMove == PlayerMove.Flee;
    }

    public bool PlayerFled()
    {
        return fleeResult;
    }
    
    #endregion
    private enum PlayerMove
    {
        None,
        Attack,
        Item,
        SwitchPokemon,
        Flee
    }
}
