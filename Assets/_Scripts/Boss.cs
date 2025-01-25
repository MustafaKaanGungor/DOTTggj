using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField] private LayerMask playerMask;
    [SerializeField] private Transform tentacle;
    [SerializeField] private float attackDelay = 1.5f;
    [SerializeField] private float attackWidht = 1f;
    [SerializeField] private float attackHeight = 4f;
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

    private void LineAttack(Vector2 targetPos)
    {
        StartCoroutine(TentacleLineAttack(targetPos));
    }

    private IEnumerator TentacleLineAttack(Vector2 targetPos)
    {

        yield return new WaitForSeconds(attackDelay);
        Vector2 boxSize = new Vector2(attackWidht, attackHeight);
        Collider2D[] hitObjects = Physics2D.OverlapBoxAll(tentacle.transform.position, boxSize, 0f, playerMask);
        foreach (Collider2D hitObject in hitObjects)
        {
            if (hitObject.CompareTag("Player"))
            {
                //player take damage metodu
                Debug.Log("Attacked player");
            }
        }
    }

    void Update()
    {
        // Test etmek için eklenmiþtir
        if (Input.GetKeyDown(KeyCode.Space))
        {
            RotateTentacleAttack();
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            BottomUpTentacleAttack();
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
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(tentacle.position, new Vector2(attackWidht, attackHeight));
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
