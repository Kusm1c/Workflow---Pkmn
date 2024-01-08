using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public CombatSystem combatSystem;
    [SerializeField] private GameObject combatUIPrefab;

    private CombatUI combatUI;

    [SerializeField] private List<PokemonSO> playerPokemonList;
    [SerializeField] private List<PokemonSO> selectablePokemons;
    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        if (!instance) instance = this;
        combatSystem = GetComponent<CombatSystem>();
        OnFightStart();
    }

    public void OnFightStart()
    {
        instance.combatSystem.StartFight(instance.playerPokemonList, instance.selectablePokemons[0]);
        var ui = Instantiate(instance.combatUIPrefab);
        instance.combatUI = ui.GetComponent<CombatUI>();
    }
    
    public void OnFightStart(PokemonSO pokemonSo)
    {
        instance.combatSystem.StartFight(instance.playerPokemonList,pokemonSo);
        var ui = Instantiate(instance.combatUIPrefab);
        instance.combatUI = ui.GetComponent<CombatUI>();
    }

    public static void OnFightEnd()
    {
        instance.combatSystem.EndFight();
        PlayerMvmnt.Instance.isFighting = false;
        Destroy(instance.combatUI.gameObject);
    }

    public static void GivePlayerPokemon(PokemonSO pokemonSo)
    {
        if (instance.playerPokemonList.Count >= 6)
        {
            Debug.LogWarning("Max pokemons in team reached ! Sending new pokemon to brazil");
            return;
        }
            
        instance.playerPokemonList.Add(pokemonSo);
    }

    public static void GivePlayerItem(ItemSO itemSo)
    {
        
    }
}
