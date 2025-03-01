using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LittleBone_Head : MonoBehaviour
{
    private Rigidbody2D rigid;
    private MoveDirection moveDir;
    public MoveDirection MoveDir { get { return moveDir; } set { moveDir = value; } }
    private MoveRotate moveRotate;
    private CircleCollider2D circleColl;

    [Header("리틀본 머리")]
    [SerializeField] private float moveTime;
    private float moveTimer;
    private bool timerOn;
    private float maintenanceTimer;

    private void Awake()
    {
        TryGetComponent(out rigid);
        TryGetComponent(out moveDir);
        TryGetComponent(out moveRotate);
        TryGetComponent(out circleColl);
        moveTimer = moveTime;
        timerOn = true;
    }

    private void OnEnable()
    {
        rigid.gravityScale = 0f;
        moveTimer = moveTime;
        timerOn = true;
        moveDir.MoveOff = false;
        moveRotate.Speed = 20f;
        maintenanceTimer = 0f;
    }

    private void Update()
    {
        if (timerOn)
        {
            moveTimer -= Time.deltaTime;
            if (moveTimer <= 0f)
            {
                moveTimer = 0f;
                rigid.gravityScale = 1f;
                rigid.velocity = Vector2.zero;
                moveDir.MoveOff = true;
                moveRotate.Speed = 0f;
                timerOn = false;
            }
        }
        else
        {
            maintenanceTimer += Time.deltaTime;
            if (maintenanceTimer >= 15f)
            {
                gameObject.SetActive(false);
                maintenanceTimer = 0f;
            }
        }

        headCollCheck();
    }

    private void onCollision(Collider2D[] collider)
    {
        if (collider == null)
        {
            return;
        }

        foreach (Collider2D coll in collider)
        {
            if((coll.gameObject.layer.Equals(LayerMask.NameToLayer("Monster"))
                || coll.gameObject.layer.Equals(LayerMask.NameToLayer("Ground"))
                || coll.gameObject.layer.Equals(LayerMask.NameToLayer("Footboard"))
                || coll.gameObject.layer.Equals(LayerMask.NameToLayer("Wall"))) 
                && !moveDir.MoveOff)
            {
                moveTimer = 0f;
                rigid.gravityScale = 1f;
                moveDir.MoveOff = true;
                rigid.velocity = Vector2.zero;
                moveRotate.Speed = 0f;
                timerOn = false;
            }
        }
    }

    private void headCollCheck()
    {
        Collider2D[] collider =
            Physics2D.OverlapCircleAll(circleColl.bounds.center, circleColl.radius);
        onCollision(collider);
    }
}
