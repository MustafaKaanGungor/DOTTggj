using UnityEngine;

public class Wave : MonoBehaviour
{
    //collider belirtilen  saniye sonra triggerý enable oluyooluyor
    [SerializeField] private float setCollider = 1f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Palayer has take damage!!!");
            //player.takedamage
            Invoke(nameof(EnableTrigger), setCollider);
        }
    }
    private void EnableTrigger()
    {
        GetComponentInChildren<Collider2D>().isTrigger = true;
    }
}
