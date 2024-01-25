using System;
using System.Collections.Generic;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

public class CombatSystem : MonoBehaviour
{
    private PokemonSO pokemonSo1;
    private PokemonSO pokemonSo2;

    private PokemonSO[] playerPokemons;
    private bool[] tookPart;
    private PokemonSO[] opponentPokemons;
    private int opponentPokemonIndex = 0;
    
    private ItemSO lastUsedItem;
    
    private MoveSO p1Move;
    private MoveSO p2Move;
    
    private Stats p1CurrentStats;
    private Stats p2CurrentStats;

    private PlayerMove nextPlayerMove = PlayerMove.None;
    
    private int fleeAttempts = 0;

    private bool multipleOpponents;
    
    private bool playerPlaysFirst;
    private bool skipPlayerTurn;
    private bool nextStep;
    private bool fightOngoing;
    private bool playerHitLastAction;
    private bool opponentHitLastAction;
    private bool fleeResult;
    private bool opponentCaught;
    private bool opponentSwitched;
    
    private void Awake()
    {
        p1CurrentStats = new();
        p2CurrentStats = new();
        opponentPokemonIndex = 0;
        
        tookPart = new bool[playerPokemons.Length];
        tookPart[0] = true;
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

    //Only for testing
    private void StartFight()
    {
        //Get copies of pokemon stats
        p1CurrentStats = pokemonSo1.TotalStats;
        p2CurrentStats = pokemonSo2.TotalStats;
        fightOngoing = true;
    }
    
    //todo : ajouter un start fight avec PokemonEncounter
    
    public void StartFight(PokemonSO opponentPokemon)
    {
        playerPokemons = GameManager.GetPlayerPokemons().ToArray();
        pokemonSo1 = playerPokemons[0];
        pokemonSo2 = opponentPokemon;

        //Initialize "default moves"
        p1Move = pokemonSo1.Moves[0];
        p2Move = pokemonSo2.Moves[0];
        
        p1CurrentStats = pokemonSo1.TotalStats;
        p2CurrentStats = pokemonSo2.TotalStats;
        
        multipleOpponents = false;
        fightOngoing = true;
    }

    public void StartFightEncounter(PokemonEcounter encounter)
    {
        playerPokemons = GameManager.GetPlayerPokemons().ToArray();
        pokemonSo1 = playerPokemons[0];
        pokemonSo2 = encounter.pokemon;
        pokemonSo2.Level = Random.Range(encounter.minLevel, encounter.maxLevel);
        
        //Initialize "default moves"
        p1Move = pokemonSo1.Moves[0];
        p2Move = pokemonSo2.Moves[0];
        
        p1CurrentStats = pokemonSo1.TotalStats;
        p2CurrentStats = pokemonSo2.TotalStats;
        
        multipleOpponents = false;
        fightOngoing = true;
    }
    
    public void StartFightTrainer(List<PokemonSO> opponentPokemonList)
    {
        playerPokemons = GameManager.GetPlayerPokemons().ToArray();
        opponentPokemons = opponentPokemonList.ToArray();
        opponentPokemonIndex = 0;
        
        pokemonSo1 = playerPokemons[0];
        pokemonSo2 = opponentPokemons[opponentPokemonIndex];

        //Initialize "default moves"
        p1Move = pokemonSo1.Moves[0];
        p2Move = pokemonSo2.Moves[0];
        
        p1CurrentStats = pokemonSo1.TotalStats;
        p2CurrentStats = pokemonSo2.TotalStats;
        
        multipleOpponents = true;
        fightOngoing = true;
    }
    
    public void EndFight()
    {
        //Save stats after fight
        pokemonSo1.TotalStats = p1CurrentStats;
        pokemonSo2.TotalStats = p2CurrentStats;
        
        if (p1CurrentStats.HP > 0)
            pokemonSo1.Exp += CalculateExp();
        
        fightOngoing = false;
    }
    
    private void PlayTurn(PlayerMove playerMove)
    {
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
                    EndFight();
                else
                    skipPlayerTurn = true;
                break;
        }
        
        CalculatePriority();
        
        if (playerPlaysFirst)
        {
            if(!skipPlayerTurn)
                UseMove(p1Move, pokemonSo1, pokemonSo2, ref p2CurrentStats, ref p2CurrentStats, ref playerHitLastAction);
            UseMove(p2Move, pokemonSo2, pokemonSo1, ref p1CurrentStats, ref p1CurrentStats, ref opponentHitLastAction);
        }
        else
        {
            UseMove(p2Move, pokemonSo2, pokemonSo1, ref p1CurrentStats, ref p1CurrentStats, ref opponentHitLastAction);
            if(!skipPlayerTurn)
                UseMove(p1Move, pokemonSo1, pokemonSo2, ref p2CurrentStats, ref p2CurrentStats, ref playerHitLastAction);
        }

        if(p2CurrentStats.HP <= 0)
           OpponentSwap();
        
        skipPlayerTurn = false;
        nextStep = false;
        opponentSwitched = false;
    }

