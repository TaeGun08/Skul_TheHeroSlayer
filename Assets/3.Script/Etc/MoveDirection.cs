using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveDirection : MonoBehaviour
{
    private Rigidbody2D rigid;

    [Header("¿Ãµø")]
    [SerializeField] private Vector2 moveDir;
    public Vector2 MoveDir { get { return moveDir; } set { moveDir = value; } }

    private void Awake()
    {
        TryGetComponent(out rigid);
    } 

    private void FixedUpdate()
    {
        rigid.velocity = new Vector2(moveDir.x, rigid.velocity.y);
    }
}
