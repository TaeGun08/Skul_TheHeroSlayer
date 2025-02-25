using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private KeyManager keyManager;
    private MoveDirection moveDirection;
    private State state;
    private Gravity gravity;
    private PlayerAnimation playerAnim;

    private Rigidbody2D rigid;
    private int jumpCount;
    private float jumpTimer;

    private void Awake()
    {
        TryGetComponent(out state);
        TryGetComponent(out moveDirection);
        TryGetComponent(out rigid);
        TryGetComponent(out gravity);
        TryGetComponent(out playerAnim);
    }

    private void Start()
    {
        FindAnyObjectByType<KeyManager>().TryGetComponent(out keyManager);
    }

    public void InputMoveLeftOrRight(float _moveSpeed)
    {
        Vector2 moveDir = moveDirection.MoveDir;
        Vector2 scale = transform.localScale;

        if (rigid.velocity.y <= 0 && gravity.IsGround)
        {
            state.StateEnum = StateEnum.Idle;
        }
        moveDir.x = 0;

        if (Input.GetKey(keyManager.Key.KeyCodes[2])
        && Input.GetKey(keyManager.Key.KeyCodes[3]))
        {
            if (rigid.velocity.y <= 0 && gravity.IsGround)
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
            if (rigid.velocity.y <= 0 && gravity.IsGround)
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
            if (rigid.velocity.y <= 0 && gravity.IsGround)
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
            jumpCount = 0;
        }

        if (Input.GetKeyDown(keyManager.Key.KeyCodes[7]) && _maxJumpCount > jumpCount)
        {
            state.StateEnum = StateEnum.Jump;
            gravity.Velocity = 0f;
            gravity.enabled = false;
            rigid.velocity = new Vector2(rigid.velocity.y, _jumpForce);
            jumpCount++;
            playerAnim.FallAnim();
            jumpTimer = 0f;
        }

        if (state.StateEnum.Equals(StateEnum.Jump))
        {
            jumpTimer += Time.deltaTime;
            if (jumpTimer >= 0.2f)
            {
                gravity.enabled = true;
                state.StateEnum = StateEnum.Fall;
            }
        }
    }

    public void InputDash()
    {

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
