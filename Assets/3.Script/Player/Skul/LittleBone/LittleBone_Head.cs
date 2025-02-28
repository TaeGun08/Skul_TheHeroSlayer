using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LittleBone_Head : MonoBehaviour
{
    private Rigidbody2D rigid;
    private MoveDirection moveDir;
    public MoveDirection MoveDir { get { return moveDir; } set { moveDir = value; } }

    [Header("��Ʋ�� �Ӹ�")]
    [SerializeField] private bool gravityOn;

    private void Awake()
    {
        TryGetComponent(out rigid);
        TryGetComponent(out moveDir);
    }
}
