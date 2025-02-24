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

    public IEnumerator jumpAnim()
    {
        while (state.StateEnum.Equals(StateEnum.Jump))
        {

            yield return null;
        }
    }

    private void fallAnim()
    {

    }

    public void groundAnim()
    {
        anim.SetBool("isGround", gravity.IsGround);
    }

    public void PlayerAnim()
    {
        moveAnim();
        groundAnim();
    }
}
