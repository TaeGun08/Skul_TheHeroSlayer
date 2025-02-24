using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour
{
    private Rigidbody2D rigid;
    private BoxCollider2D boxColl;
    private State state;

    [Header("ม฿ทย")]
    [SerializeField] private float gravity;
    private float velocity;

    [SerializeField] private bool isGround;
    public bool IsGround => isGround;
    

    private void Awake()
    {
        TryGetComponent(out rigid);
        TryGetComponent(out boxColl);
        TryGetComponent(out state);
    }

    private void FixedUpdate()
    {
        if (!state.StateEnum.Equals(StateEnum.Jump))
        {
            gravityVelocity();
        }
        else
        {
            if (rigid.velocity.y <= 0)
            {
                state.StateEnum = StateEnum.Fall;
            }
        }

        rigid.velocity = new Vector2(rigid.velocity.x, velocity);
    }

    private void gravityVelocity()
    {
        if (!groundCheck())
        {
            velocity -= gravity * Time.deltaTime;
        }
        else
        {
            velocity = 0;
        }
    }

    private bool groundCheck()
    {
        isGround = false;

        if (Physics2D.BoxCast(boxColl.bounds.center, boxColl.bounds.size, 0.0f, Vector2.down, 0.1f, LayerMask.GetMask("Ground")))
        {
            isGround = true;
        }

        if (Physics2D.BoxCast(boxColl.bounds.center, boxColl.bounds.size, 0.0f, Vector2.down, 0.1f, LayerMask.GetMask("Footboard")))
        {
            isGround = true;
        }

        return isGround;
    }
}
