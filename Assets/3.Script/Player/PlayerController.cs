using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private GameManager gameManager;

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
    [Space]
    [SerializeField] private int formChange;

    private void Start()
    {
        gameManager = GameManager.Instance;

        TryGetComponent(out input);
        TryGetComponent(out anim);
    }

    private void Update()
    {
        if (gameManager.IsGamePause)
        {
            return;
        }

        input.InputMoveLeftOrRight(speed);
        input.InputJump(jumpForce, jumpCount);
        input.InputDash(dashForce, dashCoolTime, dashCount, gravityDash);
        input.InputFootholdFall();
        input.InputAttack(ref formChange);
        anim.PlayerAnim();
    }
}
