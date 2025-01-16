using CustomUtility;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 7.5f;
    [SerializeField] private float jumpPower = 7f;
    [SerializeField] private float jumpTime = 0.5f;
    [SerializeField] private float extraHeight = 0.25f;
    [SerializeField] private LayerMask whatIsGround;

    Rigidbody2D rigid;
    Animator anim;
    Collider2D coll;

    // Todo : 코드 테스트를 위해서 인스팩터에 노출시킴, 확인 후 지울 것
    [SerializeField] private bool isFacingRight = false; 
    private bool isJumping;
    private bool isFalling;
    private RaycastHit2D groundHit;
    private float jumpTimeCounter;

    private Coroutine resetTrigger;

    private void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();

        jumpTimeCounter = jumpTime;
    }

    private void Update()
    {
        PlayerMove();
        PlayerJump();
    }

    

    private void PlayerMove()
    {
        // 입력 Control
        float horizontal = InputUser.Instance.moveInput.x;

        // 입력값이 있었는가?
        if(Math.Abs(horizontal) > 0) // 입력값이 있을 
        {
            anim.SetBool("isWalking", true);
            TurnCheck();
        }
        else
        {
            anim.SetBool("isWalking", false);
        }

        // rigidbody2D
        rigid.velocity = new Vector2( horizontal * moveSpeed, rigid.velocity.y);
    }

    private void PlayerJump()
    {
        // 소
        if(InputUser.Instance.control.Jumping.Jump.WasPerformedThisFrame() && IsGrounded())
        {
            isJumping = true;
            jumpTimeCounter = jumpTime;
            rigid.velocity = new Vector2(rigid.velocity.x, jumpPower);
        }

        // 대
        if(InputUser.Instance.control.Jumping.Jump.WasPressedThisFrame())
        {
            if(jumpTimeCounter > 0 && isJumping)
            {
                rigid.velocity = new Vector2(rigid.velocity.x, jumpPower);
                jumpTimeCounter -= Time.deltaTime;
            }
            else if(jumpTimeCounter == 0)
            {
                isJumping = false;
                isFalling = true;
            } 
            else
            {
                isJumping = true;
            }
        }

        if(InputUser.Instance.control.Jumping.Jump.WasReleasedThisFrame())
        {
            isJumping = false;
            isFalling = true;
        }
        DrawGroundedCheck();
    }

    private bool CheckForLand()
    {
        if(isFalling)
        {
            if(IsGrounded())
            {
                isFalling = false;

                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }


    private bool IsGrounded()
    {
        groundHit = Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, extraHeight, whatIsGround);

        if(groundHit.collider != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    #region Flip

    private void TurnCheck()
    {
        if(InputUser.Instance.moveInput.x > 0 && !isFacingRight) // 정면
        {
            Turn();
        }
        else if(InputUser.Instance.moveInput.x < 0 && isFacingRight) // 후면
        {
            Turn();
        }
    }

    private void Turn()
    {
        if(isFacingRight) // 정면 -> 후면
        {
            Vector3 rotator = new Vector3(transform.rotation.x, 0, transform.rotation.z);
            transform.rotation = Quaternion.Euler(rotator);
            isFacingRight = !isFacingRight;
        }
        else // 후면 -> 정면
        {
            Vector3 rotator = new Vector3(transform.rotation.x, 180, transform.rotation.z);
            transform.rotation = Quaternion.Euler(rotator);
            isFacingRight = !isFacingRight;
        }
    }

    #endregion

    #region Debug Funtion
    private void DrawGroundedCheck()
    {
        Color rayColor;

        if(IsGrounded())
            {
                rayColor  = Color.green;
            }
            else
            {
                rayColor  = Color.red;
            }

        Debug.DrawRay(coll.bounds.center +
            new Vector3(coll.bounds.extents.x,0), Vector2.down * (coll.bounds.extents.y + extraHeight), rayColor);

        Debug.DrawRay(coll.bounds.center -
            new Vector3(coll.bounds.extents.x,0), Vector2.down * (coll.bounds.extents.y + extraHeight), rayColor);

        Debug.DrawRay(coll.bounds.center - new Vector3(coll.bounds.extents.x, coll.bounds.extents.y + extraHeight), Vector2.right * (coll.bounds.extents.x * 2), rayColor);
    }
    #endregion
   
}