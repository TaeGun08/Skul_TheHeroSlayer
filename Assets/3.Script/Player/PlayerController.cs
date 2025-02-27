using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerInput input;
    private PlayerAnimation anim;

    [Header("Skul")]
    [SerializeField] private int skulIndex;
    public int SkulIndex => skulIndex;
    [Space]
    [SerializeField] private float speed;
    [Space]
    [SerializeField] private float jumpForce;
    [SerializeField] private int jumpCount;
    [Space]
    [SerializeField] private float dashForce;
    [SerializeField] private float dashCoolTime;
    [SerializeField] private int dashCount;
    [SerializeField] private bool gravityDash;

    private void Start()
    {
        TryGetComponent(out input);
        TryGetComponent(out anim);
    }

    private void Update()
    {
        input.InputMoveLeftOrRight(speed);
        input.InputJump(jumpForce, jumpCount);
        input.InputDash(dashForce, dashCoolTime, dashCount, gravityDash);
        input.InputAttack();
        anim.PlayerAnim();
    }
}
