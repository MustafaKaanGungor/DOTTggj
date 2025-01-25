using System;
using UnityEngine;

public class PlayerAim : MonoBehaviour
{
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private float damage;
    [SerializeField] private GameObject fireEffect;
    private float selfDamageTimer = 0;
    private void Start() {

    }

    void Update()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.right = (mousePos - (Vector2)transform.position).normalized;
        if(GameInput.Instance.IsAttacking()) {
            selfDamageTimer += Time.deltaTime;
            if(selfDamageTimer >= 1) {
                Player.Instance.DamageBubbleAir(2);
                selfDamageTimer = 0;
            }
            RaycastHit2D hit = Physics2D.Raycast(transform.position, mousePos - (Vector2)transform.position, 5, layerMask);
            if(hit) {
                if(hit.collider.CompareTag("Boss")) {
                    hit.collider.GetComponent<Boss>().DamageHealth(damage);
                }
            }
            
        }
    }
}
