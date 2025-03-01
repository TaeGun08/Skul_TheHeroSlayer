using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Rigidbody2D rigid;
    private State state;
    private Animator anim;
    private Gravity gravity;

    private float fallTime;
    public float FallTime { set { fallTime = value; } }

    private void Awake()
    {
        TryGetComponent(out rigid);
        TryGetComponent(out state);
        TryGetComponent(out anim);
        TryGetComponent(out gravity);
    }

    private void moveAnim()
    {
        anim.SetFloat("StateIndex", state.StateEnum == StateEnum.Walk ? 1 : 0);
    }

    private void fallAnim()
    {
        if (rigid.velocity.y < 0 && !gravity.IsGround)
        {
            fallTime += Time.deltaTime * 2f;
            state.SetSateEnum(StateEnum.Fall, gravity.IsGround);
            if (fallTime >= 1f)
            {
                fallTime = 1f;
            }
        }

        if (gravity.IsGround)
        {
            fallTime = 0;
        }

        anim.SetFloat("JumpFall", fallTime);
    }

    private void jumpAnim()
    {
        anim.SetBool("isGround", gravity.IsGround);
        anim.SetBool("isJump", state.StateEnum == StateEnum.Jump ? true : false);
    }

    private void dashAnim()
    {
        anim.SetBool("isDash", state.StateEnum == StateEnum.Dash ? true : false);
    }

    public void PlayerAnim()
    {
        moveAnim();
        fallAnim();
        jumpAnim();
        dashAnim();
    }
}
