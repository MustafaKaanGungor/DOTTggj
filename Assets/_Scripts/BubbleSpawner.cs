using UnityEngine;

public class BubbleSpawner : MonoBehaviour
{
    [SerializeField] private GameObject bubblePrefab;
    [SerializeField] private GameObject[] spawnPoints;
    [SerializeField] private float spawnDuration;
    private float timeUntilSpawn;


    // Update is called once per frame
    void Update()
    {
        SpawnLoop();
    }
    void SpawnLoop()
    {
        timeUntilSpawn += Time.deltaTime;
        if (timeUntilSpawn >= spawnDuration)
        {
            Spawn();
            timeUntilSpawn = 0f;
        }
    }
    void Spawn()
    {
        GameObject spawnP = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject spawnedProjectile = Instantiate(bubblePrefab, spawnP.transform.position, Quaternion.identity);
        Rigidbody2D rigidbody = spawnedProjectile.GetComponent<Rigidbody2D>();
        BoxCollider2D boxCollider = spawnedProjectile.GetComponent<BoxCollider2D>();
        
    }
}
