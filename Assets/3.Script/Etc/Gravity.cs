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
    public bool IsGround { get { return isGround; } set { isGround = value; } }

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
            velocity = gravity * Time.deltaTime * 3f;

            rigid.velocity -= new Vector2(0f, velocity);

            if (rigid.velocity.y <= -gravity)
            {
                rigid.velocity = new Vector2(rigid.velocity.x, -gravity);
            }
        }
    }

    private bool groundCheck()
    {
        isGround = false;

        if (rigid.velocity.y > 0f)
        {
            isGround = false;
        }

        if (Physics2D.BoxCast(new Vector2(boxColl.bounds.center.x, boxColl.bounds.min.y), 
            new Vector2(boxColl.bounds.size.x, boxColl.bounds.size.y * 0.1f), 0.0f, Vector2.down, 0.1f, 
            LayerMask.GetMask("Ground"))) 
        {
            isGround = true;
        }

        if (Physics2D.BoxCast(new Vector2(boxColl.bounds.center.x, boxColl.bounds.min.y),
            new Vector2(boxColl.bounds.size.x, boxColl.bounds.size.y * 0.1f), 0.0f, Vector2.down, 0.1f,
            LayerMask.GetMask("Footboard")))
        {
            isGround = true;
        }

        return isGround;
    }
}
