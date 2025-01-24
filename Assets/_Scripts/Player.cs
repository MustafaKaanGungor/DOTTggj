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
    private void Awake() {
        Instance = this;
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
        Debug.Log("heyooo");
    }

    private void PlayerOnAttack(object sender, EventArgs e)
    {
        //oyuncu saldırıyor
        Debug.Log("heyo");
        bubbleAmount -= bubbleSpendPerAttack;
    }

    void Update()
    {
        Movement();
        DecreaseBubblePerSecond();
        Debug.Log(bubbleAmount);
        
    }

    private void Movement() {
       
        //hareket
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
