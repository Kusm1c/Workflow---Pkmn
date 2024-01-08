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

    public static void OnFightStart()
    {
        instance.combatSystem.StartFight(instance.playerPokemonList, instance.selectablePokemons[0]);
        var ui = Instantiate(instance.combatUIPrefab);
        instance.combatUI = ui.GetComponent<CombatUI>();
    }

    public static void OnFightEnd()
    {
        instance.combatSystem.EndFight();
        Destroy(instance.combatUI.gameObject);
    }
}
