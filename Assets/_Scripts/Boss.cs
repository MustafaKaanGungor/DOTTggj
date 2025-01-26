using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public static Boss Instance;

    [SerializeField] private LayerMask playerMask;
    [SerializeField] private Transform[] tentacles;
    [SerializeField] private float attackDelay = 1.5f;
    [SerializeField] private float attackWidht = 1f;
    [SerializeField] private float attackHeight = 4f;
    [SerializeField] private int buttomUpDamage = 30;
    [SerializeField] private int tentacleDamage = 20;
    [SerializeField] private GameObject attackEffect;
    private bool isAttacking = false;

    [SerializeField] private GameObject tentacleParent;
    [SerializeField] private GameObject tentaclePrefab;
    [SerializeField] private GameObject tentacleLongParent;
    [SerializeField] private GameObject[] tentacleLong;
    [SerializeField] private Collider2D spawnAreaCollider;
    private List<GameObject> tentaclePool = new List<GameObject>();

    [SerializeField] private GameObject[] projectilePrefabs;
    [SerializeField] private GameObject[] spawnPoints;
    [SerializeField] private float waveSpeed;
    [SerializeField] private float spawnDuration;
    private float timeUntilSpawn;

    [SerializeField] public float bossHealthCurrent;
    [SerializeField] public float bossHealthMax;

    private float attackTimer;
    [SerializeField] private float attackInterval;

    private bool isDead = false;

    
    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        TentaclePooling(20);
    }

    void Update()
    {
        attackTimer += Time.deltaTime;
        if(attackTimer >= attackInterval) {
            attackTimer = 0;
            int select = Random.Range(0,2);
            if(select == 0) {
                RotateTentacleAttack();
            } else {
                BottomUpTentacleAttack();
            }
        } 
        
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
        GameObject spawnedProjectile = Instantiate(projectilePrefabs[Random.Range(0, projectilePrefabs.Length)], spawnP.transform.position, Quaternion.identity);
        Rigidbody2D rigidbody = spawnedProjectile.GetComponent<Rigidbody2D>();
        BoxCollider2D boxCollider = spawnedProjectile.GetComponent<BoxCollider2D>();
        rigidbody.linearVelocity = Vector2.down * waveSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isAttacking)
        {
            isAttacking = true;
            LineAttack();
        }
    }
    private void LineAttack()
    {
        StartCoroutine(TentacleLineAttack());
    }

    private IEnumerator TentacleLineAttack()
    {
        Vector2 boxSize = new Vector2(attackWidht, attackHeight);

        // **Önce kırmızı uyarı efekti verelim**
        foreach (Transform tentacle in tentacles)
        {
            SpriteRenderer sr = tentacle.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                sr.color = Color.red; // **Kırmızı uyarı**
                tentacle.gameObject.SetActive(true);
            }
        }

        yield return new WaitForSeconds(attackDelay); // **Uyarı süresi**

        // **Uyarıyı kaldır ve saldırıyı yap**
        foreach (Transform tentacle in tentacles)
        {
            SpriteRenderer sr = tentacle.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                sr.color = new Color(0.4056604f, 0.1215686f, 0.3331002f); // **Eski rengine dön**
              tentacle.gameObject.SetActive(true);
            }

            Collider2D[] hitObjects = Physics2D.OverlapBoxAll(tentacle.position, boxSize, 0, playerMask);
            foreach (Collider2D hitObject in hitObjects)
            {
                if (hitObject.CompareTag("Player"))
                {
                    Debug.Log("Attacked player");
                    Player.Instance.DamageBubbleAir(tentacleDamage);
                }
            }
        }
        isAttacking = false;
        Invoke(nameof(TentacleSetActiveFalse), 0.5f);
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
            isDead = true;
        }
    }
    private void TentacleSetActiveFalse()
    {
        foreach (Transform tentacle in tentacles)
        {
            tentacle.gameObject.SetActive(false);
        }
    }
    public bool IsBossDead() {
        return isDead;
    }

}
