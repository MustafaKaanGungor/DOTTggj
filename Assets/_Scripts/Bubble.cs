using UnityEngine;

public class Bubble : MonoBehaviour
{
    [SerializeField] private float bubbleValue;
    [SerializeField] private int bubbleTTL = 5;
    private float currentTime = 0;

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            Player.Instance.HealBubbleAir(bubbleValue);
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        DestroyBubble();
    }

    void DestroyBubble()
    {
        currentTime += Time.deltaTime;
        if (currentTime >= bubbleTTL)
        {
            Destroy(gameObject);
            currentTime = 0f;
        }
    }
}
