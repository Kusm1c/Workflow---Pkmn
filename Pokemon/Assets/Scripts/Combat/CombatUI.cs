using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CombatUI : MonoBehaviour
{
    [Header("Pannels")]
    [SerializeField] private GameObject CombatUICanvas;
    [SerializeField] private GameObject CardsPannel;
    [SerializeField] private GameObject PlayerCard;
    [SerializeField] private GameObject OpponentCard;
    [SerializeField] private GameObject ActionPannel;
    [SerializeField] private GameObject MovePannel;
    [SerializeField] private GameObject PokemonPannel;
    [SerializeField] private GameObject ItemPanel;
    
    [Header("Buttons")]
    [SerializeField] private GameObject[] pokemonSelectionButtons;
    [SerializeField] private GameObject[] itemButtons;
    
    [Header("Text")]
    [SerializeField] private TMP_Text TextBox;
    [SerializeField] private TMP_Text PlayerInfo;
    [SerializeField] private TMP_Text PlayerLevel;
    [SerializeField] private TMP_Text PlayerHealth;
    [SerializeField] private TMP_Text OpponentInfo;
    [SerializeField] private TMP_Text OpponentLevel;
    [SerializeField] private TMP_Text CurrentPP;
    [SerializeField] private TMP_Text MaxPP;
    [SerializeField] private TMP_Text MoveType;
    [SerializeField] private TMP_Text ItemDescription;
    
    [SerializeField] private TMP_Text[] MoveNames;
    [SerializeField] private TMP_Text[] PokemonSelectionLevels;
    [SerializeField] private TMP_Text[] ItemNames;
    [SerializeField] private TMP_Text PokemonSelectedName;
    [SerializeField] private TMP_Text PokemonSelectedLevel;
    
    [Header("Sprites")]
    [SerializeField] private GameObject playerDisplay;
    [SerializeField] private GameObject opponentDisplay;

    [Header("Stat Gauges")] 
    [SerializeField] private Image PlayerHealthBar;
    [SerializeField] private Image PlayerXPBar;
    [SerializeField] private Image OpponentHealthBar;

    [Header("Gauges Colors")] 
    [SerializeField] private Sprite Green;
    [SerializeField] private Sprite Yellow;
    [SerializeField] private Sprite Red;
    
    private string currentPokemonName;
    
    private bool nextStep;
    
    private MenuState menuState;
    
    private GameObject currentMenu;
    
    private MoveSO[] playerMoveSet;
    
    private CombatSystem combatSystem;
    private PlayerControls input;
    
    public static CombatUI Instance { get; private set; }
    
    private void OnEnable()
    {
        Instance = this;
        Debug.Log("CombatUI enabled");
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
        GetCancelInput();
        GetValidateInput();
    }

    public void StartCombat()
    {
        combatSystem = GameManager.instance.combatSystem;
        
        CombatUICanvas.SetActive(true);
        CardsPannel.SetActive(true);
        ActionPannel.SetActive(true);
        MovePannel.SetActive(false);
        
        TextBox.text = $"What will {combatSystem.GetPlayerPokemonName()} do ?";
        
        playerMoveSet = combatSystem.GetPlayerMoveArray();
        PlayerInfo.text = $"{combatSystem.GetPlayerPokemonName()}";
        PlayerLevel.text = $"{combatSystem.GetPlayerPokemonLevel()}";
        OpponentInfo.text = $"{combatSystem.GetOpponentPokemonName()}";
        OpponentLevel.text = $"{combatSystem.GetOpponentPokemonLevel()}";

         playerDisplay.GetComponent<Image>().sprite = combatSystem.GetPlayerPokemonSprite();
         opponentDisplay.GetComponent<Image>().sprite = combatSystem.GetOpponentPokemonSprite();
        
        for (int i = 0; i < pokemonSelectionButtons.Length; i++)
        {
            if ((i >= combatSystem.GetPlayerPokemons().Length))
            {
                pokemonSelectionButtons[i].SetActive(false);
                continue;
            }

            if (combatSystem.GetPlayerPokemons()[i].Name == combatSystem.GetPlayerPokemonName())
            {
                currentPokemonName = combatSystem.GetPlayerPokemonName();
                PokemonSelectedName.text = currentPokemonName;
                PokemonSelectedLevel.text = combatSystem.GetPlayerPokemonLevel().ToString();
                
                pokemonSelectionButtons[i].SetActive(false);
                
                continue;
            }
            
            pokemonSelectionButtons[i].SetActive(true);
            pokemonSelectionButtons[i].GetComponentInChildren<TMP_Text>().text =
                combatSystem.GetPlayerPokemons()[i].Name;
            PokemonSelectionLevels[i].text = combatSystem.GetPlayerPokemons()[i].Level.ToString();
        }
        
        for (int i = 0; i < playerMoveSet.Length; i++)
            MoveNames[i].text = playerMoveSet[i].Name;
        
        currentMenu = ActionPannel;
        
        UpdateOpponentInfoText();
        UpdatePlayerInfoText();
        UpdatePlayerHealthBar();
        UpdateOpponentHealthBar();
    }
    
    public void OpenActionPanel()
    {
        SwitchMenu(ActionPannel);
        TextBox.text = $"What will {combatSystem.GetPlayerPokemonName()} do ?";
    }
    
    public void OpenMovePanel()
    {
        SwitchMenu(MovePannel);
        TextBox.text = "Select a move.";
        GameManager.instance.combatSystem.ChoseNextPlayerAction(0);
    }

    public void OpenPokemonPanel()
    {
        SwitchMenu(PokemonPannel);
        PlayerCard.SetActive(false);
        OpponentCard.SetActive(false);
        TextBox.text = "Chose a POKéMON.";
        combatSystem.ChoseNextPlayerAction(2);
    }

    public void OpenItemPanel()
    {
        SwitchMenu(ItemPanel);
        ResetItemButtons();
        TextBox.text = $"What will {combatSystem.GetPlayerPokemonName()} do ?";
    }
    
    public void TryFlee()
    {
        combatSystem.ChoseNextPlayerAction(3);
        combatSystem.NextStep();
    }

    public void SelectMove(int moveIndex)
    {
        ResetMoveButtons();
        combatSystem.ChoseNextMove(moveIndex);
        combatSystem.NextStep();
    }

    public void SelectPokemon(int pokemonIndex)
    {
        ResetSelectButtons();
        combatSystem.ChoseNewPokemon(pokemonIndex);
        combatSystem.NextStep();
    }

    public void SelectItem(int itemIndex)
    {
        combatSystem.ChoseNextPlayerAction(1);
        combatSystem.UseItem(itemIndex);
        combatSystem.NextStep();
    }
    
    public void EnterHoverMove(int moveIndex)
    {
        CurrentPP.text = $"{playerMoveSet[moveIndex].PP}";
        MaxPP.text = $"{playerMoveSet[moveIndex].PP}";
        MoveType.text = $"{playerMoveSet[moveIndex].Type}";
    }

    public void ExitHoverMove()
    {
        //TextBox.text = "Select a move.";
        CurrentPP.text = "";
        MaxPP.text = "";
        MoveType.text = "";
    }

    public void EnterHoverItem(int itemIndex)
    {
        ItemDescription.text = combatSystem.GetPlayerItems()[itemIndex].description;
    }

    public void ExitHoverItem()
    {
        ItemDescription.text = "Chose an item.";
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
                TextBox.text = $"What will {currentPokemonName} do ?";
                break;
            
            case MenuState.PlayerTriedFlee:
                TextBox.text = $"{currentPokemonName} tries to flee.";
                break;
            
            case MenuState.PlayerFled:
                TextBox.text = $"{currentPokemonName} fled !";
                break;
            
            case MenuState.PlayerFledFailed:
                TextBox.text = $"{currentPokemonName} failed !";
                break;
            
            case MenuState.PlayerSwitchedOld:
                TextBox.text = $"Come back, {currentPokemonName} !";
                break;
            
            case MenuState.PlayerSwitchedNew:
                TextBox.text = $"Go, {currentPokemonName} !";
                UpdatePlayerSprite();
                currentPokemonName = combatSystem.GetPlayerPokemonName();
                break;
            
            case MenuState.PlayerUsedItem:
                TextBox.text = $"Player used {combatSystem.GetLastUsedItem().name}";
                break;
            
            case MenuState.PlayerMove:
                UpdateOpponentInfoText();
                if(!combatSystem.GetPlayerMove()) 
                    TextBox.text = $"{currentPokemonName} used [MOVE]";
                else
                    TextBox.text = $"{currentPokemonName} used {combatSystem.GetPlayerMove().Name}";
                break;
            
            case MenuState.PlayerEfficiency:
                UpdateOpponentInfoText();
                var playerEfficiency = TypeTable.GetTypeDamageMultiplier(combatSystem.GetPlayerMove().Type,
                    combatSystem.GetOpponentType());
                if(playerEfficiency < .75f)
                    TextBox.text = "It's not very effective...";
                else if(playerEfficiency > 1.5f)
                    TextBox.text = "It's very effective !";
                break;
            
            case MenuState.PlayerMissed:
                TextBox.text = $"{currentPokemonName} missed !";
                break;
            
            case MenuState.OppenentMove:
                UpdatePlayerInfoText();
                TextBox.text = $"Enemy {combatSystem.GetOpponentPokemonName()} used {combatSystem.GetOpponentMove().Name}";
                break;
            
            case MenuState.OpponentEfficiency:
                UpdatePlayerInfoText();
                var opponentEfficiency = TypeTable.GetTypeDamageMultiplier(combatSystem.GetOpponentMove().Type,
                    combatSystem.GetPlayerPokemonType());
                if(opponentEfficiency < .75f)
                    TextBox.text = "It's not very effective...";
                else if(opponentEfficiency > 1.5f)
                    TextBox.text = "It's very effective !";
                break;
            
            case MenuState.OpponentMiss:
                TextBox.text = $"Enemy {combatSystem.GetOpponentPokemonName()} missed !";
                break;
            
            case MenuState.PlayerFainted:
                TextBox.text = $"{currentPokemonName} fainted !";
                break;
            
            case MenuState.OpponentFainted:
                TextBox.text = $"Enemy {combatSystem.GetOpponentPokemonName()} fainted !";
                break;
            
            case MenuState.OpponentCaught:
                TextBox.text = $"{combatSystem.GetOpponentPokemonName()} was caught !";
                break;
            
            case MenuState.OpponentNotCaught:
                TextBox.text = $"{combatSystem.GetOpponentPokemonName()} broke free !";
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
                    menuState =  MenuState.PlayerSwitchedOld;
                else if (combatSystem.PlayerUsedItem())
                    menuState = MenuState.PlayerUsedItem;
                else
                    menuState = combatSystem.PlayerPlaysFirst() ? MenuState.PlayerMove : MenuState.OppenentMove;
                ActionPannel.SetActive(false);
                break;
            
            case MenuState.PlayerTriedFlee:
                if (combatSystem.PlayerFled())
                    menuState = MenuState.PlayerFled;
                else
                    menuState = MenuState.PlayerFledFailed;
                break;
            
            case MenuState.PlayerFled:
                GameManager.OnFightEnd();
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
            
            case MenuState.PlayerUsedItem:
                if (combatSystem.GetLastUsedItem().itemType == ItemType.Pokeball)
                {
                    menuState = combatSystem.OpponentIsCaught() ? MenuState.OpponentCaught : MenuState.OpponentNotCaught;
                    break;
                }
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
                {
                    UpdateOpponentHealthBar();
                    menuState = MenuState.OpponentFainted;
                }
                else if(combatSystem.PlayerPlaysFirst())
                {
                    UpdateOpponentHealthBar();
                    menuState = MenuState.OppenentMove;
                    break;
                }

                menuState = MenuState.Default;
                OpenActionPanel();
                break;
            
            case MenuState.PlayerMissed:
                if (combatSystem.PlayerPlaysFirst())
                {
                    menuState = MenuState.OppenentMove;
                }
                else
                {
                    menuState = MenuState.Default;
                    OpenActionPanel();
                }
                break;
            
            case MenuState.PlayerEfficiency:
                UpdateOpponentHealthBar();
                if (combatSystem.OpponentFainted())
                    menuState = MenuState.OpponentFainted;
                else
                {
                    if (combatSystem.PlayerPlaysFirst())
                    {
                        menuState = MenuState.OppenentMove;
                    }
                    else
                    {
                        menuState = MenuState.Default;
                        OpenActionPanel();
                    }
                }
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
                    UpdatePlayerHealthBar();
                    if (combatSystem.PlayerFainted())
                    {
                        menuState = MenuState.PlayerFainted;
                        break;
                    }
                    if(combatSystem.PlayerPlaysFirst() || combatSystem.PlayerUsedItem())
                    {
                        menuState = MenuState.Default;
                        OpenActionPanel();
                        break;
                    }
                    menuState = MenuState.PlayerMove;
                }
                break;
            
            case MenuState.OpponentMiss:
                if (combatSystem.PlayerPlaysFirst())
                {
                    menuState = MenuState.Default;
                    OpenActionPanel();
                    break;
                }
                
                menuState = MenuState.PlayerMove;
                break;
            
            case MenuState.OpponentEfficiency:
                UpdatePlayerHealthBar();
                if (combatSystem.PlayerFainted())
                    menuState = MenuState.PlayerFainted;
                else if(combatSystem.PlayerPlaysFirst() || combatSystem.PlayerUsedItem())
                {
                    menuState = MenuState.Default;
                    OpenActionPanel();
                }
                else
                {
                    menuState = MenuState.PlayerMove;
                }
                break;
            
            case MenuState.PlayerFainted:
                menuState = MenuState.Lose;
                break;
            
            case MenuState.OpponentFainted:
                menuState = MenuState.Win;
                break;
            
            case MenuState.OpponentCaught:
                GameManager.GivePlayerPokemon(combatSystem.GetOpponentPokemon());
                menuState = MenuState.Win;
                break;
            
            case MenuState.OpponentNotCaught:
                menuState = MenuState.OppenentMove;
                break;
            
            case MenuState.Win:
                UpdatePlayerXPBar();
                GameManager.OnFightEnd();
                break;
            
            case MenuState.Lose:
                GameManager.OnFightEnd();
                break;
        }
        
        DisplayAction();
    }

    private void UpdatePlayerHealthBar()
    {
        float hpRatio = (float)combatSystem.GetPlayerPokemonHp() / combatSystem.GetPlayerPokemonMaxHp();
        PlayerHealthBar.fillAmount = hpRatio;
        if (hpRatio < .25) PlayerHealthBar.sprite = Red;
        else if (hpRatio < .5) PlayerHealthBar.sprite = Yellow;
    }

    private void UpdatePlayerXPBar()
    {
        PlayerXPBar.fillAmount = combatSystem.GetPlayerCurrentPokemon().Exp / combatSystem.GetPlayerCurrentPokemon().Exp;
    }
    
    private void UpdateOpponentHealthBar()
    {
        float hpRatio = (float)combatSystem.GetOpponentPokemonHp() / combatSystem.GetOpponentPokemon().TotalStats.HP;
        OpponentHealthBar.fillAmount = hpRatio;
        if (hpRatio < .25) OpponentHealthBar.sprite = Red;
        else if (hpRatio < .5) OpponentHealthBar.sprite = Yellow;
    }
    
    private void UpdatePlayerInfoText()
    {
        PlayerInfo.text = $"{combatSystem.GetPlayerPokemonName()}";
        PlayerHealth.text = $"{combatSystem.GetPlayerPokemonHp()}/{combatSystem.GetPlayerPokemonMaxHp()}";
    }

    private void UpdateOpponentInfoText()
    {
        OpponentInfo.text = $"{combatSystem.GetOpponentPokemonName()}";
    }

    private void UpdatePlayerSprite()
    {
        playerDisplay.GetComponent<Image>().sprite = combatSystem.GetPlayerPokemonSprite();
    }

    private void UpdateOpponentSprite()
    {
        opponentDisplay.GetComponent<Image>().sprite = combatSystem.GetOpponentPokemonSprite();
    }

    private void ResetMoveButtons()
    {
        for (int i = 0; i < playerMoveSet.Length; i++)
            MoveNames[i].text = playerMoveSet[i].Name;
    }
    
    private void ResetSelectButtons()
    {
        for (int i = 0; i < pokemonSelectionButtons.Length; i++)
        {
            if ((i >= combatSystem.GetPlayerPokemons().Length))
            {
                pokemonSelectionButtons[i].SetActive(false);
                continue;
            }

            if (combatSystem.GetPlayerPokemons()[i].Name == combatSystem.GetPlayerPokemonName())
            {
                PokemonSelectedName.text = combatSystem.GetPlayerPokemonName();
                PokemonSelectedLevel.text = combatSystem.GetPlayerPokemonLevel().ToString();
                
                pokemonSelectionButtons[i].SetActive(false);
                
                continue;
            }
            
            pokemonSelectionButtons[i].SetActive(true);
            pokemonSelectionButtons[i].GetComponentInChildren<TMP_Text>().text =
                combatSystem.GetPlayerPokemons()[i].Name;
            PokemonSelectionLevels[i].text = combatSystem.GetPlayerPokemons()[i].Level.ToString();
        }
    }

    private void ResetItemButtons()
    {
        for (int i = 0; i < itemButtons.Length; i++)
        {
            if (i >= combatSystem.GetPlayerItems().Length)
            {
                itemButtons[i].SetActive(false);
                continue;
            }

            itemButtons[i].SetActive(true);
            ItemNames[i].text = combatSystem.GetPlayerItems()[i].name;
        }
    }
    
    private void GetValidateInput()
    {
        if (input.Menu.Validate.triggered && menuState != MenuState.Default)
             GoToNextStep();
    }

    private void GetCancelInput()
    {
        if(input.Menu.Cancel.triggered && menuState == MenuState.Default)
            OpenActionPanel();
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
        PlayerUsedItem,
        PlayerMissed,
        PlayerEfficiency,
        PlayerFainted,
        OppenentMove,
        OpponentMiss,
        OpponentEfficiency,
        OpponentFainted,
        OpponentCaught,
        OpponentNotCaught,
        Win,
        Lose,
    }

    public void StartCombatAgainstWildPokemon(PokemonSO opponentPokemon)
    {
        combatSystem = GameManager.instance.combatSystem;
        
        CombatUICanvas.SetActive(true);
        CardsPannel.SetActive(true);
        ActionPannel.SetActive(true);
        MovePannel.SetActive(false);
        TextBox.text = $"A wild {opponentPokemon.Name} appeared !";
        
        playerMoveSet = combatSystem.GetPlayerMoveArray();
        PlayerInfo.text = $"{combatSystem.GetPlayerPokemonName()}";
        PlayerLevel.text = $"{combatSystem.GetPlayerPokemonLevel()}";
        OpponentInfo.text = $"{opponentPokemon.Name}";
        OpponentLevel.text = $"{opponentPokemon.Level}";

        playerDisplay.GetComponent<Image>().sprite = combatSystem.GetPlayerPokemonSprite();
        opponentDisplay.GetComponent<Image>().sprite = opponentPokemon.BackSprite;
        
        for (int i = 0; i < pokemonSelectionButtons.Length; i++)
        {
            if ((i >= combatSystem.GetPlayerPokemons().Length))
            {
                pokemonSelectionButtons[i].SetActive(false);
                continue;
            }

            if (combatSystem.GetPlayerPokemons()[i].Name == combatSystem.GetPlayerPokemonName())
            {
                currentPokemonName = combatSystem.GetPlayerPokemonName();
                PokemonSelectedName.text = currentPokemonName;
                PokemonSelectedLevel.text = combatSystem.GetPlayerPokemonLevel().ToString();
                
                pokemonSelectionButtons[i].SetActive(false);
                
                continue;
            }
            
            pokemonSelectionButtons[i].SetActive(true);
            pokemonSelectionButtons[i].GetComponentInChildren<TMP_Text>().text =
                combatSystem.GetPlayerPokemons()[i].Name;
            PokemonSelectionLevels[i].text = combatSystem.GetPlayerPokemons()[i].Level.ToString();
        }
        
        for (int i = 0; i < playerMoveSet.Length; i++)
            MoveNames[i].text = playerMoveSet[i].Name;
        
        currentMenu = ActionPannel;
    }
}
