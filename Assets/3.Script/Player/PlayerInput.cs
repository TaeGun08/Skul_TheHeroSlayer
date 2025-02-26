using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private KeyManager keyManager;
    private MoveDirection moveDirection;
    private State state;
    private Gravity gravity;
    private PlayerAnimation anim;

    private Rigidbody2D rigid;
    private int jumpCount;

    private bool isDash;
    private int dashCount;
    private float dashCoolTimer;
    private bool isDashing;

    private void Awake()
    {
        TryGetComponent(out state);
        TryGetComponent(out moveDirection);
        TryGetComponent(out rigid);
        TryGetComponent(out gravity);
        TryGetComponent(out anim);
    }

    private void Start()
    {
        FindAnyObjectByType<KeyManager>().TryGetComponent(out keyManager);
    }

    private IEnumerator jumpingCoroutine()
    {
        yield return new WaitForSeconds(0.15f);
        gravity.enabled = true;
        state.StateEnum = StateEnum.Fall;
        yield return null;
    }

    private IEnumerator dashingCoroutine()
    {
        yield return new WaitForSeconds(0.1f);
        moveDirection.enabled = true;
        yield return null;
        moveDirection.enabled = false;
        rigid.velocity = Vector2.zero;
    }

    public void InputMoveLeftOrRight(float _moveSpeed)
    {
        Vector2 moveDir = moveDirection.MoveDir;
        Vector2 scale = transform.localScale;

        if (gravity.IsGround)
        {
            state.StateEnum = StateEnum.Idle;
        }
        moveDir.x = 0;

        if (Input.GetKey(keyManager.Key.KeyCodes[2])
        && Input.GetKey(keyManager.Key.KeyCodes[3]))
        {
            if (gravity.IsGround)
            {
                state.StateEnum = StateEnum.Idle;
            }
            moveDir.x = 0;
            moveDirection.MoveDir = moveDir;
            return;
        }

        //왼쪽
        if (Input.GetKey(keyManager.Key.KeyCodes[2]))
        {
            if (gravity.IsGround)
            {
                state.StateEnum = StateEnum.Walk;
            }
            moveDir.x = -_moveSpeed;

            if (scale.x > 0)
            {
                scale.x *= -1;
                transform.localScale = scale;
            }
        }
        //오른쪽
        if (Input.GetKey(keyManager.Key.KeyCodes[3]))
        {
            if (gravity.IsGround)
            {
                state.StateEnum = StateEnum.Walk;
            }
            moveDir.x = _moveSpeed;

            if (scale.x < 0)
            {
                scale.x *= -1;
                transform.localScale = scale;
            }
        }

        moveDirection.MoveDir = moveDir;
    }

    public void InputJump(float _jumpForce, int _maxJumpCount)
    {
        if (rigid.velocity.y <= 0 && gravity.IsGround)
        {
            anim.IsJump = false;
            jumpCount = 0;
        }

        if (Input.GetKeyDown(keyManager.Key.KeyCodes[7]) && _maxJumpCount > jumpCount)
        {
            gravity.enabled = false;
            anim.IsJump = true;
            anim.FallTime = 0f;
            gravity.Velocity = 0f;
            isDashing = false;
            rigid.velocity = new Vector2(rigid.velocity.x, _jumpForce);
            jumpCount++;
            StopCoroutine("jumpingCoroutine");
            StartCoroutine("jumpingCoroutine");
        }
    }

    public void InputDash(float _dashForce, float _dashCoolTime, int _maxDashCount)
    {
        if (isDash)
        {
            dashCoolTimer += Time.deltaTime;
            if (dashCoolTimer >= _dashCoolTime)
            {
                isDash = false;
                dashCoolTimer = 0f;
                dashCount = 0;
            }

            if (dashCoolTimer >= 0.3f && isDashing && dashCount > 0)
            {
                moveDirection.enabled = true;
                gravity.enabled = true;
                isDashing = false;
            }
        }

        if (Input.GetKeyDown(keyManager.Key.KeyCodes[8]) 
            && dashCount < _maxDashCount
            && dashCoolTimer < 0.3f)
        {
            gravity.Velocity = 0f;
            anim.FallTime = 0f;
            dashCoolTimer = 0f;
            state.StateEnum = StateEnum.Dash;
            moveDirection.enabled = false;
            gravity.enabled = false;
            isDashing = true;
            isDash = true;
            rigid.velocity = new Vector2(_dashForce * transform.localScale.x, 0f);
            dashCount++;
            StopCoroutine("jumpingCoroutine");
            StopCoroutine("dashingCoroutine");
            StartCoroutine("dashingCoroutine");
        }
    }

    public void InputFootholdFall()
    {
        //아래
        //점프
        if (Input.GetKeyDown(keyManager.Key.KeyCodes[1])
            && Input.GetKeyDown(keyManager.Key.KeyCodes[7]))
        {
            state.StateEnum = StateEnum.Fall;
        }
    }
}
