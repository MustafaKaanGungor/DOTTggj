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

    [Header("Prefabs")]
    [SerializeField] private GameObject tentacleParent;
    [SerializeField] private GameObject tentaclePrefab;
    [SerializeField] private GameObject tentacleLongParent;
    [SerializeField] private GameObject[] tentacleLong;
    [SerializeField] private Collider2D spawnAreaCollider;

    private List<GameObject> tentaclePool = new List<GameObject>();

    [SerializeField] public GameObject playerPos;

    [SerializeField] private float bossHealthCurrent;
    [SerializeField] private float bossHealthMax;
    
    void Start()
    {
        TentaclePooling(20);
        LineAttack(new Vector2(playerPos.transform.position.x,playerPos.transform.position.y));
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
        // Test etmek i�in eklenmi�tir
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
            var tentacle = Instantiate(tentaclePrefab, tentacleParent.transform);
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
                Debug.LogError("Maksimum deneme ula��ld�");
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
        List<Vector2> randomPositions = GenerateRandomPositions(20, 2.5f);

        for (int i = 0; i < randomPositions.Count; i++)
        {
            tentaclePool[i].transform.position = randomPositions[i];
            tentaclePool[i].SetActive(true);
        }
    }

    private void RotateTentacleAttack()
    {
        foreach (var item in tentacleLong)
        {
            item.SetActive(true);
        }
    }

    public void DamageHealth(float damage) {
        bossHealthCurrent -= damage;
        bossHealthCurrent = Mathf.Clamp(bossHealthCurrent, 0, bossHealthMax);
        if(bossHealthCurrent <= 0) {
            //dead
        }
    }

}
