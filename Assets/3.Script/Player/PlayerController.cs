using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerInput input;
    private PlayerAnimation anim;

    [Header("플레이어")]
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private int jumpCount;

    private void Start()
    {
        TryGetComponent(out input);
        TryGetComponent(out anim);
    }

    private void Update()
    {
        input.InputMoveLeftOrRight(speed);
        input.InputJump(jumpForce, jumpCount);
        input.InputDash();
        anim.PlayerAnim();
    }
}
