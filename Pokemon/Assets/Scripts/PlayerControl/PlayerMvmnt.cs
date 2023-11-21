using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

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

    private int currentAnimationFrame = 0;

    //boy 0 = down 1 = up 2 = right, 
    //girl 3 = down 4 = up 5 = right
    [SerializeField] private List<Sprite> notMovingSprites;

    private bool isMoving = false;
    private bool isRunning = false;
    private bool isBoy = false;

    private GameObject player;

    private void Start()
    {
        player = transform.gameObject;
        player.GetComponent<SpriteRenderer>().sprite = isBoy ? notMovingSprites[0] : notMovingSprites[3];
        CameraScript.instance.target = player.transform;
    }
    
    void Update()
    {
        if (Input.GetKey(acceptKey)) Accept();
        else if (Input.GetKey(cancelKey)) Cancel();
        else if (Input.GetKey(menuKey)) Menu();
        else if (Input.GetKey(runKey)) Run();
        else if (Input.GetKey(interactKey)) Interact();
        else if (Input.GetKey(upKey)) MovePlayer(Vector3.up);
        else if (Input.GetKey(downKey)) MovePlayer(Vector3.down);
        else if (Input.GetKey(leftKey)) MovePlayer(Vector3.left);
        else if (Input.GetKey(rightKey)) MovePlayer(Vector3.right);
        else Idle();
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
            else if (lastDirection == Vector3.right) player.GetComponent<SpriteRenderer>().sprite = notMovingSprites[5];
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
        Debug.Log("Menu");
    }

    private void Run()
    {
        Debug.Log("Run");
    }

    private void Interact()
    {
        Debug.Log("Interact");
    }

    private Vector3 lastDirection;

    private void MovePlayer(Vector3 direction)
    {
        if (isMoving) return;
        lastDirection = direction;
        isMoving = true;
        if (CheckCollisionInDirection(direction)) return;
        StartCoroutine(Move(direction));
    }

    private Vector3 tolerance = new Vector3(0.1f, 0.1f, 0.1f);

    private bool CheckCollisionInDirection(Vector3 direction)
    {
        if (MapGenerator.Instance.blockedPositions.Any(pos =>
                Mathf.Abs(transform.position.x + direction.x * tileSize - pos.x) < tolerance.x &&
                Mathf.Abs(transform.position.y + direction.y * tileSize - pos.y) < tolerance.y))
        {
            isMoving = false;
            return true;
        }

        if (MapGenerator.Instance.interactablePositions.Any(pos =>
                Mathf.Abs(transform.position.x + direction.x * tileSize - pos.x) < tolerance.x &&
                Mathf.Abs(transform.position.y + direction.y * tileSize - pos.y) < tolerance.y))
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
        return false;
    }

    private void UseDoor(Vector3 doorPosition)
    {
        int nextDoor = Doors.nextDoorFromPosition[doorPosition];
        transform.position = new Vector3(Doors.positionFromDoor[nextDoor].x + lastDirection.x * tileSize, Doors.positionFromDoor[nextDoor].y +lastDirection.y * tileSize, -0.1f);
    }

    private IEnumerator Move(Vector3 direction)
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
            player.transform.position = Vector3.Lerp(startingPos, targetPos, elapsedTime);
            elapsedTime += Time.deltaTime * speed;
            yield return null;
        }

        currentAnimationFrame = (currentAnimationFrame + 1) % 4;
        player.transform.position = targetPos;
        CheckForDoor();
        isMoving = false;
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