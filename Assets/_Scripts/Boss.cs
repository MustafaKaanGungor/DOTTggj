using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField] private LayerMask playerMask;
    [SerializeField] private Transform tentacle;
    [SerializeField] private float attackDelay = 1.5f;
    [SerializeField] private float attackWeight = 3f;
    [SerializeField] private int attackDamage;
    [SerializeField] private GameObject attackEffect;


    [SerializeField] private GameObject tentacleParent;
    [SerializeField] private GameObject tentaclePrefab;
    [SerializeField] private GameObject tentacleLongParent;
    [SerializeField] private GameObject[] tentacleLong;
    [SerializeField] private Collider2D spawnAreaCollider;

    private List<GameObject> tentaclePool = new List<GameObject>();

    void Start()
    {
        TentaclePooling(20);
    }

    void Update()
    {
        // Test etmek için eklenmiþtir
        if (Input.GetKeyDown(KeyCode.Space))
        {
            BottomUpTentacleAttack();
            RotateTentacleAttack();
        }
    }

    public void TentaclePooling(int tentacleCount)
    {
        int spawnedTenctacleCount = 0;
        while (spawnedTenctacleCount != tentacleCount)
        {
            var tentacle = Instantiate(tentaclePrefab, transform.position, Quaternion.identity);
            tentacle.transform.parent = tentacleParent.transform;
            tentacle.SetActive(false);
            tentaclePool.Add(tentacle);
            spawnedTenctacleCount++;
        }
    }

    public List<Vector2> GenerateRandomPositions(int count, float minDistance)
    {
        List<Vector2> positions = new List<Vector2>();
        int maxAttempts = 1000;

        while (positions.Count < count)
        {
            if (maxAttempts-- <= 0)
            {
                Debug.LogError("Maksimum deneme ulaþýldý");
                break;
            }

            Vector2 pos = new Vector2(Random.Range(-12, 12), Random.Range(-8, 8));
            Vector2 candidate = spawnAreaCollider.ClosestPoint(pos);

            if (pos != candidate) continue;
            if (candidate == tentacleParent.GetComponent<Collider2D>().ClosestPoint(candidate)) continue;
            if (positions.Exists(pos => Vector2.Distance(pos, candidate) < minDistance)) continue;

            positions.Add(candidate);
        }

        return positions;
    }

    private void BottomUpTentacleAttack()
    {
        var randomPositions = GenerateRandomPositions(20, 2.5f);

        for (int i = 0; i < randomPositions.Count; i++)
        {
            tentaclePool[i].transform.position = randomPositions[i];
        }

        foreach (var item in tentaclePool)
        {
            item.SetActive(true);
        }
    }

    private void RotateTentacleAttack()
    {
        foreach (var item in tentacleLong)
        {
            item.SetActive(true);
        }
    }

}
