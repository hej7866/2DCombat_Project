using CustomUtility;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 7.5f;

    Rigidbody2D rigid;
    Animator anim;

    // Todo : 코드 테스트를 위해서 인스팩터에 노출시킴, 확인 후 지울 것
    [SerializeField] private bool isFacingRight = false; 

    private void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        Move();
        Jump();
    }

    

    private void Move()
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

    private void Jump()
    {
        
    }
}