    private void Attack(MoveSO attack, PokemonSO attacker, PokemonSO defender, ref Stats defenderStats, ref bool actorMissed)
    {
        actorMissed = !AttackHits(attack);
        if (!actorMissed)
        {
            var attackerDamage = CalculateDamage(attack, attacker, defender);
            defenderStats.HP -= attackerDamage;
            defenderStats.HP = (defenderStats.HP < 0 ? 0 : defenderStats.HP);

            if(EndCondition())
                EndFight();
        }
    }
    
    private void UseMove(MoveSO move, PokemonSO attacker, PokemonSO defender, ref Stats attackerStats, ref Stats defenderStats,ref bool actorMissed)
    {
        if (move.PP == 0) return;
        move.PP--;

        switch (move.MoveType)
        {
            case MoveType.Status:
                if (move.Target == Target.Self)
                    ApplySelfBonus(move, ref attackerStats);
                else
                    ApplyEffect(move, ref defenderStats);
                break;
            
            default:
                Attack(move, attacker, defender, ref defenderStats, ref actorMissed);
                break;
        }
    }

    private void ApplySelfBonus(MoveSO move, ref Stats stats)
    {
        switch (move.Buff)
        {
            //Buff Section
            case Buff.AttackUp:
                stats.Attack += move.Power;
                break;
            case Buff.DefenseUp:
                stats.Defense += move.Power;
                break;
            case Buff.SpeedUp:
                stats.Speed += move.Power;
                break;
            case Buff.SpAtkUp:
                stats.SpAttack += move.Power;
                break;
            case Buff.SpDefUp:
                stats.SpDefense += move.Power;
                break;
        }
    }

    private void ApplyEffect(MoveSO move, ref Stats stats)
    {
        //Debuff Section
        switch (move.Buff)
        {
            case Buff.AttackDown:
                stats.Attack -= move.Power;
                break;
            case Buff.DefenseDown:
                stats.Defense -= move.Power;
                break;
            case Buff.SpeedDown:
                stats.Speed -= move.Power;
                break;
            case Buff.SpAtkDown:
                stats.SpAttack -= move.Power;
                break;
            case Buff.SpDefDown:
                stats.SpDefense -= move.Power;
                break;
        }

    }

    public void UseItem(int itemIndex)
    {
        ItemSO item = GameManager.GetPlayerItems()[itemIndex];

        lastUsedItem = item;
        
        switch (item.itemType)
        {
            case ItemType.Pokeball:
                int catchValue = Random.Range(0, 256);
            
                int f = Mathf.FloorToInt((p2CurrentStats.HP * 255 * 4) / (pokemonSo2.TotalStats.HP * 12));

                if (f >= catchValue)
                {
                    opponentCaught = true;
                    Debug.Log("Caught !");
                }
                else
                {
                    Debug.Log("Broke free.");
                }
                break;
            
            case ItemType.StatBuff:
                switch (item.targetStat)
                {
                    case TargetStat.Attack:
                        p1CurrentStats.Attack += item.effectValue;
                        break;
                    case TargetStat.Defense:
                        p1CurrentStats.Defense += item.effectValue;
                        break;
                    case TargetStat.SpAttack:
                        p1CurrentStats.SpAttack += item.effectValue;
                        break;
                    case TargetStat.SpDefense:
                        p1CurrentStats.SpDefense += item.effectValue;
                        break;
                    case TargetStat.Speed:
                        p1CurrentStats.Speed += item.effectValue;
                        break;
                    case TargetStat.HP:
                        p1CurrentStats.HP += item.effectValue;
                        p1CurrentStats.HP = p1CurrentStats.HP > pokemonSo1.TotalStats.HP
                            ? pokemonSo1.TotalStats.HP
                            : p1CurrentStats.HP;
                        break;
                }

                break;
        }
    }
    
    private bool AttackHits(MoveSO attack)
    {
        if (attack.Accuracy > Random.Range(0, 100))
            return true;
        return false;
    }
    
    private void GetOpponentNextMove(PokemonSO pokemon)
    {
        int numOfMoves = pokemon.Moves.Count;
        p2Move = pokemon.Moves[Random.Range(0, numOfMoves)];
    }
    
