using System;
using UnityEngine;

public class PlayerAim : MonoBehaviour
{
    [SerializeField] private LayerMask layerMask;
    private void Start() {

    }

    void Update()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.right = (mousePos - (Vector2)transform.position).normalized;
        if(GameInput.Instance.IsAttacking()) {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, mousePos - (Vector2)transform.position, 10, layerMask);
        }
    }
}
