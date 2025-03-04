using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WereWolf_Attack : PlayerAttack
{
    [Header("리틀본 기본 공격력")]
    [SerializeField] private int damage;

    [Header("기본공격 히트박스")]
    [SerializeField] private BoxCollider2D hitBox;

    [Header("스킬 히트박스")]
    [SerializeField] private BoxCollider2D[] skillHitBoxs;
    private int useSkillNumber;
    private int skill01Count;
    private float skill01Timer;
    private bool skill01On;

    private void onCollisions(Collider2D[] colliders)
    {
        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject.layer.Equals(LayerMask.NameToLayer("Monster")))
            {
                Monster monsterSc = collider.GetComponent<Monster>();
                monsterSc.Hit(damage * (playerStatus.PlayingGameStatus.physicalAttackPower / 100), new Vector2(transform.localScale.x, 0f));
                Vector2 scale = transform.localScale;
                scale.x *= -1f;
                Instantiate(vfx[0], monsterSc.transform.position, Quaternion.identity).GetComponent<VFX>().Scale = scale;
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

    private void useWereWolfSkill()
    {
        if (skillACollTimer <= 0f)
        {
            isSkillAttack = true;
            anim.SetBool("isSkillAttack", isSkillAttack);
            anim.SetInteger("SkillCount", hasSkillNumber[0]);
            useSkillNumber = hasSkillNumber[0];

            moveDir.MoveDir = Vector2.zero;
            rigid.velocity = Vector2.zero;
            gravity.Velocity = 0f;
            state.SetStateEnum(StateEnum.SkillAttack, gravity.IsGround);

            switch (hasSkillNumber[0])
            {
                case 1:
                    StopCoroutine("skill01Coroutine");
                    StartCoroutine("skill01Coroutine");
                    skillACollTimer = 15f;
                    skillACollTime = 15f;
                    skill01Count = 2;
                    skill01On = true;
                    skill01Timer = 10f;
                    break;
                case 2:
                    skillACollTimer = 15f;
                    skillACollTime = 15f;
                    damage = 15;
                    break;
                case 3:
                    skillACollTimer = 20f;
                    skillACollTime = 20f;
                    damage = 20;
                    break;
                case 4:
                    StopCoroutine("skill04Coroutine");
                    StartCoroutine("skill04Coroutine");
                    skillACollTimer = 20f;
                    skillACollTime = 20f;
                    break;
            }
        }
        else if (skill01On && skill01Count > 0)
        {
            isSkillAttack = true;
            anim.SetBool("isSkillAttack", isSkillAttack);
            anim.SetInteger("SkillCount", hasSkillNumber[0]);
            useSkillNumber = hasSkillNumber[0];

            moveDir.MoveDir = Vector2.zero;
            rigid.velocity = Vector2.zero;
            gravity.Velocity = 0f;
            state.SetStateEnum(StateEnum.SkillAttack, gravity.IsGround);
            StopCoroutine("skill01Coroutine");
            StartCoroutine("skill01Coroutine");
            skill01Count--;
        }
    }

    private IEnumerator skill01Coroutine()
    {
        EndAttack();
        state.SetStateEnum(StateEnum.SwitchAttack, gravity.IsGround);
        moveDir.MoveDir = Vector2.zero;
        moveDir.MoveOff = true;
        rigid.velocity = Vector2.zero;
        rigid.velocity = new Vector2(30f * transform.localScale.x, 8f);
        damage = 15;
        VFX vfxSc = Instantiate(vfx[1], transform.position, Quaternion.identity).GetComponent<VFX>();
        vfxSc.Scale = transform.localScale;
        vfxSc.transform.position -= new Vector3(transform.localScale.x, 10f, 0f);
        yield return new WaitForSeconds(0.15f);
        damage = 9;
        moveDir.MoveOff = false;
        isSkillAttack = false;
        state.SetStateEnum(StateEnum.Idle, true);
        anim.SetBool("isSkillAttack", isSkillAttack);
    }

    private IEnumerator skill04Coroutine()
    {
        EndAttack();
        state.SetStateEnum(StateEnum.SwitchAttack, gravity.IsGround);
        moveDir.MoveDir = Vector2.zero;
        moveDir.MoveOff = true;
        gravity.enabled = false;
        rigid.velocity = Vector2.zero;
        rigid.velocity = new Vector2(40f * transform.localScale.x, 0f);
        damage = 40;
        VFX vfxSc = Instantiate(vfx[1], transform.position, Quaternion.identity).GetComponent<VFX>();
        vfxSc.Scale = transform.localScale;
        vfxSc.transform.position -= new Vector3(transform.localScale.x, 0f, 0f);
        yield return new WaitForSeconds(0.05f);
        damage = 9;
        yield return new WaitForSeconds(0.10f);
        moveDir.MoveOff = false;
        isSkillAttack = false;
        gravity.enabled = true;
        state.SetStateEnum(StateEnum.Idle, true);
        anim.SetBool("isSkillAttack", isSkillAttack);
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

    public void SkillMontserCollCheck()
    {
        Collider2D[] monsters = Physics2D.OverlapBoxAll(skillHitBoxs[useSkillNumber - 1].bounds.center, hitBox.bounds.size, 0.0f,
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
            }
        }
    }

    public override void ResetSkillAttack()
    {
        if (skillACollTimer > 0)
        {
            skillACollTimer -= Time.deltaTime;
        }

        if (skill01Timer > 0 && skill01On)
        {
            skill01Timer -= Time.deltaTime;
        }
        else
        {
            skill01On = false;
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
            if (_skillnumber.Equals(0))
            {
                useWereWolfSkill();
            }
            else if (_skillnumber.Equals(1) && skillBCollTimer <= 0f && !hasSkillNumber[1].Equals(0))
            {
                useWereWolfSkill();
            }
        }
    }

    public override void ComboAttack()
    {
        moveDir.enabled = true;
        isAttack = false;
        anim.SetBool("isAttack", isAttack);
        anim.SetInteger("AttackCount", attackCount);
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
        useSkillNumber = 0;
        damage = 9;
        isSkillAttack = false;
        anim.SetBool("isSkillAttack", isSkillAttack);
        state.SetStateEnum(gravity.IsGround == true ? StateEnum.Idle : StateEnum.Jump, gravity.IsGround);
    }

    public override IEnumerator SwitchAttackCoroutine()
    {
        if (isSwitchAttack)
        {
            EndAttack();
            state.SetStateEnum(StateEnum.SwitchAttack, gravity.IsGround);
            anim.SetTrigger("SwitchAttack");
            gravity.enabled = false;
            moveDir.MoveDir = Vector2.zero;
            moveDir.MoveOff = true;
            rigid.velocity = Vector2.zero;
            rigid.velocity = new Vector2(40f * transform.localScale.x, 0f);
            damage = 30;
            VFX vfxSc = Instantiate(vfx[1], transform.position, Quaternion.identity).GetComponent<VFX>();
            vfxSc.Scale = transform.localScale;
            vfxSc.transform.position -= new Vector3(transform.localScale.x, 0f, 0f);
        }
        yield return new WaitForSeconds(0.05f);
        damage = 9;
        yield return new WaitForSeconds(0.10f);
        gravity.enabled = true;
        moveDir.MoveOff = false;
        state.SetStateEnum(StateEnum.Idle, true);
        isSwitchAttack = false;
    }
}
