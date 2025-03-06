using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LittleBone_Attack : PlayerAttack
{
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

    private void OnDisable()
    {
        if (littleBone_Head_Clone != null)
        {
            Destroy(littleBone_Head_Clone.gameObject);
            littleBone_Head_Clone = null;
        }
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
                anim.SetLayerWeight(0, 1.0f);
                anim.SetLayerWeight(1, 0f);
                littleBone_Head_Clone.gameObject.SetActive(false);
                throwingHead = false;
                skillACoolTimer = 0f;
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
                Boss bossSc = collider.GetComponent<Boss>();

                if (monsterSc != null)
                {
                    monsterSc.Hit(damage * (playerStatus.PlayingGameStatus.physicalAttackPower / 100), new Vector2(transform.localScale.x, 0f));
                    Vector2 scale = transform.localScale;
                    scale.x *= -1f;
                    Instantiate(vfx[0], monsterSc.transform.position, Quaternion.identity).GetComponent<VFX>().Scale = scale;
                }
                
                if (bossSc != null)
                {
                    bossSc.Hit(damage * (playerStatus.PlayingGameStatus.physicalAttackPower / 100));
                    Vector2 scale = transform.localScale;
                    scale.x *= -1f;
                    Instantiate(vfx[0], bossSc.transform.position, Quaternion.identity).GetComponent<VFX>().Scale = scale;
                }
            }
        }
    }

    private void headCollCheck()
    {
        Collider2D collider =
            Physics2D.OverlapBox(boxColl.bounds.center, boxColl.bounds.size, 0.0f, LayerMask.GetMask("Head"));
        onCollision(collider);
    }

    private IEnumerator headCoroutine()
    {
        yield return new WaitForSeconds(0.2f);

        littleBone_Head_Clone.gameObject.SetActive(true);
        gravity.enabled = true;
        throwingHead = true;
        EndSkillAttack();
    }

    private IEnumerator activeFalseHeadCoroutine()
    {
        yield return new WaitForSeconds(15f);
        littleBone_Head_Clone.gameObject.SetActive(false);
        anim.SetLayerWeight(0, 1.0f);
        anim.SetLayerWeight(1, 0f);
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
        inventoryManager.UseItemAbility(StateEnum.Attack);

        Collider2D[] monsters = Physics2D.OverlapBoxAll(hitBox.bounds.center, hitBox.bounds.size, 0.0f,
            LayerMask.GetMask("Monster"));

        if (!monsters.Length.Equals(0))
        {
            onCollisions(monsters);
        }
    }

    public void SwitchMonsterCollCheck()
    {
        Collider2D[] monsters = Physics2D.OverlapBoxAll(hitBox.bounds.center, hitBox.bounds.size, 0.0f,
           LayerMask.GetMask("Monster"));

        if (!monsters.Length.Equals(0))
        {
            onCollisions(monsters);
        }
    }

    public override void Attack()
    {
        if (!state.StateEnum.Equals(StateEnum.Attack) 
            && !state.StateEnum.Equals(StateEnum.JumpAttack)
            && !state.StateEnum.Equals(StateEnum.SkillAttack))
        {
            rigid.velocity = new Vector2(0f, rigid.velocity.y);

            state.SetStateEnum(gravity.IsGround == true ? StateEnum.Attack : StateEnum.JumpAttack, gravity.IsGround);

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

            if (!isComboAttack)
            {
                anim.SetTrigger("Attack");
            }
        }
        else
        {
            if (attackCount.Equals(0)
                && anim.GetCurrentAnimatorStateInfo(0).IsName("Attack_01")
                && !state.StateEnum.Equals(StateEnum.JumpAttack))
            {
                attackCount++;
                isComboAttack = true;
            }
        }
    }

    public override void ResetAttack()
    {
        if (littleBone_Head_Clone != null)
        {
            if (!littleBone_Head_Clone.gameObject.activeSelf && throwingHead)
            {
                throwingHead = false;
            }
        }

        if (skillACoolTimer > 0)
        {
            skillACoolTimer -= Time.deltaTime;
        }

        if (skillBCoolTimer > 0)
        {
            skillBCoolTimer -= Time.deltaTime;
        }

        headCollCheck();
    }

    public override void SkillAttack(int _skillnumber)
    {
        if (!state.StateEnum.Equals(StateEnum.SwitchAttack))
        {
            if (_skillnumber.Equals(0) && skillACoolTimer <= 0f)
            {
                EndAttack();
                moveDir.MoveDir = Vector2.zero;
                rigid.velocity = Vector2.zero;
                gravity.enabled = false;
                gravity.Velocity = 0f;
                state.SetStateEnum(StateEnum.SkillAttack, gravity.IsGround);
                skillACoolTimer = skillACoolTime;

                anim.SetBool("isSkillAttack", state.StateEnum.Equals(StateEnum.SkillAttack));

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

                anim.SetLayerWeight(0, 0f);
                anim.SetLayerWeight(1, 1.0f);

                littleBone_Head_Clone.MoveDir.MoveDir = new Vector2(dir, 0f);
                littleBone_Head_Clone.gameObject.SetActive(false);
                littleBone_Head_Clone.Damage = (int)(damage * 1.5f)  * (playerStatus.PlayingGameStatus.physicalAttackPower / 100);
                StopCoroutine("headCoroutine");
                StartCoroutine("headCoroutine");
                StopCoroutine("activeFalseHeadCoroutine");
                StartCoroutine("activeFalseHeadCoroutine");
            }
            else if (_skillnumber.Equals(1) && skillBCoolTimer <= 0f)
            {
                if (littleBone_Head_Clone == null)
                {
                    return;
                }

                if (littleBone_Head_Clone.gameObject.activeSelf && throwingHead)
                {
                    EndAttack();
                    moveDir.MoveDir = Vector2.zero;
                    rigid.velocity = Vector2.zero;
                    gravity.enabled = false;
                    gravity.Velocity = 0f;
                    state.SetStateEnum(StateEnum.SkillAttack, gravity.IsGround);

                    anim.SetLayerWeight(0, 1.0f);
                    anim.SetLayerWeight(1, 0f);

                    skillBCoolTimer = skillBCoolTime;
                    skillACoolTimer = 0f;
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
        anim.SetInteger("AttackCount", attackCount);
        anim.SetBool("isComboAttack", isComboAttack);
        state.SetStateEnum(gravity.IsGround == true ? StateEnum.Idle : StateEnum.Jump, gravity.IsGround);
    }

    public override void EndAttack()
    {
        moveDir.enabled = true;
        attackCount = 0;
        isComboAttack = false;
        anim.SetInteger("AttackCount", attackCount);
        anim.SetBool("isComboAttack", isComboAttack);
        state.SetStateEnum(gravity.IsGround == true ? StateEnum.Idle : StateEnum.Jump, gravity.IsGround);
    }

    public override void EndSkillAttack()
    {
        state.SetStateEnum(gravity.IsGround == true ? StateEnum.Idle : StateEnum.Jump, gravity.IsGround);
        anim.SetBool("isSkillAttack", false);
    }

    public override IEnumerator SwitchAttackCoroutine()
    {
        if (isSwitchAttack)
        {
            gravity.enabled = true;
            EndAttack();
            state.SetStateEnum(StateEnum.SwitchAttack, gravity.IsGround);
            anim.SetTrigger("SwitchAttack");
            moveDir.MoveDir = Vector2.zero;
            moveDir.MoveOff = true;
            rigid.velocity = Vector2.zero;
            rigid.velocity = new Vector2(2f * transform.localScale.x, 0f);
            damage = 4;
        }
        yield return new WaitForSeconds(1.5f);
        damage = 8;
        moveDir.MoveOff = false;
        isSwitchAttack = false;
        state.SetStateEnum(StateEnum.Idle, true);
    }
}