    private void CalculatePriority()
    {
        //If player's turn is skipped, order doesnt matter
        if (skipPlayerTurn)
        {
            playerPlaysFirst = true;
            return;
        }
        
        if (p1Move.MoveType == MoveType.Status)
        {
            playerPlaysFirst = true;
            return;
        }
        if (p2Move.MoveType == MoveType.Status)
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
        float attackValue;
        float defenseValue;
        if (attack.MoveType == MoveType.Physical)
        {
            attackValue = (2.0f * attacker.Level / 5.0f + 2.0f) * attack.Power * attacker.TotalStats.Attack;
            defenseValue = defender.TotalStats.SpDefense;
        }
        else
        {
            attackValue = (2.0f * attacker.Level / 5.0f + 2.0f) * attack.Power * attacker.TotalStats.SpAttack;
            defenseValue = defender.TotalStats.Defense;
        }
        
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

    private int CalculateExp()
    {
        int expGained;

        int pokemonIsWild = 1; //1 if wild, 1.5 if not
        int pokemonBaseXP = pokemonSo2.BaseXP;
        int pokemonLevel = pokemonSo2.Level;
        
        int numberOfReceivers = 0; //number of pokemons that took part and didn't faint
        
        for (int i = 0; i < tookPart.Length; i++)
            numberOfReceivers += tookPart[i] ? 1 : 0;
        
        expGained = ((pokemonBaseXP * pokemonLevel) / 7) * (1 / numberOfReceivers) * pokemonIsWild;
        
        return expGained;
    }
    
    public bool EndCondition()
    {
        if(!multipleOpponents) return p1CurrentStats.HP <= 0 || p2CurrentStats.HP <= 0;
            
        for(int i = 0; i < playerPokemons.Length; i++)
            if (playerPokemons[i].TotalStats.HP > 0)
                return false;
        
        for(int i = 0; i < opponentPokemons.Length; i++)
            if (opponentPokemons[i].TotalStats.HP > 0)
                return false;

        return true;
    }
    
    public void ChoseNextMove(int moveIndex)
    {
        nextStep = true;
        p1Move = pokemonSo1.Moves[moveIndex];
    }

    public void ChoseNewPokemon(int pokemonIndex)
    {
        //if (pokemonSo1 == playerPokemons[pokemonIndex]) return;
        
        //Register new stats for leaving pokemon
        //pokemonSo1.TotalStats = p1CurrentStats;
        
        pokemonSo1 = playerPokemons[pokemonIndex];
        p1CurrentStats = pokemonSo1.TotalStats;

        tookPart[pokemonIndex] = true;
    }

    public void OpponentSwap()
    {
        if (!multipleOpponents)
        {
            EndFight();
            return;
        }
        
        ++opponentPokemonIndex;
        
        if (opponentPokemonIndex >= opponentPokemons.Length)
        {
            Debug.Log("Fight over");
            EndFight();
            return;
        }

        pokemonSo2 = opponentPokemons[opponentPokemonIndex];
        opponentSwitched = true;
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

    #region Getters & Booleans
    
    public MoveSO[] GetPlayerMoveArray()
    {
        return pokemonSo1.Moves.ToArray();
    }

    public PokemonSO[] GetPlayerPokemons()
    {
        return playerPokemons;
    }

    public PokemonSO GetPlayerCurrentPokemon()
    {
        return pokemonSo1;
    }
    
    public PokemonSO GetOpponentPokemon()
    {
        return pokemonSo2;
    }
    
    public String GetPlayerPokemonName()
    {
        return pokemonSo1.Name;
    }
    public String GetOpponentPokemonName()
    {
        return pokemonSo2.Name;
    }

    public Sprite GetPlayerPokemonSprite()
    {
        return pokemonSo1.BackSprite;
    }

    public Sprite GetOpponentPokemonSprite()
    {
        return pokemonSo2.FrontSprite;
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

    public int GetPlayerPokemonMaxHp()
    {
        return pokemonSo1.TotalStats.MaxHP;
    }
    
    public int GetOpponentPokemonHp()
    {
        return p2CurrentStats.HP;
    }
    
    public int GetOpponentPokemonMaxHp()
    {
        return pokemonSo2.TotalStats.MaxHP;
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
        return playerHitLastAction;
    }

    public bool OpponentMissed()
    {
        return opponentHitLastAction;
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

    public bool PlayerUsedItem()
    {
        return nextPlayerMove == PlayerMove.Item;
    }

    public ItemSO GetLastUsedItem()
    {
        return lastUsedItem;
    }
    
    public bool PlayerFled()
    {
        return fleeResult;
    }

    public bool PlayerPlaysFirst()
    {
        return playerPlaysFirst;
    }

    public bool OpponentIsCaught()
    {
        return opponentCaught;
    }

    public bool OpponentCanSwap()
    {
        if (!multipleOpponents) return false;
        
        return opponentPokemonIndex < opponentPokemons.Length;
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
