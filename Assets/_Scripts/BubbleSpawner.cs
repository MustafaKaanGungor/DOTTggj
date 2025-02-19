using UnityEngine;

public class BubbleSpawner : MonoBehaviour
{
    [SerializeField] private GameObject bubblePrefab;
    [SerializeField] private float spawnDuration;
    private float timeUntilSpawn;

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
        GameObject spawnedProjectile = Instantiate(bubblePrefab, Boss.Instance.GenerateRandomPositions(1, 0)[0], Quaternion.identity);
        Rigidbody2D rigidbody = spawnedProjectile.GetComponent<Rigidbody2D>();
        BoxCollider2D boxCollider = spawnedProjectile.GetComponent<BoxCollider2D>();
    }
}
