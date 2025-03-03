using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WearWolf_Attack : PlayerAttack
{
    [Header("리틀본 기본 공격력")]
    [SerializeField] private int damage;

    [Header("기본공격 히트박스")]
    [SerializeField] private BoxCollider2D hitBox;

    private void onCollisions(Collider2D[] colliders)
    {
        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject.layer.Equals(LayerMask.NameToLayer("Monster")))
            {
                Monster monsterSc = collider.GetComponent<Monster>();
                monsterSc.Hit(damage * (playerStatus.PlayingGameStatus.physicalAttackPower / 100), new Vector2(transform.localScale.x, 3f));
            }
        }
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

        if ((!anim.GetCurrentAnimatorStateInfo(_info).IsName("Attack_01")
          || !anim.GetCurrentAnimatorStateInfo(_info).IsName("Attack_02")
          || !anim.GetCurrentAnimatorStateInfo(_info).IsName("JumpAttack"))
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
        if (skillACollTimer > 0)
        {
            skillACollTimer -= Time.deltaTime;
        }

        if (skillBCollTimer > 0)
        {
            skillBCollTimer -= Time.deltaTime;
        }
    }

    public override void SkillAttack(int _skillnumber)
    {
        if (!isSkillAttack && !state.StateEnum.Equals(StateEnum.SwitchAttack))
        {
            if (_skillnumber.Equals(0) && skillACollTimer <= 0f)
            {

            }
            else if (_skillnumber.Equals(1) && skillBCollTimer <= 0f)
            {

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
        state.SetStateEnum(gravity.IsGround == true ? StateEnum.Idle : StateEnum.Jump, gravity.IsGround);
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
        state.SetStateEnum(gravity.IsGround == true ? StateEnum.Idle : StateEnum.Jump, gravity.IsGround);
    }

    public override void EndSkillAttack()
    {
        isSkillAttack = false;
        anim.SetBool("isSkillAttack", isSkillAttack);
        state.SetStateEnum(gravity.IsGround == true ? StateEnum.Idle : StateEnum.Jump, gravity.IsGround);
    }

    public override IEnumerator SwitchAttackCoroutine()
    {
        if (isSwitchAttack)
        {
            state.SetStateEnum(StateEnum.SwitchAttack, gravity.IsGround);
            anim.SetTrigger("SwitchAttack");
            moveDir.MoveDir = Vector2.zero;
            moveDir.MoveOff = true;
            rigid.velocity = Vector2.zero;
            rigid.velocity = new Vector2(40f * transform.localScale.x, 5f);
            damage = 30;
        }
        yield return new WaitForSeconds(0.2f);
        damage = 9;
        moveDir.MoveOff = false;
        state.SetStateEnum(StateEnum.Idle, true);
        isSwitchAttack = false;
    }
}
