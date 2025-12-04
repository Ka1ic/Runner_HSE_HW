using System;
using System.Collections.Generic;
using System.Resources;
using System.Security.Cryptography;
using UnityEngine;
using Random = UnityEngine.Random;

public class GroundGenerator : MonoBehaviour
{
    public GameObject startGroundPrefab;
    public GameObject[] groundPrefabs;
    private List<GameObject> createdGrounds = new List<GameObject>();
    [SerializeField] private float spawnPos = 0;
    [SerializeField] private float groundLength = 100;
    [SerializeField] private Transform player;
    [SerializeField] private int startGroundCount = 3;
    [SerializeField] private int currentGroundId = 0;
    
    private void Start()
    {
        SpawnGround(isStart: true);
        for (int i = 0; i < startGroundCount; i++)
        {
            SpawnGround();
            currentGroundId = (currentGroundId + 1) % groundPrefabs.Length;
        }
    }

    private void Update()
    {
        // будем генерировать землю по мере передвижения игрока
        if (player.position.z > spawnPos - (startGroundCount - 1) * groundLength)
        {
            SpawnGround();
            currentGroundId = (currentGroundId + 1) % groundPrefabs.Length;
            Destroy(createdGrounds[0]); // удаляем те куски, которые уже не видно
            createdGrounds.RemoveAt(0);
        }
    }

    private void SpawnGround(int prefabIndex = -1, bool isStart=false)
    {
        GameObject newGround = null;
        if (isStart)
            newGround = Instantiate(startGroundPrefab, transform.forward * spawnPos, transform.rotation);
        else if (prefabIndex == -1)
            newGround = Instantiate(groundPrefabs[Random.Range(0, groundPrefabs.Length)], transform.forward * spawnPos, transform.rotation);
        else 
            newGround = Instantiate(groundPrefabs[prefabIndex], transform.forward * spawnPos, transform.rotation);

        spawnPos += groundLength;
        createdGrounds.Add(newGround);
    }
}
