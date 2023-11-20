using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] private Sprite mapSprite;
    [SerializeField] private GameObject player;
    [SerializeField] private Vector3 playerSpawnPosition;
    [SerializeField] private GameObject actionTile;

    private void Awake()
    {
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
        
        Vector2 sizeOfMap = mapSprite.bounds.size;
        Vector2 bottomLeft = new Vector2(-sizeOfMap.x / 2, -sizeOfMap.y / 2);
        Vector2 topRight = new Vector2(sizeOfMap.x / 2, sizeOfMap.y / 2);
        Vector2 actionTileSize = actionTile.GetComponent<SpriteRenderer>().bounds.size;
        for (float x = bottomLeft.x + actionTileSize.x / 2; x < topRight.x; x += actionTileSize.x)
        {
            for (float y = bottomLeft.y + actionTileSize.y / 2; y < topRight.y; y += actionTileSize.y)
            {
                GameObject tile = Instantiate(actionTile, new Vector3(x, y, 0), Quaternion.identity);
                tile.transform.parent = map.transform;
            }
        }
    }
}