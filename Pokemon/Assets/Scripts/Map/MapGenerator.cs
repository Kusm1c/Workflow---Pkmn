using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] private List<Sprite> mapSprites;
    [SerializeField] private GameObject player;
    [SerializeField] private Vector3 playerSpawnPosition;

    private void Awake()
    {
        GenerateMap();
        SpawnPlayer();
    }

    private void SpawnPlayer()
    {
        player = Instantiate(player, playerSpawnPosition, Quaternion.identity);
    }

    private void GenerateMap()
    {
        for (int i = 0; i < mapSprites.Count; i++)
        {
            GameObject mapTile = new GameObject("MapTile_" + i);
            mapTile.transform.SetParent(transform);
            mapTile.transform.position = new Vector3(i % 10, i / 10, 0);
            mapTile.AddComponent<SpriteRenderer>().sprite = mapSprites[i];
        }
    }
}