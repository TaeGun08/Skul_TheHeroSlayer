using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private State state;
    private Animator anim;

    private void Awake()
    {
        TryGetComponent(out state);
        TryGetComponent(out anim);
    }

    public void MoveAnim()
    {
        anim.SetFloat("Walk", state.StateEnum == StateEnum.Walk ? 1 : 0);
    }
}
