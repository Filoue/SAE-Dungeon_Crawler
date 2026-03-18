using UnityEngine;

public class RoomTrigger : MonoBehaviour
{
    public GameObject[] mobPrefabs;
    public int spawnAmount = 10;
    
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
            SpawnMobs();
            hasSpawned = true;
        }
    }

    private void SpawnMobs()
    {
        for (int i = 0; i < spawnAmount; i++)
        {
            float randomX = Random.Range(roomData.xMin + 1, roomData.xMax - 1);
            float randomY = Random.Range(roomData.yMin + 1, roomData.yMax - 1);
            Vector3 spawnPos = new Vector3(randomX + 0.5f, randomY + 0.5f, 0);
            
            GameObject mob = Instantiate(mobPrefabs[Random.Range(0, mobPrefabs.Length)], spawnPos, Quaternion.identity);
            
            mob.transform.parent = this.transform;
        }
        
        Debug.Log("Player entered room: Mobs spawned!");
    }
}
