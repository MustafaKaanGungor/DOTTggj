using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance {get; private set;}
    #region Bubble System References
    public static  float bubbleAmount;
    [Header("BubbleAmount References")]
    [SerializeField] private float bubbleAmountAtStart;
    [SerializeField] private float bubbleSpendPerAttack;
    [SerializeField] private float bubbleSpendPerSecond;
    private float bubbleTimer;
    #endregion
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
        bubbleAmount = bubbleAmountAtStart;
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
        Debug.Log("heyo");
        bubbleAmount -= bubbleSpendPerAttack;
        //Physics2D.BoxCast(new Vector2(transform.position.x, transform.position.y), new Vector2(5,5), transform.right, )
        
    }

    void Update()
    {
        Movement();
        DecreaseBubblePerSecond();
        Debug.Log(bubbleAmount);
        
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
    //Decrease the bubble amount per second
    private void DecreaseBubblePerSecond()
    {
        bubbleTimer += Time.deltaTime;
        if(bubbleTimer >= 1)
        {
            bubbleAmount -= bubbleSpendPerSecond;   
            bubbleTimer = 0;
        }
    }
}
