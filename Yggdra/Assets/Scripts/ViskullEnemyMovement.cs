using UnityEditor.Tilemaps;
using UnityEngine;

public class ViskullEnemyMovement : MonoBehaviour
{
    [SerializeField] float movementSpeed;
    Rigidbody2D viskullRigidbody;
    Transform viskullTranform;
    BoxCollider2D viskullBoxCollider;
    Animator viskullAnimator;
    [SerializeField] PlayerMovement playerScript;

    void Start()
    {
        viskullRigidbody = GetComponent<Rigidbody2D>();
        viskullTranform = GetComponent<Transform>();
        viskullAnimator = GetComponent<Animator>();
        viskullBoxCollider = GetComponent<BoxCollider2D>();
    }

    void Update()
    {       
       Move();
       StopMovement();
    }

    void FlipEnemySprite()
    {
        viskullTranform.localScale = new Vector2 (Mathf.Sign(-movementSpeed), 1f);
    }

    void Move ()
    {

        viskullRigidbody.linearVelocityX = movementSpeed;

        bool enemyHasHorizontalSpeed = Mathf.Abs(viskullRigidbody.linearVelocityX) > Mathf.Epsilon;
        FlipEnemySprite();
        viskullAnimator.SetBool("isWalking", enemyHasHorizontalSpeed);       
    }

    void StopMovement()
    {
        if (!playerScript.GetPlayerDeathState())
        {
            movementSpeed = Mathf.Epsilon;
        }
    }

    void OnTriggerExit2D(Collider2D other) 
    {
        movementSpeed = -movementSpeed;
        FlipEnemySprite();  
    }
}
