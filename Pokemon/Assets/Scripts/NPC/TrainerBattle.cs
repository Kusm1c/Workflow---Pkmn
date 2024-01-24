using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainerBattle : MonoBehaviour
{
    public List<PokemonSO> playerPokemonList;
    public int numberOfPokemonAlive;
    public bool hasLost;
    public bool isGymLeader;
    public string Name;
    public string BeforeBattleText;
    public string AfterBattleText;
    public bool routeLoaded;
    public List<Vector2> tilesToCheck;

    private PlayerMvmnt _player;

    public int deafeatedMoney;
    
    public TypeOfTrainer typeOfTrainer;
#if typeOfTrainer == GymLeader
    public Badge badge;
#endif

    private void Start()
    {
        _player = PlayerMvmnt.Instance;
    }

    void Update()
    {
        if (routeLoaded)
        {
            foreach (var VARIABLE in tilesToCheck)
            {
                if (VARIABLE == _player.currentPos)
                {
                    _player.isFighting = true;
                    MoveToPlayer();
                    SayBeforeBattleText();
                    GameManager.instance.OnFightStart(playerPokemonList[0]); // TODO : Change this to a list of pokemons
                    CombatUI.Instance.SetTrainerName(Name);
                    CombatUI.Instance.StartCombat();
                }
            }
        }
    }

    private void SayBeforeBattleText()
    {
        // TODO : Say Before Battle Text
    }

    private void MoveToPlayer()
    {
        // TODO : Move to player
    }
}

public enum TypeOfTrainer
{
    GymLeader,
    Trainer
}

public enum Badge
{
    Boulder,
    Cascade,
    Thunder,
    Rainbow,
    Soul,
    Marsh,
    Volcano,
    Earth
}