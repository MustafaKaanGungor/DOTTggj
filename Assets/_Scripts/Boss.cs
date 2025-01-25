using System.Collections;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField] private LayerMask playerMask;
    [SerializeField] private Transform tentacle;
    [SerializeField] private  float attackDelay = 1.5f;
    [SerializeField] private float attackWidht = 1f;
    [SerializeField] private float attackHeight = 4f;
    [SerializeField] private int attackDamage;
    [SerializeField] private GameObject attackEffect;

    
    void Start()
    {
        
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
            if(hitObject.CompareTag("Player")) 
            {
                //player take damage metodu
                Debug.Log("Attacked player");
            }
        }

    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(tentacle.position, new Vector2(attackWidht, attackHeight));
    }
}
