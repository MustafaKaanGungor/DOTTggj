using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance {get; private set;}
    private void Awake() {
        Instance = this;
    }
    void Start()
    {
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
    }

    void Update()
    {
        Movement();
        
    }

    private void Movement() {
        Debug.Log(GameInput.Instance.GetMovementVector());
        //hareket
    }
}
