using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallTree : Monster
{
    [Header("³ª¹«")]
    [SerializeField] private BoxCollider2D playerCheckColl;
    [SerializeField] private GroundCheck groundCheck;
    [SerializeField] private BoxCollider2D hitBox;
    private float randomIdleTime;
    private float randomIdleTimer;
    private float randomMoveTime;
    private float randomMoveTimer;
    private float attackDelay;
    private Collider2D hitCollider;

    private void FixedUpdate()
    {
        if (isHit)
        {
            state.SetStateEnum(StateEnum.Idle, true);
            return;
        }

        if (!playerCheck)
        {
            smallTreeMove();
        }

        hitCollCheck();
    }

    private void Update()
    {
        playerAnim();
    }

    private void smallTreeMove()
    {
        if (state.StateEnum.Equals(StateEnum.Idle))
        {
            randomIdleTimer += Time.deltaTime;
            if (randomIdleTimer >= randomIdleTime)
            {
                randomMove();
            }
        }
        else if (state.StateEnum.Equals(StateEnum.Walk))
        {
            if (!groundCheck.Ground)
            {
                flip();
                return;
            }

            randomMoveTimer += Time.deltaTime;
            if (randomMoveTimer >= randomMoveTime)
            {
                randomIdle();
            }
        }
    }

    private void randomIdle()
    {
        randomIdleTimer = 0f;
        moveDir.MoveOff = true;
        rigid.velocity = Vector2.zero;
        randomIdleTime = Random.Range(1f, 3f);
        state.SetStateEnum(StateEnum.Idle, gravity.IsGround);
    }

    private void randomMove()
    {
        flip();
        randomMoveTimer = 0f;
        randomMoveTime = Random.Range(3f, 7f);
        moveDir.MoveOff = false;
        state.SetStateEnum(StateEnum.Walk, gravity.IsGround);
    }

    private void flip()
    {
        Vector2 scale = transform.localScale;
        if (transform.localScale.x > 0)
        {
            scale.x = -1;
        }
        else if (transform.localScale.x < 0)
        {
            scale.x = 1;
        }
        transform.localScale = scale;
        moveDir.MoveDir = new Vector2(2f * transform.localScale.x, 0);
    }

    private void playerAnim()
    {
        if (state.StateEnum.Equals(StateEnum.Idle))
        {
            anim.SetBool("isWalk", false);
        }
        else if (state.StateEnum.Equals(StateEnum.Walk))
        {
            anim.SetBool("isWalk", true);
        }

        anim.SetBool("isAttack", isAttack);
    }

    private void trackingFlip(Vector2 _direction)
    {
        Vector2 scale = transform.localScale;
        if (_direction.x < 0)
        {
            scale.x = -1;
        }
        else if (_direction.x > 0)
        {
            scale.x = 1;
        }
        transform.localScale = scale;
        moveDir.MoveDir = new Vector2(2f * transform.localScale.x, 0);
    }

    private void hitCollCheck()
    {
        Collider2D collider = Physics2D.OverlapBox(playerCheckColl.bounds.center, playerCheckColl.bounds.size, 0.0f,
            LayerMask.GetMask("Player"));

        hitCollider = Physics2D.OverlapBox(hitBox.bounds.center, hitBox.bounds.size, 0.0f,
            LayerMask.GetMask("Player"));

        attack(collider);
    }

    private IEnumerator attackCoroutine(PlayerController _playerController)
    {
        yield return new WaitForSeconds(1f);
        anim.SetBool("isAttackReady", false);
        isAttack = true;
        attackDelay = 0f;
        yield return new WaitForSeconds(0.3f);
        if (hitCollider != null)
        {
            _playerController.Hit(damage, new Vector2(transform.localScale.x * 3f, 3f));
        }
        hitCollider = null;
    }

    protected override void attack(Collider2D _collider = null)
    {
        playerCheck = false;

        if (_collider == null)
        {
            return;
        }

        if (_collider.gameObject.layer.Equals(LayerMask.NameToLayer("Player")))
        {
            playerCheck = true;

            float distance = Vector2.Distance(_collider.transform.position, transform.position);
            Vector2 direction = (_collider.transform.position - transform.position).normalized;

            if (!state.StateEnum.Equals(StateEnum.Attack) && distance <= 1f)
            {
                rigid.velocity = Vector2.zero;
                moveDir.MoveOff = true;
                anim.SetBool("isWalk", false);
                state.SetStateEnum(StateEnum.Idle, gravity.IsGround);

                attackDelay += Time.deltaTime;

                if (attackDelay >= 3f && hitCollider != null)
                {
                    anim.SetBool("isAttackReady", true);
                    StartCoroutine(attackCoroutine(hitCollider.gameObject.GetComponent<PlayerController>()));
                    state.SetStateEnum(StateEnum.Attack, gravity.IsGround);
                }
            }
            else if (!state.StateEnum.Equals(StateEnum.Attack) && distance > 1f)
            {
                trackingFlip(direction);
                smallTreeMove();
            }
        }
    }

    public override void Hit(int _damage, Vector2 _knockback)
    {
        base.Hit(_damage, _knockback);
        isHit = true;
    }
}
