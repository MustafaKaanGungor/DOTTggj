using System;
using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    public event EventHandler OnPlayerDeath;
    public event EventHandler OnPlayerHealthUpdated;
    public event EventHandler OnPlayerDashUpdated;

    [SerializeField] private Animator animator;

    [Header("Bubble Air")]
    public float bubbleAirCurrent;
    public float bubbleAirMax = 100;
    [SerializeField] private float bubbleSpendPerSecond;
    private float bubbleTimer;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 10;

    [SerializeField] private float dashingPower = 200f;
    [SerializeField] private float dashingDamage = 10f;
    [SerializeField] private float dashingTime = 0.2f;
    [SerializeField] private float effectTime = 0.3f;
    [SerializeField] private float dashingCooldown = 1f;

    [Header("Components")]
    private Rigidbody2D playerRb;
    private CircleCollider2D playerCollider;
    [SerializeField] private PlayerVisuals playerVisuals;
    [SerializeField] private GameObject dashEffect;

    private bool isDashing = false;
    private bool canDash = true;
    private bool isDead = false;


    private void Awake()
    {
        Instance = this;
        playerRb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {

        dashEffect.SetActive(false);
        bubbleAirCurrent = bubbleAirMax;
        playerCollider = GetComponent<CircleCollider2D>();
        GameInput.Instance.OnDash += PlayerOnDash;
    }

    private void PlayerOnDash(object sender, EventArgs e)
    {
        if (canDash && !isDead)
        {
            StartCoroutine(Dash());
            DamageBubbleAir(dashingDamage);
            OnPlayerDashUpdated?.Invoke(new float[] { dashingTime, dashingCooldown }, EventArgs.Empty);
        }
    }

    void Update()
    {
        DecreaseBubblePerSecond();
        //SetPlayerHitBox();
        StayMap();
    }
    void FixedUpdate()
    {
        if (!isDashing && !isDead)
        {
            Movement();
        }
    }

    private void Movement()
    {
        Vector2 inputVector = GameInput.Instance.GetMovementVector().normalized;
        playerRb.MovePosition(new Vector2(transform.position.x, transform.position.y) + inputVector * Time.deltaTime * moveSpeed);
        animator.SetBool("IsWalking", inputVector != Vector2.zero);

        if (inputVector.x < 0)
        {
            animator.gameObject.transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (inputVector.x > 0)
        {
            animator.gameObject.transform.localScale = new Vector3(1, 1, 1);
        }
    }

    public Vector2 GetPlayerPositionVector()
    {
        return (Vector2)transform.position;
    }

    public float GetBubbleAirPercentage()
    {
        return bubbleAirCurrent / bubbleAirMax;
    }

    public void DamageBubbleAir(float damage)
    {
        if (!isDashing)
        {
            bubbleAirCurrent -= damage;
            bubbleAirCurrent = Mathf.Clamp(bubbleAirCurrent, 0, bubbleAirMax);
            OnPlayerHealthUpdated?.Invoke(this, EventArgs.Empty);
            if (damage >= 3)
            {
                PlayerUI.Instance.StartCoroutine(PlayerUI.Instance.ShowFlashEffect(PlayerUI.Instance.DamageFlashEffectImage));
            }
            if (bubbleAirCurrent <= 0)
            {
                isDead = true;
                OnPlayerDeath?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public void HealBubbleAir(float healAmount)
    {
        bubbleAirCurrent += healAmount;
        bubbleAirCurrent = Mathf.Clamp(bubbleAirCurrent, 0, bubbleAirMax);
        OnPlayerHealthUpdated?.Invoke(this, EventArgs.Empty);
        PlayerUI.Instance.StartCoroutine(PlayerUI.Instance.ShowFlashEffect(PlayerUI.Instance.healFlashEffectImage));
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

    public bool IsPlayerDead()
    {
        return isDead;
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;

        StartCoroutine(PlayDashEffect());

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
    IEnumerator PlayDashEffect()
    {
        dashEffect.SetActive(true);

        float effectTimer = 0f;

        while (effectTimer < effectTime)
        {
            effectTimer += Time.deltaTime;

            yield return null;
        }

        dashEffect.SetActive(false);
    }

    private void StayMap()
    {
        Vector3 minBounds = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0));
        Vector3 maxBounds = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, 0));

        Vector3 playerPos = transform.position;

        // Oyuncuyu ekranın sınırları içinde tut
        playerPos.x = Mathf.Clamp(playerPos.x, minBounds.x, maxBounds.x);
        playerPos.y = Mathf.Clamp(playerPos.y, minBounds.y, maxBounds.y);

        transform.position = playerPos;
    }
}
