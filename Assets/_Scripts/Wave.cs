using UnityEngine;

public class Wave : MonoBehaviour
{
    //collider belirtilen  saniye sonra trigger� enable oluyooluyor
    [SerializeField] private float setCollider = 1f;
    [SerializeField] private float damage = 20;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Player.Instance.DamageBubbleAir(damage);
            Invoke(nameof(EnableTrigger), setCollider);
        }

        if (collision.gameObject.CompareTag("WaveEnd"))
        {
            Destroy(gameObject.transform.parent.gameObject);
        }
    }

    private void EnableTrigger()
    {
        GetComponentInChildren<Collider2D>().isTrigger = true;
    }
}
