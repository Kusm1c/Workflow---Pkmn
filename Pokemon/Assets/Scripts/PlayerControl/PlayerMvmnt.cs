using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerMvmnt : MonoBehaviour
{
    [SerializeField] private float tileSize;
    [SerializeField] private float speed;

    [SerializeField] private KeyCode acceptKey;
    [SerializeField] private KeyCode cancelKey;
    [SerializeField] private KeyCode menuKey;

    [SerializeField] private KeyCode runKey;
    [SerializeField] private KeyCode interactKey;

    [SerializeField] private KeyCode upKey;
    [SerializeField] private KeyCode downKey;
    [SerializeField] private KeyCode leftKey;
    [SerializeField] private KeyCode rightKey;

    //From 0 to 3 boy Sprite, From 4 to 7 female Sprite
    [SerializeField] private List<Sprite> walkUpSprites;
    [SerializeField] private List<Sprite> walkDownSprites;
    [SerializeField] private List<Sprite> walkRightSprites;

    [SerializeField] private List<Sprite> runUpSprites;
    [SerializeField] private List<Sprite> runDownSprites;
    [SerializeField] private List<Sprite> runRightSprites;

    [SerializeField] private List<Sprite> bikeUpSprites;
    [SerializeField] private List<Sprite> bikeDownSprites;
    [SerializeField] private List<Sprite> bikeRightSprites;

    //boy 0 = down 1 = up 2 = right, 
    //girl 3 = down 4 = up 5 = right
    [SerializeField] private List<Sprite> notMovingSprites;

    private int currentAnimationFrame = 0;

    [SerializeField] private List<EcounterTableSO> ecounterTable;

    private bool isMoving = false;
    private bool isRunning = false;
    private bool isBoy = false;

    private GameObject player;

    public bool isFighting = false;

    public static PlayerMvmnt Instance;
    
    [SerializeField] private AnimationCurve jumpCurve;
    
    public Vector3 up { get { return new Vector3(0, 0, 1); } }
    private Vector3 down { get { return new Vector3(0, 0, -1); } }
    private Vector3 left { get { return new Vector3(-1, 0, 0); } }
    private Vector3 right { get { return new Vector3(1, 0, 0); } }
    
    public Vector2 currentPos
    {
        get => transform.position;
        set => transform.position = value;
    }

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        player = transform.gameObject;
        player.GetComponent<SpriteRenderer>().sprite = isBoy ? notMovingSprites[0] : notMovingSprites[3];
        CameraScript.instance.target = player.transform;
    }

    void Update()
    {
        if (isFighting) return;
        if (Input.GetKeyDown(acceptKey)) Accept();
        else if (Input.GetKeyDown(cancelKey)) Cancel();
        else if (Input.GetKeyDown(menuKey)) Menu();
        else if (Input.GetKey(runKey)) Run();
        else if (Input.GetKeyDown(interactKey)) Interact();
        else if (Input.GetKey(upKey)) MovePlayer(Vector3.up);
        else if (Input.GetKey(downKey)) MovePlayer(Vector3.down);
        else if (Input.GetKey(leftKey)) MovePlayer(Vector3.left);
        else if (Input.GetKey(rightKey)) MovePlayer(Vector3.right);
        else Idle();

        currentPos = transform.position;
    }

    private void Idle()
    {
        if (isBoy)
        {
            if (lastDirection == Vector3.up) player.GetComponent<SpriteRenderer>().sprite = notMovingSprites[1];
            else if (lastDirection == Vector3.down) player.GetComponent<SpriteRenderer>().sprite = notMovingSprites[0];
            else if (lastDirection == Vector3.right)
            {
                player.GetComponent<SpriteRenderer>().sprite = notMovingSprites[2];
                player.GetComponent<SpriteRenderer>().flipX = false;
            }
            else if (lastDirection == Vector3.left)
            {
                player.GetComponent<SpriteRenderer>().sprite = notMovingSprites[2];
                player.GetComponent<SpriteRenderer>().flipX = true;
            }
        }
        else
        {
            if (lastDirection == Vector3.up) player.GetComponent<SpriteRenderer>().sprite = notMovingSprites[4];
            else if (lastDirection == Vector3.down) player.GetComponent<SpriteRenderer>().sprite = notMovingSprites[3];
            else if (lastDirection == Vector3.right)
            {
                player.GetComponent<SpriteRenderer>().sprite = notMovingSprites[5];
                player.GetComponent<SpriteRenderer>().flipX = false;
            }
            else if (lastDirection == Vector3.left)
            {
                player.GetComponent<SpriteRenderer>().sprite = notMovingSprites[5];
                player.GetComponent<SpriteRenderer>().flipX = true;
            }
        }
    }

    private void Accept()
    {
        Debug.Log("Accept");
    }

    private void Cancel()
    {
        Debug.Log("Cancel");
    }

    private void Menu()
    {
        MenuManager.OpenActionMenu(OptionManager.Instance.menuActionPanel);
    }

    private void Run()
    {
        Debug.Log("Run");
    }
    private Vector3 lastDirection;

    private void Interact()
    {
        int x, y;
        if (lastDirection == Vector3.up && MapGenerator.Instance.interactableTilesList.Any(pos =>
                Mathf.Abs(transform.position.x - pos.interactablePositions.x) < tolerance.x &&
                Mathf.Abs(transform.position.y + tileSize - pos.interactablePositions.y) < tolerance.y))
        {
            var tileInteracted = MapGenerator.Instance.interactableTilesList.First(pos =>
                Mathf.Abs(transform.position.x - pos.interactablePositions.x) < tolerance.x &&
                Mathf.Abs(transform.position.y + tileSize - pos.interactablePositions.y) < tolerance.y);
            Debug.Log("Interact up");
            if (tileInteracted.npc != null)
            {
                if (tileInteracted.npc.GetComponent<NonTrainer>())
                {
                    NonTrainer aaa = tileInteracted.npc.GetComponent<NonTrainer>();
                    if (aaa.firstTimeTalkItem != null)
                    {
                        GameManager.instance.SetPlayerItemQuantity(aaa.firstTimeTalkItem, 1);
                        aaa.firstTimeTalkItem = null;
                    }
                    Debug.Log(aaa.firstTimeTalk ? aaa.firstTimeTalkText : aaa.talkText);
                    aaa.firstTimeTalk = false;
                }
                else if (tileInteracted.npc.GetComponent<TrainerBattle>())
                {
                    TrainerBattle aaa = tileInteracted.npc.GetComponent<TrainerBattle>();
                    if (aaa.hasLost)
                    {
                        Debug.Log(aaa.AfterBattleText);
                    }
                    else
                    {
                        Debug.Log(aaa.BeforeBattleText);
                        GameManager.instance.OnFightStart(aaa.playerPokemonList[0]);
                        CombatUI.Instance.SetTrainerName(aaa.Name);
                        CombatUI.Instance.StartCombat();
                    }
                }
            }
        }
        else if (lastDirection == Vector3.down && MapGenerator.Instance.interactableTilesList.Any(pos =>
                     Mathf.Abs(transform.position.x - pos.interactablePositions.x) < tolerance.x &&
                     Mathf.Abs(transform.position.y - tileSize - pos.interactablePositions.y) < tolerance.y))
        {
            var tileInteracted = MapGenerator.Instance.interactableTilesList.First(pos =>
                Mathf.Abs(transform.position.x - pos.interactablePositions.x) < tolerance.x &&
                Mathf.Abs(transform.position.y - tileSize - pos.interactablePositions.y) < tolerance.y);
            Debug.Log("Interact down");
            if (tileInteracted.npc != null)
            {
                if (tileInteracted.npc.GetComponent<NonTrainer>())
                {
                    NonTrainer aaa = tileInteracted.npc.GetComponent<NonTrainer>();
                    if (aaa.firstTimeTalkItem != null)
                    {
                        GameManager.instance.SetPlayerItemQuantity(aaa.firstTimeTalkItem, 1);
                        aaa.firstTimeTalkItem = null;
                    }
                    Debug.Log(aaa.firstTimeTalk ? aaa.firstTimeTalkText : aaa.talkText);
                    aaa.firstTimeTalk = false;
                }
                else if (tileInteracted.npc.GetComponent<TrainerBattle>())
                {
                    TrainerBattle aaa = tileInteracted.npc.GetComponent<TrainerBattle>();
                    if (aaa.hasLost)
                    {
                        Debug.Log(aaa.AfterBattleText);
                    }
                    else
                    {
                        Debug.Log(aaa.BeforeBattleText);
                        GameManager.instance.OnFightStart(aaa.playerPokemonList[0]);
                        CombatUI.Instance.SetTrainerName(aaa.Name);
                        CombatUI.Instance.StartCombat();
                    }
                }
            }
        }
        else if (lastDirection == Vector3.right && MapGenerator.Instance.interactableTilesList.Any(pos =>
                     Mathf.Abs(transform.position.x + tileSize - pos.interactablePositions.x) < tolerance.x &&
                     Mathf.Abs(transform.position.y - pos.interactablePositions.y) < tolerance.y))
        {
            var tileInteracted = MapGenerator.Instance.interactableTilesList.First(pos =>
                Mathf.Abs(transform.position.x + tileSize - pos.interactablePositions.x) < tolerance.x &&
                Mathf.Abs(transform.position.y - pos.interactablePositions.y) < tolerance.y);
            Debug.Log("Interact right");
            if (tileInteracted.npc != null)
            {
                if (tileInteracted.npc.GetComponent<NonTrainer>())
                {
                    NonTrainer aaa = tileInteracted.npc.GetComponent<NonTrainer>();
                    if (aaa.firstTimeTalkItem != null)
                    {
                        GameManager.instance.SetPlayerItemQuantity(aaa.firstTimeTalkItem, 1);
                        aaa.firstTimeTalkItem = null;
                    }
                    Debug.Log(aaa.firstTimeTalk ? aaa.firstTimeTalkText : aaa.talkText);
                    aaa.firstTimeTalk = false;
                    aaa.GetComponent<SpriteRenderer>().flipX = false;
                }
                else if (tileInteracted.npc.GetComponent<TrainerBattle>())
                {
                    TrainerBattle aaa = tileInteracted.npc.GetComponent<TrainerBattle>();
                    if (aaa.hasLost)
                    {
                        Debug.Log(aaa.AfterBattleText);
                    }
                    else
                    {
                        Debug.Log(aaa.BeforeBattleText);
                        GameManager.instance.OnFightStart(aaa.playerPokemonList[0]);
                        CombatUI.Instance.SetTrainerName(aaa.Name);
                        CombatUI.Instance.StartCombat();
                    }
                }
            }
        }
        else if (lastDirection == Vector3.left && MapGenerator.Instance.interactableTilesList.Any(pos =>
                     Mathf.Abs(transform.position.x - tileSize - pos.interactablePositions.x) < tolerance.x &&
                     Mathf.Abs(transform.position.y - pos.interactablePositions.y) < tolerance.y))
        {
            var tileInteracted = MapGenerator.Instance.interactableTilesList.First(pos =>
                Mathf.Abs(transform.position.x - tileSize - pos.interactablePositions.x) < tolerance.x &&
                Mathf.Abs(transform.position.y - pos.interactablePositions.y) < tolerance.y);
            Debug.Log("Interact left");
            if (tileInteracted.npc != null)
            {
                if (tileInteracted.npc.GetComponent<NonTrainer>())
                {
                    NonTrainer aaa = tileInteracted.npc.GetComponent<NonTrainer>();
                    if (aaa.firstTimeTalkItem != null)
                    {
                        GameManager.instance.SetPlayerItemQuantity(aaa.firstTimeTalkItem, 1);
                        aaa.firstTimeTalkItem = null;
                    }
                    Debug.Log(aaa.firstTimeTalk ? aaa.firstTimeTalkText : aaa.talkText);
                    aaa.firstTimeTalk = false;
                    aaa.GetComponent<SpriteRenderer>().flipX = true;
                }
                else if (tileInteracted.npc.GetComponent<TrainerBattle>())
                {
                    TrainerBattle aaa = tileInteracted.npc.GetComponent<TrainerBattle>();
                    if (aaa.hasLost)
                    {
                        Debug.Log(aaa.AfterBattleText);
                    }
                    else
                    {
                        Debug.Log(aaa.BeforeBattleText);
                        GameManager.instance.OnFightStart(aaa.playerPokemonList[0]);
                        CombatUI.Instance.SetTrainerName(aaa.Name);
                        CombatUI.Instance.StartCombat();
                    }
                }
            }
        }
    }


    private void MovePlayer(Vector3 direction)
    {
        if (isMoving) return;
        lastDirection = direction;
        isMoving = true;
        if (CheckCollisionInDirection(direction)) return;
        StartCoroutine(Move(direction,null));
    }

    private Vector3 tolerance = new Vector3(0.1f, 0.1f, 0.1f);

    private Color GetPixelColorOnCollisionMap(Vector2 playerPos)
    {
        Texture2D tex = MapGenerator.Instance.collisionTexture;
        Vector2 pixelUV = playerPos;
        pixelUV.x *= tex.width;
        pixelUV.y *= tex.height;
        return tex.GetPixel((int) pixelUV.x, (int) pixelUV.y);
    }

    private bool CheckCollisionInDirection(Vector3 direction)
    {
        // red = blocked
        // magenta = blocked from below
        // yellow = interactable
        // black = door
        // white = else
        if (MapGenerator.Instance.blockedPositions.Any(pos =>
                Mathf.Abs(transform.position.x + direction.x * tileSize - pos.x) < tolerance.x &&
                Mathf.Abs(transform.position.y + direction.y * tileSize - pos.y) < tolerance.y))
        {
            isMoving = false;
            return true;
        }
        
        if (MapGenerator.Instance.interactableTilesList.Any(pos =>
                Mathf.Abs(transform.position.x + direction.x * tileSize - pos.interactablePositions.x) < tolerance.x &&
                Mathf.Abs(transform.position.y + direction.y * tileSize - pos.interactablePositions.y) < tolerance.y))
        {
            isMoving = false;
            return true;
        }
        
        foreach (var pos in MapGenerator.Instance.blockedFromBelowPositions)
        {
            if (direction == Vector3.down) return false;
            if (Mathf.Abs(transform.position.x + direction.x * tileSize - pos.x) < tolerance.x &&
                Mathf.Abs(transform.position.y + direction.y * tileSize - pos.y) < tolerance.y)
            {
                isMoving = false;
                return true;
            }
        }

        if (Math.Abs(transform.position.x + direction.x * tileSize) > 32.6f ||
            Mathf.Abs(transform.position.y + direction.y * tileSize) > 32.6f)
        {
            isMoving = false;
            return true;
        }
        
        
        
        return false;
    }

    private void UseDoor(Vector3 doorPosition)
    {
        int nextDoor = Doors.nextDoorFromPosition[doorPosition];
        transform.position = new Vector3(Doors.positionFromDoor[nextDoor].x + lastDirection.x * tileSize,
            Doors.positionFromDoor[nextDoor].y + lastDirection.y * tileSize, -0.1f);
        CameraScript.instance.cameraBlockedPosition = Doors.blockedCameraPositionWhenTookDoor[nextDoor];
        CameraScript.instance.maxCameraPosition = Doors.maxBlockedCameraPositionWhenTookDoor[nextDoor];
        CameraScript.instance.minCameraPosition = Doors.minBlockedCameraPositionWhenTookDoor[nextDoor];
    }

    private IEnumerator Move(Vector3 direction, float? speedParam)
    {
        float elapsedTime = 0.0f;
        if (isBoy)
        {
            if (!isRunning)
            {
                if (direction == Vector3.up)
                {
                    player.GetComponent<SpriteRenderer>().sprite = walkUpSprites[currentAnimationFrame];
                }
                else if (direction == Vector3.down)
                {
                    player.GetComponent<SpriteRenderer>().sprite = walkDownSprites[currentAnimationFrame];
                }
                else if (direction == Vector3.right)
                {
                    player.GetComponent<SpriteRenderer>().sprite = walkRightSprites[currentAnimationFrame];
                    player.GetComponent<SpriteRenderer>().flipX = false;
                }
                else if (direction == Vector3.left)
                {
                    player.GetComponent<SpriteRenderer>().sprite = walkRightSprites[currentAnimationFrame];
                    player.GetComponent<SpriteRenderer>().flipX = true;
                }
            }
        }
        else
        {
            if (!isRunning)
            {
                if (direction == Vector3.up)
                {
                    player.GetComponent<SpriteRenderer>().sprite = walkUpSprites[currentAnimationFrame + 4];
                }
                else if (direction == Vector3.down)
                {
                    player.GetComponent<SpriteRenderer>().sprite = walkDownSprites[currentAnimationFrame + 4];
                }
                else if (direction == Vector3.right)
                {
                    player.GetComponent<SpriteRenderer>().sprite = walkRightSprites[currentAnimationFrame + 4];
                    player.GetComponent<SpriteRenderer>().flipX = false;
                }
                else if (direction == Vector3.left)
                {
                    player.GetComponent<SpriteRenderer>().sprite = walkRightSprites[currentAnimationFrame + 4];
                    player.GetComponent<SpriteRenderer>().flipX = true;
                }
            }
        }

        Vector3 startingPos = player.transform.position;
        Vector3 targetPos = startingPos + direction * tileSize;
        while (elapsedTime < 1.0f)
        {
            if (hasToJump)
            {
                player.transform.position = Vector3.Lerp(startingPos, targetPos, elapsedTime);
                elapsedTime = MoveAction(speedParam, elapsedTime);
                yield return null;
                continue;
            }
            player.transform.position = Vector3.Lerp(startingPos, targetPos, elapsedTime);
            elapsedTime = MoveAction(speedParam, elapsedTime);
            yield return null;
        }
        hasToJump = false;

        currentAnimationFrame = (currentAnimationFrame + 1) % 4;
        player.transform.position = targetPos;
        CheckForDoor();
        CheckForGrass();
        CheckForBlockedFromBelow();
        isMoving = false;
    }

    private float MoveAction(float? speedParam, float elapsedTime)
    {
        if (speedParam == null) elapsedTime += Time.deltaTime * speed;
        else
        {
            var vector3 = player.transform.position;
            vector3.y += jumpCurve.Evaluate(elapsedTime);
            player.transform.position = vector3;
            elapsedTime += Time.deltaTime * (float)speedParam;
        }

        return elapsedTime;
    }

    public bool hasToJump = false;
    private void CheckForBlockedFromBelow()
    {
        if (!MapGenerator.Instance.blockedFromBelowPositions.Any(pos =>
                Mathf.Abs(transform.position.x - pos.x) < tolerance.x &&
                Mathf.Abs(transform.position.y - pos.y) < tolerance.y)) return;
        Debug.Log("Jump");
        hasToJump = true;
        StartCoroutine(Move(lastDirection, speed/4));
    }


    private void CheckForGrass()
    {
        if (MapGenerator.Instance.grassPositions.Any(pos =>
                Mathf.Abs(transform.position.x - pos.x) < tolerance.x &&
                Mathf.Abs(transform.position.y - pos.y) < tolerance.y))
        {
            int currentEncounterTableIndex = ecounterTable.TakeWhile(table =>
                !(table.grassPositionmin.x <= transform.position.x) ||
                !(table.grassPositionmax.x >= transform.position.x) ||
                !(table.grassPositionmin.y <= transform.position.y) ||
                !(table.grassPositionmax.y >= transform.position.y)).Count();
            // Debug.Log("Grass");
            // bool pokemonAppeared = false;

            foreach (var pokemon in ecounterTable[currentEncounterTableIndex].pokemonList)
            {
                int pokemonOdd = 0;
                switch (pokemon.rarity)
                {
                    case Rarity.Common:
                        pokemonOdd = 20;
                        break;
                    case Rarity.Uncommon:
                        pokemonOdd = 10;
                        break;
                    case Rarity.Rare:
                        pokemonOdd = 2;
                        break;
                }

                if (Random.Range(0, 100) >= pokemonOdd) continue;
                // pokemonAppeared = true;
                Debug.Log("Pokemon appeared " + pokemon.pokemon.name);
                GameManager.instance.OnFightStart(pokemon.pokemon);
                CombatUI.Instance.StartCombat();
                isFighting = true;
                break;
            }
        }
    }
    
    

    private void CheckForDoor()
    {
        if (MapGenerator.Instance.doorsPositions.Any(pos =>
                Mathf.Abs(transform.position.x - pos.x) < tolerance.x &&
                Mathf.Abs(transform.position.y - pos.y) < tolerance.y))
        {
            UseDoor(MapGenerator.Instance.doorsPositions.First(pos =>
                Mathf.Abs(transform.position.x - pos.x) < tolerance.x &&
                Mathf.Abs(transform.position.y - pos.y) < tolerance.y));
        }
    }
}