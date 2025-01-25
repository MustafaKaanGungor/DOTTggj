using System;
using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }
    #region Bubble System References
    public static float bubbleAmount;
    [Header("BubbleAmount References")]
    [SerializeField] private float bubbleAmountAtStart;
    [SerializeField] private float bubbleSpendPerAttack;
    [SerializeField] private float bubbleSpendPerSecond;
    [SerializeField] private float dashingPower = 200f;
    [SerializeField] private float dashingTime = 0.2f;
    [SerializeField] private float dashingCooldown = 1f;

    [SerializeField] private TrailRenderer trailRenderer;


    private float bubbleTimer;
    #endregion
    private Rigidbody2D playerRb;
    [SerializeField] private float moveSpeed = 10;
    private int bubbleAirCurrent = 100;
    private int bubbleAirmax = 100;


    private bool isDashing = false;
    private bool canDash = true;


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
        //isDashing = true


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
        if (!isDashing)
        {
            Movement();
        }

        DecreaseBubblePerSecond();

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
        }
    }

    private void Movement() {
        Vector2 inputVector = GameInput.Instance.GetMovementVector();
        playerRb.MovePosition(new Vector2(transform.position.x, transform.position.y) + inputVector * Time.deltaTime * moveSpeed);
    }

    public int GetBubbleAirCurrent() {
        return bubbleAirCurrent;
    }

    public void DamageBubbleAir(int damage) {
        if (!isDashing) {
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
        if (bubbleTimer >= 1)
        {
            bubbleAmount -= bubbleSpendPerSecond;
            bubbleTimer = 0;
        }
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;

        StartCoroutine(CameraShake.Instance.Shake(0.2f, 0.2f));

        // Başlangıçta dash yönünü belirle
        Vector2 dashDirection = GameInput.Instance.GetMovementVector().normalized;

        if (dashDirection == Vector2.zero)
        {
            dashDirection = new Vector2(transform.localScale.x, 0).normalized;
        }

        float dashTimer = 0f;
        trailRenderer.emitting = true;

        while (dashTimer < dashingTime)
        {
            dashTimer += Time.deltaTime;

            // Dash sırasında input'u kontrol et
            Vector2 inputDirection = GameInput.Instance.GetMovementVector().normalized;

            // Dash yönü ve input'u birleştir
            Vector2 combinedDirection = dashDirection + inputDirection;

            playerRb.linearVelocity = combinedDirection.normalized * dashingPower;

            yield return null;
        }

        // Dash bittiğinde
        trailRenderer.emitting = false;
        playerRb.linearVelocity = Vector2.zero; // Dash durduğunda hız sıfırlanır
        isDashing = false;

        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }

}
