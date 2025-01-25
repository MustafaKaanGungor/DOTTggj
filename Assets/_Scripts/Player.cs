using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    public event EventHandler OnPlayerDeath;

    #region Bubble System References
    [Header("BubbleAmount References")]
    public float bubbleAirCurrent;
    public float bubbleAirMax = 100;
    [SerializeField] private float bubbleSpendPerAttack;
    [SerializeField] private float bubbleSpendPerSecond;
    private float bubbleTimer;
    private CircleCollider2D playerCollider;
    #endregion
    [SerializeField] private float dashingPower = 200f;
    [SerializeField] private float dashingTime = 0.2f;
    [SerializeField] private float dashingCooldown = 1f;
    [SerializeField] private float moveSpeed = 10;
    private Rigidbody2D playerRb;
    [SerializeField] private PlayerVisuals playerVisuals;

    [SerializeField] public Vector2 playerPos;
    private bool isDashing = false;
    private bool canDash = true;
    private bool isDead = false;


    private void Awake() {
        Instance = this;
        playerRb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        bubbleAirCurrent = bubbleAirMax;
        playerCollider = GetComponent<CircleCollider2D>();
        GameInput.Instance.OnAttack += PlayerOnAttack;
        GameInput.Instance.OnDash += PlayerOnDash;
    }

    private void PlayerOnDash(object sender, EventArgs e)
    {
        if(canDash && !isDead) {
            StartCoroutine(Dash());
        }
    }

    void Update()
    {
        DecreaseBubblePerSecond();
        SetPlayerHitBox();
    }
    void FixedUpdate()
    {
        if (!isDashing && !isDead)
        {
            Movement();
        }
    }

    private void Movement() {
        Vector2 inputVector = GameInput.Instance.GetMovementVector().normalized;
        playerRb.MovePosition(new Vector2(transform.position.x, transform.position.y) + inputVector * Time.deltaTime * moveSpeed);
    }

    public float GetBubbleAirCurrent() {
        return bubbleAirCurrent;
    }

    public void DamageBubbleAir(float damage) {
        if (!isDashing) {
            bubbleAirCurrent -= damage;
            bubbleAirCurrent = Mathf.Clamp(bubbleAirCurrent, 0, bubbleAirMax);
            if(bubbleAirCurrent <= 0) {
                isDead = true;
                OnPlayerDeath?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public void HealBubbleAir(float healAmount) {
        bubbleAirCurrent += healAmount;
        bubbleAirCurrent = Mathf.Clamp(bubbleAirCurrent, 0, bubbleAirMax);
    }

    private void DecreaseBubblePerSecond()
    {
        bubbleTimer += Time.deltaTime;
        if (bubbleTimer >= 1)
        {
            DamageBubbleAir(bubbleSpendPerSecond);
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
        playerVisuals.StartTrail();

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
        playerVisuals.StopTrail();
        playerRb.linearVelocity = Vector2.zero; // Dash durduğunda hız sıfırlanır
        isDashing = false;

        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }
    private void SetPlayerHitBox()
    {
        playerCollider.radius = bubbleAirCurrent / 50;
    }


}
