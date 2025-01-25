using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField] private LayerMask playerMask;
    [SerializeField] private Transform tentacle;
    [SerializeField] private  float attackDelay = 1.5f;
    [SerializeField] private float attackWeight = 3f;
    [SerializeField] private int attackDamage;
    [SerializeField] private GameObject attackEffect;

    
    void Start()
    {
        //
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
