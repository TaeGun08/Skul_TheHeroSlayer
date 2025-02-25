using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private State state;
    private Animator anim;
    private Gravity gravity;

    private float fallTime;

    private void Awake()
    {
        TryGetComponent(out state);
        TryGetComponent(out anim);
        TryGetComponent(out gravity);
    }

    private void moveAnim()
    {
        anim.SetFloat("StateIndex", state.StateEnum == StateEnum.Walk ? 1 : 0);
    }

    private IEnumerator fallAnim()
    {
        while (true)
        {
            if (fallTime >= 1f)
            {
                fallTime = 0;
                break;
            }
            fallTime += Time.deltaTime * 2f;
            anim.SetFloat("JumpFall", fallTime);
            yield return null;
        }
    }

    public void FallAnim()
    {
        fallTime = 0;
        StopCoroutine("fallAnim");
        StartCoroutine("fallAnim");
    }

    public void jumpAnim()
    {
        anim.SetBool("isGround", gravity.IsGround);
    }

    public void PlayerAnim()
    {
        moveAnim();
        jumpAnim();
    }
}
