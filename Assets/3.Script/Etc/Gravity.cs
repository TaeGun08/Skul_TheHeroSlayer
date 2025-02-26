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
    public float Velocity { get { return velocity; } set { velocity = value; } }

    [SerializeField] private bool isGround;
    public bool IsGround => isGround;

    private void Awake()
    {
        TryGetComponent(out rigid);
        TryGetComponent(out boxColl);
    }

    private void FixedUpdate()
    {
        gravityVelocity();
    }

    private void gravityVelocity()
    {
        if (!groundCheck())
        {
            velocity = gravity * Time.deltaTime;

            rigid.velocity -= new Vector2(0f, velocity);

            if (rigid.velocity.y <= -gravity)
            {
                rigid.velocity = new Vector2(rigid.velocity.x, -gravity);
            }
        }
        else
        {
            rigid.velocity = new Vector2(rigid.velocity.x, 0f);
        }
    }

    private bool groundCheck()
    {
        isGround = false;

        if (rigid.velocity.y > 0f)
        {
            isGround = false;
        }

        if (Physics2D.BoxCast(boxColl.bounds.center, boxColl.bounds.size, 0.0f, Vector2.down, 0.1f, LayerMask.GetMask("Ground"))) 
        {
            isGround = true;
        }

        return isGround;
    }
}
