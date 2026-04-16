using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class BossRoomTrigger : MonoBehaviour
{
    public GameObject[] bossPrefabs;

    private bool hasSpawned = false;
    private RectInt roomData;

    public void Setup(RectInt data)
    {
        roomData = data;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!hasSpawned && other.gameObject.CompareTag("Player"))
        {
            SpawnBoss();
            hasSpawned = true;
        }
    }

    private void SpawnBoss()
    {
        Vector3 spawnPos = new Vector3(roomData.center.x, roomData.center.y, 0);

        GameObject Boss = Instantiate(bossPrefabs[Random.Range(0, bossPrefabs.Length)], spawnPos, Quaternion.identity);

        Boss.transform.parent = this.transform;
    }
}
