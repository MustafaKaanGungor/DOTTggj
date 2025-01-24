using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance {get; private set;}
    private Rigidbody2D playerRb;
    [SerializeField] private float moveSpeed = 10;
    private int bubbleAirCurrent = 100;
    private int bubbleAirmax = 100;
    private bool isDashing = false;

    private void Awake() {
        Instance = this;
        playerRb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        GameInput.Instance.OnAttack += PlayerOnAttack;
        GameInput.Instance.OnDash += PlayerOnDash;
    }

    private void PlayerOnDash(object sender, EventArgs e)
    {
        //oyuncu dash atıyor
        
    }

    private void PlayerOnAttack(object sender, EventArgs e)
    {
        //oyuncu saldırıyor
        //Physics2D.BoxCast(new Vector2(transform.position.x, transform.position.y), new Vector2(5,5), transform.right, )
        
    }

    void Update()
    {
        Movement();
        
    }

    private void Movement() {
        Vector2 inputVector = GameInput.Instance.GetMovementVector();
        playerRb.MovePosition(new Vector2(transform.position.x, transform.position.y) + inputVector * Time.deltaTime * moveSpeed);
    }

    public int GetBubbleAirCurrent() {
        return bubbleAirCurrent;
    }

    public void DamageBubbleAir(int damage) {
        if(!isDashing) {
            bubbleAirCurrent -= damage;
            Mathf.Clamp(bubbleAirCurrent, 0, bubbleAirmax);
        }
    }

    public void HealBubbleAir(int healAmount) {
        bubbleAirCurrent += healAmount;
        Mathf.Clamp(bubbleAirCurrent, 0, bubbleAirmax);
    }
}
