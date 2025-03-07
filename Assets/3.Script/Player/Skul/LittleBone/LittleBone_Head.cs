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
    [SerializeField] private VFX hit;
    private float moveTimer;
    private bool timerOn;
    private int damage;
    public int Damage { set { damage = value; } }

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
    }

    private void Update()
    {
        if (timerOn)
        {
            moveTimer -= Time.deltaTime;
            if (moveTimer <= 0f)
            {
                moveStop();
            }
        }

        headCollCheck();
    }

    private void moveStop()
    {
        StopCoroutine("hitHeadCoroutine");
        StartCoroutine("hitHeadCoroutine");
        moveTimer = 0f;
        rigid.gravityScale = 1f;
        moveDir.MoveOff = true;
        rigid.velocity = Vector2.zero;
        rigid.velocity = new Vector2(transform.localScale.x * -1f, 3f);
        moveRotate.Speed = 5f * transform.localScale.x;
        timerOn = false;
        Vector2 scale = transform.localScale;
        scale.x *= -1f;
        Instantiate(hit, transform.position, Quaternion.identity).GetComponent<VFX>().Scale = scale;
    }

    private IEnumerator hitHeadCoroutine()
    {
        yield return new WaitForSeconds(3f);
        rigid.velocity = Vector2.zero;
        moveRotate.Speed = 0f;
    }

    private void onCollision(Collider2D[] collider)
    {
        if (collider == null)
        {
            return;
        }

        foreach (Collider2D coll in collider)
        {
            if (!moveDir.MoveOff)
            {
                if (coll.gameObject.layer.Equals(LayerMask.NameToLayer("Monster")))
                {
                    coll.TryGetComponent(out Hit _hit);
                    _hit.Hit(damage, Vector2.zero);
                    moveStop();
                }

                if ((coll.gameObject.layer.Equals(LayerMask.NameToLayer("Ground"))
                     || coll.gameObject.layer.Equals(LayerMask.NameToLayer("Wall"))))
                {
                    moveStop();
                }
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
