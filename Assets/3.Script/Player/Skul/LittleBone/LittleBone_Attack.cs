using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LittleBone_Attack : PlayerAttack
{
    private BoxCollider2D boxColl;

    [Header("리틀본 기본 공격력")]
    [SerializeField] private int damage;

    [Header("리틀본 어택")]
    [SerializeField] private LittleBone_Head littleBone_Head;
    private LittleBone_Head littleBone_Head_Clone;
    private bool throwingHead;

    [Header("기본공격 히트박스")]
    [SerializeField] private BoxCollider2D hitBox;

    protected override void Awake()
    {
        base.Awake();
        TryGetComponent(out boxColl);
    }

    private void onCollision(Collider2D collider)
    {
        if (collider == null)
        {
            return;
        }

        if (collider.gameObject.layer.Equals(LayerMask.NameToLayer("Head")) && littleBone_Head_Clone != null)
        {
            if (littleBone_Head_Clone.MoveDir.MoveOff)
            {
                littleBone_Head_Clone.gameObject.SetActive(false);
                throwingHead = false;
                skillACollTimer = 0f;
            }
        }
    }

    private void onCollisions(Collider2D[] colliders)
    {
        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject.layer.Equals(LayerMask.NameToLayer("Monster")))
            {
                Monster monsterSc = collider.GetComponent<Monster>();
                monsterSc.Hit(damage, new Vector2(transform.localScale.x, 5f));
            }
        }
    }

    private void headCollCheck()
    {
        Collider2D collider = 
            Physics2D.OverlapBox(boxColl.bounds.center, boxColl.bounds.size, 0.0f, LayerMask.GetMask("Head"));
        onCollision(collider);
    }

    private IEnumerator head()
    {
        yield return new WaitForSeconds(0.2f);

        littleBone_Head_Clone.gameObject.SetActive(true);
        gravity.enabled = true;
        throwingHead = true;
        EndSkillAttack();
    }

    protected override IEnumerator AttackMoveCoroutine()
    {
        moveDir.enabled = false;
        rigid.velocity += new Vector2(5f * transform.localScale.x, 0f);
        yield return new WaitForSeconds(0.1f);
        moveDir.enabled = true;
        yield return null;
    }

    public void MonsterCollCheck()
    {
        Collider2D[] monsters = Physics2D.OverlapBoxAll(hitBox.bounds.center, hitBox.bounds.size, 0.0f,
            LayerMask.GetMask("Monster"));

        if (!monsters.Length.Equals(0))
        {
            onCollisions(monsters);
        }
    }

    public override void ResetAttack(int _info)
    {
        if (!isAttack)
        {
            isJumpAttack = false;
            return;
        }

        if (isComboAttack)
        {
            if (anim.GetCurrentAnimatorStateInfo(_info).IsName("Attack_01")
               && anim.GetCurrentAnimatorStateInfo(_info).normalizedTime >= 0.7f)
            {
                if (Input.GetKey(keyManager.Key.KeyCodes[2]))
                {
                    StopCoroutine("AttackMoveCoroutine");
                    StartCoroutine("AttackMoveCoroutine");
                }

                if (Input.GetKey(keyManager.Key.KeyCodes[3]))
                {
                    StopCoroutine("AttackMoveCoroutine");
                    StartCoroutine("AttackMoveCoroutine");
                }

                anim.SetBool("isComboAttack", isComboAttack);
            }
        }

        if ((anim.GetCurrentAnimatorStateInfo(_info).IsName("Attack_01")
          || anim.GetCurrentAnimatorStateInfo(_info).IsName("Attack_02")
          || anim.GetCurrentAnimatorStateInfo(_info).IsName("JumpAttack"))
          && anim.GetCurrentAnimatorStateInfo(_info).normalizedTime >= 1f)
        {
            EndAttack();
        }
    }

    public override void Attack(int _info)
    {
        if (!isAttack)
        {
            isAttack = true;

            rigid.velocity = new Vector2(0f, rigid.velocity.y);

            state.SetSateEnum(gravity.IsGround == true ? StateEnum.Attack : StateEnum.JumpAttack, gravity.IsGround);

            if (Input.GetKey(keyManager.Key.KeyCodes[2]))
            {
                StopCoroutine("AttackMoveCoroutine");
                StartCoroutine("AttackMoveCoroutine");
            }

            if (Input.GetKey(keyManager.Key.KeyCodes[3]))
            {
                StopCoroutine("AttackMoveCoroutine");
                StartCoroutine("AttackMoveCoroutine");
            }

            anim.SetInteger("AttackCount", attackCount);
            anim.SetBool("isAttack", isAttack);
        }
        else
        {
            if (attackCount.Equals(0) && anim.GetCurrentAnimatorStateInfo(_info).IsName("Attack_01"))
            {
                attackCount++;
                isComboAttack = true;
                anim.SetInteger("AttackCount", attackCount);
            }
        }
    }

    public override void ResetSkillAttack()
    {
        if (littleBone_Head_Clone != null)
        {
            if (!littleBone_Head_Clone.gameObject.activeSelf && throwingHead)
            {
                throwingHead = false;
            }
        }

        if (skillACollTimer > 0)
        {
            skillACollTimer -= Time.deltaTime;
        }

        if (skillBCollTimer > 0)
        {
            skillBCollTimer -= Time.deltaTime;
        }

        headCollCheck();
    }

    public override void SkillAttack(int _skillnumber)
    {
        if (!isSkillAttack)
        {
            if (_skillnumber.Equals(0) && skillACollTimer <= 0f)
            {
                isSkillAttack = true;

                moveDir.MoveDir = Vector2.zero;
                rigid.velocity = Vector2.zero;
                gravity.enabled = false;
                gravity.Velocity = 0f;
                state.SetSateEnum(StateEnum.SkillAttack, gravity.IsGround);
                skillACollTimer = skillACollTime;

                anim.SetBool("isSkillAttack", isSkillAttack);

                if (littleBone_Head_Clone == null)
                {
                    littleBone_Head_Clone = Instantiate(littleBone_Head);
                }
                littleBone_Head_Clone.transform.position = transform.position;
                littleBone_Head_Clone.transform.localScale = transform.localScale;

                float dir = littleBone_Head_Clone.MoveDir.MoveDir.x;
                if (transform.localScale.x > 0 && dir < 0)
                {
                    dir = littleBone_Head_Clone.MoveDir.MoveDir.x * -1f;
                }
                else if (transform.localScale.x < 0 && dir > 0)
                {
                    dir = littleBone_Head_Clone.MoveDir.MoveDir.x * -1f;
                }

                littleBone_Head_Clone.MoveDir.MoveDir = new Vector2(dir, 0f);
                littleBone_Head_Clone.gameObject.SetActive(false);
                StopCoroutine("head");
                StartCoroutine("head");
            }
            else if (_skillnumber.Equals(1) && skillBCollTimer <= 0f)
            {
                if (littleBone_Head_Clone == null)
                {
                    return;
                }

                if (littleBone_Head_Clone.gameObject.activeSelf && throwingHead)
                {
                    isSkillAttack = true;

                    moveDir.MoveDir = Vector2.zero;
                    rigid.velocity = Vector2.zero;
                    gravity.enabled = false;
                    gravity.Velocity = 0f;
                    state.SetSateEnum(StateEnum.SkillAttack, gravity.IsGround);

                    skillBCollTimer = skillBCollTime;
                    skillACollTimer = 0f;
                    rigid.velocity = Vector2.zero;
                    transform.position = littleBone_Head_Clone.transform.position;
                    littleBone_Head_Clone.gameObject.SetActive(false);
                    gravity.enabled = true;
                    throwingHead = false;
                    EndSkillAttack();
                }
            }
        }
    }

    public override void ComboAttack()
    {
        moveDir.enabled = true;
        isAttack = false;
        isComboAttack = false;
        anim.SetBool("isAttack", isAttack);
        anim.SetBool("isComboAttack", isComboAttack);
        state.SetSateEnum(gravity.IsGround == true ? StateEnum.Idle : StateEnum.Jump, gravity.IsGround);
    }

    public override void EndAttack()
    {
        moveDir.enabled = true;
        isAttack = false;
        attackCount = 0;
        isComboAttack = false;
        anim.SetBool("isAttack", isAttack);
        anim.SetInteger("AttackCount", attackCount);
        anim.SetBool("isComboAttack", isComboAttack);
        state.SetSateEnum(gravity.IsGround == true ? StateEnum.Idle : StateEnum.Jump, gravity.IsGround);
    }

    public override void EndSkillAttack()
    {
        isSkillAttack = false;
        anim.SetBool("isSkillAttack", isSkillAttack);
        state.SetSateEnum(gravity.IsGround == true ? StateEnum.Idle : StateEnum.Jump, gravity.IsGround);
    }

    public override void Hit(int _hitMonsters)
    {
        if (isAttack)
        {

        }
    }
}
