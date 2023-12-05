using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] private Sprite mapSprite;
    [SerializeField] private GameObject player;
    [SerializeField] private Vector3 playerSpawnPosition;
    [SerializeField] private GameObject actionTile;

    [SerializeField] private GameObject grassTileSample;
    [SerializeField] private GameObject flowerTileSample;
    [SerializeField] private List<GameObject> waterTileSamples;

    public List<Vector3> blockedPositions;
    [SerializeField] private List<GameObject> blockedTiles;

    public List<Vector3> doorsPositions;
    public List<GameObject> doorsTiles;

    public List<Vector3> interactablePositions;
    [SerializeField] private List<GameObject> interactableTiles;

    public List<Vector3> blockedFromBelowPositions;
    [SerializeField] private List<GameObject> blockedFromBelowTiles;

    public List<Vector3> grassPositions;

    private GameObject grassTileParent;
    private GameObject flowerTileParent;
    private GameObject waterTileParent;
    private GameObject doorsTileParent;
    private GameObject interactableTileParent;
    private GameObject blockedTileParent;
    private GameObject blockedFromBelowTileParent;

    private GameObject actionTileParent;

    [SerializeField] private List<GameObject> TilesToDelete;

    public Texture2D collisionTexture;
    
    [ContextMenu("Generate Collision Map")]
    public void GenerateCollisionMap()
    {
        //generate the collisionTexture using the mapSprite and the blockedPositions and the size of the actionTile
        Vector2 sizeOfMap = mapSprite.bounds.size;
        Vector2 bottomLeft = new Vector2(-sizeOfMap.x / 2, -sizeOfMap.y / 2);
        Vector2 topRight = new Vector2(sizeOfMap.x / 2, sizeOfMap.y / 2);
        Vector2 actionTileSize = actionTile.GetComponent<SpriteRenderer>().bounds.size;
        collisionTexture = new Texture2D((int) (sizeOfMap.x / actionTileSize.x), (int) (sizeOfMap.y / actionTileSize.y));
        for (float x = bottomLeft.x + actionTileSize.x / 2; x < topRight.x; x += actionTileSize.x)
        {
            for (float y = bottomLeft.y + actionTileSize.y / 2; y < topRight.y; y += actionTileSize.y)
            {
                Color pixelColor = mapSprite.texture.GetPixelBilinear((x - bottomLeft.x) / sizeOfMap.x,
                    (y - bottomLeft.y) / sizeOfMap.y);

                if (blockedPositions.Contains(new Vector3(x, y, 0)))
                {
                    collisionTexture.SetPixel((int) (x / actionTileSize.x), (int) (y / actionTileSize.y), Color.red);
                }
                else if (blockedFromBelowPositions.Contains(new Vector3(x, y, 0)))
                {
                    collisionTexture.SetPixel((int) (x / actionTileSize.x), (int) (y / actionTileSize.y), Color.magenta);
                }
                else if (doorsPositions.Contains(new Vector3(x, y, 0)))
                {
                    collisionTexture.SetPixel((int) (x / actionTileSize.x), (int) (y / actionTileSize.y), Color.black);
                }
                else if (interactablePositions.Contains(new Vector3(x, y, 0)))
                {
                    collisionTexture.SetPixel((int) (x / actionTileSize.x), (int) (y / actionTileSize.y), Color.yellow);
                }
                else
                {
                    collisionTexture.SetPixel((int) (x / actionTileSize.x), (int) (y / actionTileSize.y), Color.white);
                }
            }
        }
        //save the collisionTexture as a png
        byte[] bytes = collisionTexture.EncodeToPNG();
        System.IO.File.WriteAllBytes(Application.dataPath + "/Resources/SaveFiles/CollisionMap.png", bytes);
    }

    public static MapGenerator Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        GenerateMap();
        SetGrassPositions();
        GenerateCollisionMap();
    }

    private void SetGrassPositions()
    {
        grassPositions = new List<Vector3>();
        foreach (var grassTile in grassTileParent.GetComponentsInChildren<Transform>())
        {
            if (grassTile.name == "Grass")
            {
                grassPositions.Add(grassTile.transform.position);
            }
        }
    }

    private void Start()
    {
        SpawnPlayer();
        SetTilesInvisible();
    }

    private void SetTilesInvisible()
    {
        foreach (var tile in grassTileParent.GetComponentsInChildren<SpriteRenderer>())
        {
            tile.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
        }

        foreach (var tile in flowerTileParent.GetComponentsInChildren<SpriteRenderer>())
        {
            tile.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
        }

        foreach (var tile in waterTileParent.GetComponentsInChildren<SpriteRenderer>())
        {
           tile.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
        }

        foreach (var tile in actionTileParent.GetComponentsInChildren<SpriteRenderer>())
        {
            tile.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
        }

        foreach (var tile in blockedTileParent.GetComponentsInChildren<SpriteRenderer>())
        {
            tile.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
        }

        foreach (var tile in blockedFromBelowTileParent.GetComponentsInChildren<SpriteRenderer>())
        {
            tile.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
        }

        foreach (var tile in doorsTileParent.GetComponentsInChildren<SpriteRenderer>())
        {
            tile.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
        }

        foreach (var tile in interactableTileParent.GetComponentsInChildren<SpriteRenderer>())
        {
            tile.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
        }
    }

    private void SpawnPlayer()
    {
        player = Instantiate(player, playerSpawnPosition, Quaternion.identity);
        
    }

    private GameObject map;
    [ContextMenu("Generate Map")]
    private void GenerateMap()
    {
        if (map == null)
        {
            map = CreateMap();
        }
        
        GenerateTiles(map);
    }

    private void GenerateTiles(GameObject map)
    {
        CreateParent(map);

        Vector2 sizeOfMap = mapSprite.bounds.size;
        Vector2 bottomLeft = new Vector2(-sizeOfMap.x / 2, -sizeOfMap.y / 2);
        Vector2 topRight = new Vector2(sizeOfMap.x / 2, sizeOfMap.y / 2);
        Vector2 actionTileSize = actionTile.GetComponent<SpriteRenderer>().bounds.size;
        for (float x = bottomLeft.x + actionTileSize.x / 2; x < topRight.x; x += actionTileSize.x)
        {
            for (float y = bottomLeft.y + actionTileSize.y / 2; y < topRight.y; y += actionTileSize.y)
            {
                Color pixelColor = mapSprite.texture.GetPixelBilinear((x - bottomLeft.x) / sizeOfMap.x,
                    (y - bottomLeft.y) / sizeOfMap.y);

                foreach (var tile in from deepWaterTileSample in waterTileSamples
                         where Math.Abs(pixelColor.r - deepWaterTileSample.GetComponent<SpriteRenderer>().color.r) <
                               0.0001f &&
                               Math.Abs(pixelColor.g - deepWaterTileSample.GetComponent<SpriteRenderer>().color.g) <
                               0.0001f &&
                               Math.Abs(pixelColor.b - deepWaterTileSample.GetComponent<SpriteRenderer>().color.b) <
                               0.0001f
                         select Instantiate(actionTile, new Vector3(x, y, 0), Quaternion.identity))
                {
                    tile.GetComponent<SpriteRenderer>().color = pixelColor;
                    tile.GetComponent<SpriteRenderer>().color = new Color(tile.GetComponent<SpriteRenderer>().color.r,
                        tile.GetComponent<SpriteRenderer>().color.g, tile.GetComponent<SpriteRenderer>().color.b, 0.6f);
                    tile.name = "Deep Water";
                    tile.transform.parent = waterTileParent.transform;
                    continue;
                }

                if (blockedPositions.Contains(new Vector3(x, y, 0)))
                {
                    GameObject tile = Instantiate(actionTile, new Vector3(x, y, 0), Quaternion.identity);
                    tile.GetComponent<SpriteRenderer>().color = Color.red;
                    tile.GetComponent<SpriteRenderer>().color = new Color(tile.GetComponent<SpriteRenderer>().color.r,
                        tile.GetComponent<SpriteRenderer>().color.g, tile.GetComponent<SpriteRenderer>().color.b, 0.6f);
                    tile.name = "Blocked";
                    tile.transform.parent = blockedTileParent.transform;
                }
                else if (Math.Abs(pixelColor.r - grassTileSample.GetComponent<SpriteRenderer>().color.r) < 0.004f &&
                    Math.Abs(pixelColor.g - grassTileSample.GetComponent<SpriteRenderer>().color.g) < 0.004f &&
                    Math.Abs(pixelColor.b - grassTileSample.GetComponent<SpriteRenderer>().color.b) < 0.004f)
                {
                    GameObject tile = Instantiate(actionTile, new Vector3(x, y, 0), Quaternion.identity);
                    tile.GetComponent<SpriteRenderer>().color = pixelColor;
                    tile.GetComponent<SpriteRenderer>().color = new Color(tile.GetComponent<SpriteRenderer>().color.r,
                        tile.GetComponent<SpriteRenderer>().color.g, tile.GetComponent<SpriteRenderer>().color.b, 0.6f);
                    tile.name = "Grass";
                    tile.transform.parent = grassTileParent.transform;
                }
                else if (Math.Abs(pixelColor.r - flowerTileSample.GetComponent<SpriteRenderer>().color.r) < 0.004f &&
                         Math.Abs(pixelColor.g - flowerTileSample.GetComponent<SpriteRenderer>().color.g) < 0.004f &&
                         Math.Abs(pixelColor.b - flowerTileSample.GetComponent<SpriteRenderer>().color.b) < 0.004f)
                {
                    GameObject tile = Instantiate(actionTile, new Vector3(x, y, 0), Quaternion.identity);
                    tile.GetComponent<SpriteRenderer>().color = pixelColor;
                    tile.GetComponent<SpriteRenderer>().color = new Color(tile.GetComponent<SpriteRenderer>().color.r,
                        tile.GetComponent<SpriteRenderer>().color.g, tile.GetComponent<SpriteRenderer>().color.b, 0.6f);
                    tile.name = "Flower";
                    tile.transform.parent = flowerTileParent.transform;
                }
                else if (blockedFromBelowPositions.Contains(new Vector3(x, y, 0)))
                {
                    GameObject tile = Instantiate(actionTile, new Vector3(x, y, 0), Quaternion.identity);
                    tile.GetComponent<SpriteRenderer>().color = Color.magenta;
                    tile.GetComponent<SpriteRenderer>().color = new Color(tile.GetComponent<SpriteRenderer>().color.r,
                        tile.GetComponent<SpriteRenderer>().color.g, tile.GetComponent<SpriteRenderer>().color.b, 0.6f);
                    tile.name = "Blocked From Below";
                    tile.transform.parent = blockedFromBelowTileParent.transform;
                }
                else if (doorsPositions.Contains(new Vector3(x, y, 0)))
                {
                    GameObject tile = Instantiate(actionTile, new Vector3(x, y, 0), Quaternion.identity);
                    tile.GetComponent<SpriteRenderer>().color = Color.black;
                    tile.GetComponent<SpriteRenderer>().color = new Color(tile.GetComponent<SpriteRenderer>().color.r,
                        tile.GetComponent<SpriteRenderer>().color.g, tile.GetComponent<SpriteRenderer>().color.b, 0.6f);
                    tile.transform.parent = doorsTileParent.transform;
                    tile.name = "Door" + tile.transform.parent.childCount;
                }
                else if (interactablePositions.Contains(new Vector3(x, y, 0)))
                {
                    GameObject tile = Instantiate(actionTile, new Vector3(x, y, 0), Quaternion.identity);
                    tile.GetComponent<SpriteRenderer>().color = Color.yellow;
                    tile.GetComponent<SpriteRenderer>().color = new Color(tile.GetComponent<SpriteRenderer>().color.r,
                        tile.GetComponent<SpriteRenderer>().color.g, tile.GetComponent<SpriteRenderer>().color.b, 0.6f);
                    tile.name = "Interactable";
                    tile.transform.parent = interactableTileParent.transform;
                }
                else if (pixelColor != Color.white)
                {
                    GameObject tile = Instantiate(actionTile, new Vector3(x, y, 0), Quaternion.identity);
                    tile.GetComponent<SpriteRenderer>().color = pixelColor;
                    tile.GetComponent<SpriteRenderer>().color = new Color(tile.GetComponent<SpriteRenderer>().color.r,
                        tile.GetComponent<SpriteRenderer>().color.g, tile.GetComponent<SpriteRenderer>().color.b, 0.3f);
                    tile.transform.parent = actionTileParent.transform;
                }
            }
        }
    }

    private GameObject CreateMap()
    {
        GameObject map = new GameObject("Map");
        map.AddComponent<SpriteRenderer>().sprite = mapSprite;
        map.transform.position = Vector3.zero;
        return map;
    }

    private void CreateParent(GameObject map)
    {
        grassTileParent = new GameObject("Grass Tiles")
        {
            name = "Grass Tiles",
            transform =
            {
                parent = map.transform
            }
        };
        flowerTileParent = new GameObject("Flower Tiles")
        {
            name = "Flower Tiles",
            transform =
            {
                parent = map.transform
            }
        };
        waterTileParent = new GameObject("Water Tiles")
        {
            name = "Water Tiles",
            transform =
            {
                parent = map.transform
            }
        };
        blockedTileParent = new GameObject("Blocked Tiles")
        {
            name = "Blocked Tiles",
            transform =
            {
                parent = map.transform
            }
        };
        blockedFromBelowTileParent = new GameObject("Blocked From Below Tiles")
        {
            name = "Blocked From Below Tiles",
            transform =
            {
                parent = map.transform
            }
        };
        doorsTileParent = new GameObject("Doors Tiles")
        {
            name = "Doors Tiles",
            transform =
            {
                parent = map.transform
            }
        };
        interactableTileParent = new GameObject("Interactable Tiles")
        {
            name = "Interactable Tiles",
            transform =
            {
                parent = map.transform
            }
        };
        actionTileParent = new GameObject("Action Tiles")
        {
            name = "Action Tiles",
            transform =
            {
                parent = map.transform
            }
        };
    }

    [ContextMenu("Generate Blocked Positions")]
    public void GenerateBlockedPositions()
    {
        blockedPositions = new List<Vector3>();
        foreach (var blockedTile in blockedTiles.Where(blockedTile =>
                     !blockedPositions.Contains(blockedTile.transform.position)))
        {
            blockedPositions.Add(blockedTile.transform.position);
        }
    }

    [ContextMenu("Generate Blocked Tile")]
    public void GenerateBlockedTile()
    {
        blockedTiles = new List<GameObject>();
        foreach (var tile in grassTileParent.GetComponentsInChildren<Transform>())
        {
            if (tile.name == "Blocked")
            {
                blockedTiles.Add(tile.gameObject);
            }
        }

        foreach (var tile in flowerTileParent.GetComponentsInChildren<Transform>())
        {
            if (tile.name == "Blocked")
            {
                blockedTiles.Add(tile.gameObject);
            }
        }

        foreach (var tile in waterTileParent.GetComponentsInChildren<Transform>())
        {
            if (tile.name == "Blocked")
            {
                blockedTiles.Add(tile.gameObject);
            }
        }

        foreach (var tile in actionTileParent.GetComponentsInChildren<Transform>())
        {
            if (tile.name == "Blocked")
            {
                blockedTiles.Add(tile.gameObject);
            }
        }

        foreach (var tile in blockedTileParent.GetComponentsInChildren<Transform>())
        {
            if (tile.name == "Blocked")
            {
                blockedTiles.Add(tile.gameObject);
            }
        }
    }

    [ContextMenu("Generate Blocked From Below Positions")]
    public void GenerateBlockedFromBelowPositions()
    {
        blockedFromBelowPositions = new List<Vector3>();
        foreach (var blockedTile in blockedFromBelowTiles.Where(blockedTile =>
                     !blockedFromBelowPositions.Contains(blockedTile.transform.position)))
        {
            blockedFromBelowPositions.Add(blockedTile.transform.position);
        }
    }

    [ContextMenu("Generate Blocked From Below Tile")]
    public void GenerateBlockedFromBelowTile()
    {
        blockedFromBelowTiles = new List<GameObject>();
        foreach (var tile in grassTileParent.GetComponentsInChildren<Transform>())
        {
            if (tile.name == "Blocked From Below")
            {
                blockedFromBelowTiles.Add(tile.gameObject);
            }
        }

        foreach (var tile in flowerTileParent.GetComponentsInChildren<Transform>())
        {
            if (tile.name == "Blocked From Below")
            {
                blockedFromBelowTiles.Add(tile.gameObject);
            }
        }

        foreach (var tile in waterTileParent.GetComponentsInChildren<Transform>())
        {
            if (tile.name == "Blocked From Below")
            {
                blockedFromBelowTiles.Add(tile.gameObject);
            }
        }

        foreach (var tile in actionTileParent.GetComponentsInChildren<Transform>())
        {
            if (tile.name == "Blocked From Below")
            {
                blockedFromBelowTiles.Add(tile.gameObject);
            }
        }

        foreach (var tile in blockedFromBelowTileParent.GetComponentsInChildren<Transform>())
        {
            if (tile.name == "Blocked From Below")
            {
                blockedFromBelowTiles.Add(tile.gameObject);
            }
        }
    }

    [ContextMenu("Generate Doors Positions")]
    public void GenerateDoorsPositions()
    {
        doorsPositions = new List<Vector3>();
        foreach (var doorsTile in doorsTiles.Where(doorsTile =>
                     !doorsPositions.Contains(doorsTile.transform.position)))
        {
            doorsPositions.Add(doorsTile.transform.position);
        }
    }

    [ContextMenu("Generate Doors Tile")]
    public void GenerateDoorsTile()
    {
        doorsTiles = new List<GameObject>();
        foreach (var tile in grassTileParent.GetComponentsInChildren<Transform>())
        {
            if (tile.name.StartsWith("Door"))
            {
                doorsTiles.Add(tile.gameObject);
            }
        }

        foreach (var tile in flowerTileParent.GetComponentsInChildren<Transform>())
        {
            if (tile.name.StartsWith("Door"))
            {
                doorsTiles.Add(tile.gameObject);
            }
        }

        foreach (var tile in waterTileParent.GetComponentsInChildren<Transform>())
        {
            if (tile.name.StartsWith("Door"))
            {
                doorsTiles.Add(tile.gameObject);
            }
        }

        foreach (var tile in actionTileParent.GetComponentsInChildren<Transform>())
        {
            if (tile.name.StartsWith("Door"))
            {
                doorsTiles.Add(tile.gameObject);
            }
        }

        foreach (var tile in doorsTileParent.GetComponentsInChildren<Transform>())
        {
            if (tile.name.StartsWith("Door"))
            {
                doorsTiles.Add(tile.gameObject);
            }
        }
    }

    [ContextMenu("Generate Interactable Positions")]
    public void GenerateInteractablePositions()
    {
        interactablePositions = new List<Vector3>();
        foreach (var interactableTile in interactableTiles.Where(interactableTile =>
                     !interactablePositions.Contains(interactableTile.transform.position)))
        {
            interactablePositions.Add(interactableTile.transform.position);
        }
    }

    [ContextMenu("Generate Interactable Tile")]
    public void GenerateInteractableTile()
    {
        interactableTiles = new List<GameObject>();
        foreach (var tile in grassTileParent.GetComponentsInChildren<Transform>())
        {
            if (tile.name == "Interactable")
            {
                interactableTiles.Add(tile.gameObject);
            }
        }

        foreach (var tile in flowerTileParent.GetComponentsInChildren<Transform>())
        {
            if (tile.name == "Interactable")
            {
                interactableTiles.Add(tile.gameObject);
            }
        }

        foreach (var tile in waterTileParent.GetComponentsInChildren<Transform>())
        {
            if (tile.name == "Interactable")
            {
                interactableTiles.Add(tile.gameObject);
            }
        }

        foreach (var tile in actionTileParent.GetComponentsInChildren<Transform>())
        {
            if (tile.name == "Interactable")
            {
                interactableTiles.Add(tile.gameObject);
            }
        }

        foreach (var tile in interactableTileParent.GetComponentsInChildren<Transform>())
        {
            if (tile.name == "Interactable")
            {
                interactableTiles.Add(tile.gameObject);
            }
        }
    }

    [ContextMenu("PosToDelete")]
    public void PosToDelete()
    {
        foreach (var tile in TilesToDelete)
        {
            if (blockedPositions.Contains(tile.transform.position))
            {
                blockedPositions.Remove(tile.transform.position);
            }

            if (blockedFromBelowPositions.Contains(tile.transform.position))
            {
                blockedFromBelowPositions.Remove(tile.transform.position);
            }

            if (doorsPositions.Contains(tile.transform.position))
            {
                doorsPositions.Remove(tile.transform.position);
            }

            if (interactablePositions.Contains(tile.transform.position))
            {
                interactablePositions.Remove(tile.transform.position);
            }
        }
    }

    [ContextMenu("Generate save file of all positions")]
    public void GenerateSaveFile()
    {
        //make a file for each List of positions
        string path = Application.dataPath + "/Resources/SaveFiles/";
        string fileName = "BlockedPositions.txt";
        string fileName2 = "BlockedFromBelowPositions.txt";
        string fileName3 = "DoorsPositions.txt";
        string fileName4 = "InteractablePositions.txt";

        //write the positions in the file
        System.IO.File.WriteAllLines(path + fileName, blockedPositions.Select(pos => pos.ToString()).ToArray());
        System.IO.File.WriteAllLines(path + fileName2,
            blockedFromBelowPositions.Select(pos => pos.ToString()).ToArray());
        System.IO.File.WriteAllLines(path + fileName3, doorsPositions.Select(pos => pos.ToString()).ToArray());
        System.IO.File.WriteAllLines(path + fileName4, interactablePositions.Select(pos => pos.ToString()).ToArray());
    }

    //load the positions from the file
    [SerializeField] private TextAsset blockedPositionsFile;
    [SerializeField] private TextAsset blockedFromBelowPositionsFile;
    [SerializeField] private TextAsset doorsPositionsFile;
    [SerializeField] private TextAsset interactablePositionsFile;

    [ContextMenu("Load save file of all positions")]
    public void LoadSaveFile()
    {
        //make a file for each List of positions
        string path = Application.dataPath + "/Resources/SaveFiles/";
        string fileName = "BlockedPositions.txt";
        string fileName2 = "BlockedFromBelowPositions.txt";
        string fileName3 = "DoorsPositions.txt";
        string fileName4 = "InteractablePositions.txt";

        //write the positions in the file
        blockedPositions = new List<Vector3>();
        blockedFromBelowPositions = new List<Vector3>();
        doorsPositions = new List<Vector3>();
        interactablePositions = new List<Vector3>();
        foreach (var pos in System.IO.File.ReadAllLines(path + fileName))
        {
            blockedPositions.Add(StringToVector3(pos));
        }

        foreach (var pos in System.IO.File.ReadAllLines(path + fileName2))
        {
            blockedFromBelowPositions.Add(StringToVector3(pos));
        }

        foreach (var pos in System.IO.File.ReadAllLines(path + fileName3))
        {
            doorsPositions.Add(StringToVector3(pos));
        }

        foreach (var pos in System.IO.File.ReadAllLines(path + fileName4))
        {
            interactablePositions.Add(StringToVector3(pos));
        }
    }


    private Vector3 StringToVector3(string pos)
    {
        string[] posSplit = pos.Split(',');
        return new Vector3(float.Parse(posSplit[0].Substring(1)), float.Parse(posSplit[1]),
            float.Parse(posSplit[2].Substring(0, posSplit[2].Length - 1)));
    }
    
    [ContextMenu("UpdateMap")]
    public void UpdateMap()
    {
        DestroyImmediate(actionTileParent);
        DestroyImmediate(interactableTileParent);
        DestroyImmediate(doorsTileParent);
        DestroyImmediate(blockedTileParent);
        DestroyImmediate(blockedFromBelowTileParent);
        DestroyImmediate(waterTileParent);
        DestroyImmediate(flowerTileParent);
        DestroyImmediate(grassTileParent);
        
        GenerateMap();
    }
}