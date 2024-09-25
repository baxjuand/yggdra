using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
   Vector2 moveInput;
    Rigidbody2D playerRigidbody2D;
    Animator playerAnimator;
    float playerGravity;
    CapsuleCollider2D playerCapsuleCollider2D;
    [SerializeField] float playerSpeed;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] float climbSpeed;
   
    void Start()
    {
        playerRigidbody2D = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        playerCapsuleCollider2D = GetComponent<CapsuleCollider2D>();
        playerGravity = playerRigidbody2D.gravityScale;
    }

    void Update()
    {
        Run();
        ClimbLadder();
        FlipSprite();
    }

    void OnMove (InputValue value)
    {
        moveInput = value.Get<Vector2>();
        Debug.Log(moveInput);
    }

    void OnJump (InputValue value)
    {
        
        if (!playerCapsuleCollider2D.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            return;
        }

        if (value.isPressed)
        {
            playerRigidbody2D.linearVelocity += new Vector2 (Mathf.Epsilon, jumpSpeed);
        }




    }

    void Run()
    {
        Vector2 playerVelocity = new Vector2 (moveInput.x * playerSpeed, playerRigidbody2D.linearVelocityY);
        playerRigidbody2D.linearVelocity = playerVelocity;

        bool playerHasHorizontalSpeed = Mathf.Abs(playerRigidbody2D.linearVelocityX) > Mathf.Epsilon;
        playerAnimator.SetBool("isWalking", playerHasHorizontalSpeed);
    }

    void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(playerRigidbody2D.linearVelocityX) > Mathf.Epsilon;

        if (playerHasHorizontalSpeed)
        {
        transform.localScale = new Vector2 (Mathf.Sign(playerRigidbody2D.linearVelocityX), 1f);
        }
    }

    void ClimbLadder()
    {
        if (!playerCapsuleCollider2D.IsTouchingLayers(LayerMask.GetMask("Ladders")))
        { 
            playerRigidbody2D.gravityScale = playerGravity;
            playerAnimator.SetBool("isClimbing", false);
            return;
        }

        playerRigidbody2D.gravityScale = Mathf.Epsilon;

        Vector2 playerVelocityY = new Vector2 (playerRigidbody2D.linearVelocityX, moveInput.y * climbSpeed);
        playerRigidbody2D.linearVelocity = playerVelocityY;

        
        bool playerHasVerticalSpeed = Mathf.Abs(playerRigidbody2D.linearVelocityY) > Mathf.Epsilon;
        playerAnimator.SetBool("isClimbing", playerHasVerticalSpeed);
    }
}
