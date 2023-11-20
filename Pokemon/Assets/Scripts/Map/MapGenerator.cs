using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] private Sprite mapSprite;
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
        GameObject map = new GameObject("Map");
        map.AddComponent<SpriteRenderer>().sprite = mapSprite;
        map.transform.position = Vector3.zero;
    }
}