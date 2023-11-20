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
    
    [SerializeField] private List<Vector3> blockedPositions;
    [SerializeField] private List<GameObject> blockedTiles;

    private GameObject grassTileParent;
    private GameObject flowerTileParent;
    private GameObject waterTileParent;
    private GameObject blockedTileParent;
    
    private GameObject actionTileParent;
    
    public MapGenerator Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        GenerateMap();
        SpawnPlayer();
    }

    private void SpawnPlayer()
    {
        player = Instantiate(player, playerSpawnPosition, Quaternion.identity);
    }

    [ContextMenu("Generate Map")]
    private void GenerateMap()
    {
        GameObject map = new GameObject("Map");
        map.AddComponent<SpriteRenderer>().sprite = mapSprite;
        map.transform.position = Vector3.zero;

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
                        tile.GetComponent<SpriteRenderer>().color.g, tile.GetComponent<SpriteRenderer>().color.b, 0.3f);
                    tile.name = "Deep Water";
                    tile.transform.parent = waterTileParent.transform;
                    continue;
                }
                
                if (Math.Abs(pixelColor.r - grassTileSample.GetComponent<SpriteRenderer>().color.r) < 0.004f &&
                    Math.Abs(pixelColor.g - grassTileSample.GetComponent<SpriteRenderer>().color.g) < 0.004f &&
                    Math.Abs(pixelColor.b - grassTileSample.GetComponent<SpriteRenderer>().color.b) < 0.004f)
                {
                    GameObject tile = Instantiate(actionTile, new Vector3(x, y, 0), Quaternion.identity);
                    tile.GetComponent<SpriteRenderer>().color = pixelColor;
                    tile.GetComponent<SpriteRenderer>().color = new Color(tile.GetComponent<SpriteRenderer>().color.r,
                        tile.GetComponent<SpriteRenderer>().color.g, tile.GetComponent<SpriteRenderer>().color.b, 0.3f);
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
                        tile.GetComponent<SpriteRenderer>().color.g, tile.GetComponent<SpriteRenderer>().color.b, 0.3f);
                    tile.name = "Flower";
                    tile.transform.parent = flowerTileParent.transform;
                }
                else if (blockedPositions.Contains(new Vector3(x, y, 0)))
                {
                    GameObject tile = Instantiate(actionTile, new Vector3(x, y, 0), Quaternion.identity);
                    tile.GetComponent<SpriteRenderer>().color = Color.red;
                    tile.GetComponent<SpriteRenderer>().color = new Color(tile.GetComponent<SpriteRenderer>().color.r,
                        tile.GetComponent<SpriteRenderer>().color.g, tile.GetComponent<SpriteRenderer>().color.b, 0.9f);
                    tile.name = "Blocked";
                    tile.transform.parent = blockedTileParent.transform;
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
}