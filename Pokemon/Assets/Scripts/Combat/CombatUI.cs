using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Mail;
using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class CombatUI : MonoBehaviour
{
    
    [SerializeField] private GameObject CombatUICanvas;
    [SerializeField] private GameObject CardsPannel;
    [SerializeField] private GameObject PlayerCard;
    [SerializeField] private GameObject OpponentCard;
    [SerializeField] private GameObject ActionPannel;
    [SerializeField] private GameObject MovePannel;
    [SerializeField] private GameObject PokemonPannel;
    
    [SerializeField] private GameObject[] pokemonSelectionButtons;
    
    [SerializeField] private TMP_Text TextBox;
    [SerializeField] private TMP_Text PlayerInfo;
    [SerializeField] private TMP_Text OpponentInfo;
    
    
    [SerializeField] private TMP_Text[] MoveNames;
    
    [SerializeField] private CombatSystem combatSystem;

    private bool nextStep;
    private MenuState menuState;
    
    private GameObject currentMenu;
    
    private MoveSO[] playerMoveSet;
    
    private PlayerControls input;
    private void OnEnable()
    {
        input = new();
        input.Menu.Enable();
        currentMenu = ActionPannel;
        StartCombat();
    }

    private void OnDisable()
    {
        input.Menu.Disable();
    }

    private void Update()
    {
        GetValidateInput();
    }

    public void StartCombat()
    {
        CombatUICanvas.SetActive(true);
        CardsPannel.SetActive(true);
        ActionPannel.SetActive(true);
        MovePannel.SetActive(false);

        playerMoveSet = combatSystem.GetPlayerMoveArray();
        UpdateInfoText();
        
        for (int i = 0; i < pokemonSelectionButtons.Length; i++)
        {
            if ((i >= combatSystem.GetPlayerPokemons().Length))
            {
                pokemonSelectionButtons[i].SetActive(false);
                continue;
            }
     
            pokemonSelectionButtons[i].SetActive(true);
            pokemonSelectionButtons[i].GetComponentInChildren<TMP_Text>().text =
                combatSystem.GetPlayerPokemons()[i].Name;
        }
        
        for (int i = 0; i < playerMoveSet.Length; i++)
            MoveNames[i].text = playerMoveSet[i].Name;
        
        currentMenu = ActionPannel;
    }
    
    public void OpenActionPanel()
    {
        SwitchMenu(ActionPannel);
        TextBox.text = $"What will {combatSystem.GetOpponentPokemonName()} do ?";
    }
    
    public void OpenMovePanel()
    {
        SwitchMenu(MovePannel);
        TextBox.text = "Select a move.";
    }

    public void OpenPokemonPanel()
    {
        SwitchMenu(PokemonPannel);
        PlayerCard.SetActive(false);
        OpponentCard.SetActive(false);
        TextBox.text = "Chose a POKéMON.";
    }

    public void EnterHoverMove(int moveIndex)
    {
        TextBox.text = $"{playerMoveSet[moveIndex].Name} \nPP : {playerMoveSet[moveIndex].PP} \nTYPE : {playerMoveSet[moveIndex].Type}";
    }

    public void ExitHoverMove()
    {
        TextBox.text = "Select a move.";
    }
    
    public void TurnDone()
    {
        GoToNextStep();
    }
    private void SwitchMenu(GameObject nextMenu)
    {
        currentMenu.SetActive(false);
        currentMenu = nextMenu;
        currentMenu.SetActive(true);
        PlayerCard.SetActive(true);
        OpponentCard.SetActive(true);
    }

    private void DisplayAction()
    {
        switch (menuState)
        {
            case MenuState.Default:
                TextBox.text = $"What will {combatSystem.GetPlayerPokemonName()} do ?";
                break;
            
            case MenuState.PlayerTriedFlee:
                TextBox.text = $"{combatSystem.GetPlayerPokemonName()} tries to flee.";
                break;
            
            case MenuState.PlayerFled:
                TextBox.text = $"{combatSystem.GetPlayerPokemonName()} fled !";
                break;
            
            case MenuState.PlayerFledFailed:
                TextBox.text = $"{combatSystem.GetPlayerPokemonName()} failed !";
                break;
            
            case MenuState.PlayerSwitchedOld:
                TextBox.text = $"Come back, {combatSystem.GetPlayerPokemonName()} !";
                break;
            
            case MenuState.PlayerSwitchedNew:
                TextBox.text = $"Go, {combatSystem.GetPlayerPokemonName()} !";
                break;
            
            case MenuState.PlayerMove:
                TextBox.text = $"{combatSystem.GetPlayerPokemonName()} used {combatSystem.GetPlayerMove().Name}";
                break;
            
            case MenuState.PlayerEfficiency:
                var playerEfficiency = TypeTable.GetTypeDamageMultiplier(combatSystem.GetPlayerMove().Type,
                    combatSystem.GetOpponentType());
                if(playerEfficiency < .75f)
                    TextBox.text = "It's not very effective...";
                else if(playerEfficiency > 1.5f)
                    TextBox.text = "It's very effective !";
                UpdateInfoText();
                break;
            
            case MenuState.PlayerMissed:
                TextBox.text = $"{combatSystem.GetPlayerPokemonName()} missed !";
                break;
            
            case MenuState.OppenentMove:
                TextBox.text = $"{combatSystem.GetOpponentPokemonName()} used {combatSystem.GetOpponentMove().Name}";
                break;
            
            case MenuState.OpponentEfficiency:
                var opponentEfficiency = TypeTable.GetTypeDamageMultiplier(combatSystem.GetOpponentMove().Type,
                    combatSystem.GetPlayerPokemonType());
                if(opponentEfficiency < .75f)
                    TextBox.text = "It's not very effective...";
                else if(opponentEfficiency > 1.5f)
                    TextBox.text = "It's very effective !";
                UpdateInfoText();
                break;
            
            case MenuState.OpponentMiss:
                TextBox.text = $"{combatSystem.GetOpponentPokemonName()} missed !";
                break;
            
            case MenuState.PlayerFainted:
                TextBox.text = $"{combatSystem.GetPlayerPokemonName()} fainted !";
                break;
            
            case MenuState.OpponentFainted:
                TextBox.text = $"{combatSystem.GetOpponentPokemonName()} fainted !";
                break;
            
            case MenuState.Win:
                TextBox.text = "You win !";
                break;
            
            case MenuState.Lose:
                TextBox.text = "You lost...";
                break;
        }
    }

    private void GoToNextStep()
    {
        switch (menuState)
        {
            case MenuState.Default:
                if (combatSystem.PlayerTriedFlee())
                    menuState = MenuState.PlayerTriedFlee;
                else if (combatSystem.PlayerSwitched())
                    menuState = MenuState.PlayerSwitchedOld;
                else
                    menuState = MenuState.PlayerMove;
                ActionPannel.SetActive(false);
                break;
            
            case MenuState.PlayerTriedFlee:
                if (combatSystem.PlayerFled())
                    menuState = MenuState.PlayerFled;
                else
                    menuState = MenuState.PlayerFledFailed;
                break;
            
            case MenuState.PlayerFled:
                break;
            
            case MenuState.PlayerFledFailed:
                menuState = MenuState.OppenentMove;
                break;
            
            case MenuState.PlayerSwitchedOld:
                menuState = MenuState.PlayerSwitchedNew;
                break;
            
            case MenuState.PlayerSwitchedNew:
                menuState = MenuState.OppenentMove;
                break;
            
            case MenuState.PlayerMove:
                var playerEfficiency = TypeTable.GetTypeDamageMultiplier(combatSystem.GetPlayerMove().Type,
                    combatSystem.GetOpponentType());
                if (combatSystem.PlayerMissed())
                    menuState = MenuState.PlayerMissed;
                else if(playerEfficiency < .75f || playerEfficiency > 1.5f)
                    menuState = MenuState.PlayerEfficiency;
                else if (combatSystem.OpponentFainted())
                    menuState = MenuState.OpponentFainted;
                else
                    menuState = MenuState.OppenentMove;
                break;
            
            case MenuState.PlayerMissed:
                menuState = MenuState.OppenentMove;
                break;
            
            case MenuState.PlayerEfficiency:
                if (combatSystem.OpponentFainted())
                    menuState = MenuState.OpponentFainted;
                else
                    menuState = MenuState.OppenentMove;
                break;
            
            case MenuState.OppenentMove:
                var opponentEfficiency = TypeTable.GetTypeDamageMultiplier(combatSystem.GetOpponentMove().Type,
                    combatSystem.GetPlayerPokemonType());
                if (combatSystem.OpponentMissed())
                    menuState = MenuState.OpponentMiss;
                else if(opponentEfficiency < .75f || opponentEfficiency > 1.5f)
                    menuState = MenuState.OpponentEfficiency;
                else
                {
                    if (combatSystem.PlayerFainted())
                        menuState = MenuState.PlayerFainted;
                    else
                    {
                        menuState = MenuState.Default;
                        OpenActionPanel();
                    }
                }
                break;
            
            case MenuState.OpponentMiss:
                menuState = MenuState.Default;
                OpenActionPanel();
                break;
            
            case MenuState.OpponentEfficiency:
                if (combatSystem.PlayerFainted())
                    menuState = MenuState.PlayerFainted;
                else
                {
                    menuState = MenuState.Default;
                    OpenActionPanel();
                }
                break;
            
            case MenuState.PlayerFainted:
                menuState = MenuState.Lose;
                break;
            
            case MenuState.OpponentFainted:
                menuState = MenuState.Win;
                break;
            
            case MenuState.Win:
                break;
            
            case MenuState.Lose:
                break;
        }
        
        DisplayAction();
    }

    private void UpdateInfoText()
    {
        PlayerInfo.text = $"{combatSystem.GetPlayerPokemonName()} || Lv{combatSystem.GetPlayerPokemonLevel()}\nHP : {combatSystem.GetPlayerPokemonHp()}";
        OpponentInfo.text = $"{combatSystem.GetOpponentPokemonName()} || Lv{combatSystem.GetOpponentPokemonLevel()}\nHP : {combatSystem.GetOpponentPokemonHp()}";
    }
    
    private void GetValidateInput()
    {
        if (input.Menu.Validate.triggered && menuState != MenuState.Default)
            GoToNextStep();
    }
    
    private enum MenuState
    {
        Default,
        PlayerMove,
        PlayerTriedFlee,
        PlayerFled,
        PlayerFledFailed,
        PlayerSwitchedOld,
        PlayerSwitchedNew,
        PlayerMissed,
        PlayerEfficiency,
        PlayerFainted,
        OppenentMove,
        OpponentMiss,
        OpponentEfficiency,
        OpponentFainted,
        Win,
        Lose,
    }
}
