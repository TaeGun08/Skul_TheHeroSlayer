using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour
{
    private Rigidbody2D rigid;
    private BoxCollider2D boxColl;

    [Header("ม฿ทย")]
    [SerializeField] private float gravity;
    private float velocity;

    [SerializeField] private bool isGround;
    public bool IsGround => isGround;
    

    private void Awake()
    {
        TryGetComponent(out rigid);
        TryGetComponent(out boxColl);
    }

    private void FixedUpdate()
    {
        if (!groundCheck())
        {
            velocity -= gravity * Time.deltaTime;
        }
        else
        {
            velocity = 0;
        }

        rigid.velocity = new Vector2(rigid.velocity.x, velocity);
    }

    private bool groundCheck()
    {
        isGround = false;

        if (Physics2D.BoxCast(boxColl.bounds.center, boxColl.bounds.size, 0.0f, Vector2.down, 0.1f, LayerMask.GetMask("Ground")))
        {
            isGround = true;
        }

        return isGround;
    }
}
