using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private KeyManager keyManager;
    private MoveDirection moveDirection;
    private State state;
    private Gravity gravity;

    private Rigidbody2D rigid;
    private int curJumpCount;

    private void Awake()
    {
        TryGetComponent(out state);
        TryGetComponent(out moveDirection);
        TryGetComponent(out rigid);
        TryGetComponent(out gravity);
    }

    private void Start()
    {
        FindAnyObjectByType<KeyManager>().TryGetComponent(out keyManager);
    }

    public void InputMoveLeftOrRight(float _moveSpeed)
    {
        Vector2 moveDir = moveDirection.MoveDir;
        Vector2 scale = transform.localScale;

        state.StateEnum = StateEnum.Idle;
        moveDir.x = 0;

        if (Input.GetKey(keyManager.Key.KeyCodes[2])
        && Input.GetKey(keyManager.Key.KeyCodes[3]))
        {
            state.StateEnum = StateEnum.Idle;
            moveDir.x = 0;
            moveDirection.MoveDir = moveDir;
            return;
        }

        //왼쪽
        if (Input.GetKey(keyManager.Key.KeyCodes[2]))
        {
            state.StateEnum = StateEnum.Walk;
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
            state.StateEnum = StateEnum.Walk;
            moveDir.x = _moveSpeed;

            if (scale.x < 0)
            {
                scale.x *= -1;
                transform.localScale = scale;
            }
        }

        moveDirection.MoveDir = moveDir;
    }

    public void InputJump(float _jumpForce, int _jumpCount)
    {
        if (gravity.IsGround)
        {
            curJumpCount = 0;
        }

        if (Input.GetKeyDown(keyManager.Key.KeyCodes[7]) && _jumpCount > curJumpCount)
        {
            state.StateEnum = StateEnum.Jump;
            rigid.AddForce(new Vector2(rigid.velocity.x, _jumpForce), ForceMode2D.Impulse);
            curJumpCount++;
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
