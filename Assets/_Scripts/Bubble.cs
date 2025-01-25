using Unity.VisualScripting;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    [SerializeField] private float bubbleValue;

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.tag == "Player")
        {
            Player.Instance.HealBubbleAir(bubbleValue);
            Destroy(gameObject);
        }
    }
}
