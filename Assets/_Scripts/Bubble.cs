using Unity.VisualScripting;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    [SerializeField] private int bubbleValue;

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.tag == "Player")
        {
            Player.bubbleAmount += bubbleValue;
            Destroy(gameObject);
        }
    }
}
