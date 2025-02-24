using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private KeyManager keyManager;
    private MoveDirection moveDirection;
    private State state;

    private void Awake()
    {
        TryGetComponent(out state);
        TryGetComponent(out moveDirection);
    }

    private void Start()
    {
        FindAnyObjectByType<KeyManager>().TryGetComponent(out keyManager);
    }

    public bool InputMoveLeftOrRight(float _moveSpeed)
    {
        Vector2 moveDir = moveDirection.MoveDir;
        Vector2 scale = transform.localScale;

        //왼쪽
        if (Input.GetKey(keyManager.Key.KeyCodes[2]))
        {
            state.StateEnum = StateEnum.Walk;
            moveDir.x = -_moveSpeed;
            moveDirection.MoveDir = moveDir;

            if (scale.x > 0)
            {
                scale.x *= -1;
            }

            return true;
        }

        //오른쪽
        if (Input.GetKey(keyManager.Key.KeyCodes[3]))
        {
            state.StateEnum = StateEnum.Walk;
            moveDir.x = _moveSpeed;
            moveDirection.MoveDir = moveDir;

            if (scale.x < 0)
            {
                scale.x *= -1;
            }

            return false;
        }

        return false;
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
