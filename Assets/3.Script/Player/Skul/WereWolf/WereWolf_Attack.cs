using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WereWolf_Attack : PlayerAttack
{
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
            if (collider.TryGetComponent(out Hit _hit))
            {
                _hit.Hit(damage * (playerStatus.PlayingGameStatus.physicalAttackPower / 100), new Vector2(transform.localScale.x, 0f));
                Vector2 scale = transform.localScale;
                scale.x *= -1f;
                Instantiate(vfx[0], collider.transform.position, Quaternion.identity).GetComponent<VFX>().Scale = scale;
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

    private void useWereWolfSkill(int _skillNumber)
    {
        anim.SetFloat("SkillCount", hasSkillNumber[_skillNumber]);

        moveDir.MoveDir = Vector2.zero;
        rigid.velocity = Vector2.zero;
        gravity.Velocity = 0f;
        state.SetStateEnum(StateEnum.SkillAttack, gravity.IsGround);
        anim.SetBool("isSkillAttack", state.StateEnum.Equals(StateEnum.SkillAttack));

        switch (hasSkillNumber[_skillNumber])
        {
            case 1:
                StopCoroutine("skill01Coroutine");
                StartCoroutine("skill01Coroutine");
                skillACoolTimer = 15f;
                skillACoolTime = 15f;
                skill01Count = 2;
                skill01On = true;
                skill01Timer = 10f;
                useSkillNumber = 1;
                break;
            case 2:
                skillACoolTimer = 15f;
                skillACoolTime = 15f;
                damage = 15;
                useSkillNumber = 2;
                break;
            case 3:
                skillACoolTimer = 20f;
                skillACoolTime = 20f;
                damage = 20;
                useSkillNumber = 3;
                break;
            case 4:
                StopCoroutine("skill04Coroutine");
                StartCoroutine("skill04Coroutine");
                skillACoolTimer = 20f;
                skillACoolTime = 20f;
                useSkillNumber = 1;
                break;
        }
    }

    private void huntingSkill(int _skillNumber)
    {
        if (skill01On && skill01Count > 0)
        {
            anim.SetFloat("SkillCount", hasSkillNumber[_skillNumber]);
            useSkillNumber = hasSkillNumber[_skillNumber];

            moveDir.MoveDir = Vector2.zero;
            rigid.velocity = Vector2.zero;
            gravity.Velocity = 0f;
            state.SetStateEnum(StateEnum.SkillAttack, gravity.IsGround);
            anim.SetBool("isSkillAttack", state.StateEnum.Equals(StateEnum.SkillAttack));
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
        state.SetStateEnum(StateEnum.Idle, true);
        anim.SetBool("isSkillAttack", state.StateEnum.Equals(StateEnum.SkillAttack));
    }

    private IEnumerator skill04Coroutine()
    {
        EndAttack();
        state.SetStateEnum(StateEnum.SwitchAttack, gravity.IsGround);
        moveDir.MoveDir = Vector2.zero;
        moveDir.MoveOff = true;
        gravity.enabled = false;
        rigid.velocity = Vector2.zero;
        rigid.velocity = new Vector2(50f * transform.localScale.x, 0f);
        damage = 40;
        VFX vfxSc = Instantiate(vfx[1], transform.position, Quaternion.identity).GetComponent<VFX>();
        vfxSc.Scale = transform.localScale;
        vfxSc.transform.position -= new Vector3(transform.localScale.x, 0f, 0f);
        yield return new WaitForSeconds(0.05f);
        damage = 9;
        yield return new WaitForSeconds(0.10f);
        moveDir.MoveOff = false;
        gravity.enabled = true;
        state.SetStateEnum(StateEnum.Idle, true);
        anim.SetBool("isSkillAttack", state.StateEnum.Equals(StateEnum.SkillAttack));
    }

    public void MonsterCollCheck()
    {
        inventoryManager.UseItemAbility(StateEnum.Attack);

        Collider2D[] hit = Physics2D.OverlapBoxAll(hitBox.bounds.center, hitBox.bounds.size, 0.0f,
            LayerMask.GetMask("Monster"));

        if (!hit.Length.Equals(0))
        {
            onCollisions(hit);
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

    public void SwitchMonsterCollCheck()
    {
        Collider2D[] monsters = Physics2D.OverlapBoxAll(skillHitBoxs[0].bounds.center, hitBox.bounds.size, 0.0f,
           LayerMask.GetMask("Monster"));

        if (!monsters.Length.Equals(0))
        {
            onCollisions(monsters);
        }
    }

    public override void Attack()
    {
        if (!state.StateEnum.Equals(StateEnum.Attack) && !state.StateEnum.Equals(StateEnum.JumpAttack))
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
        if (skillACoolTimer > 0)
        {
            skillACoolTimer -= Time.deltaTime;
        }

        if (skillBCoolTimer > 0)
        {
            skillBCoolTimer -= Time.deltaTime;
        }

        if (skill01Timer > 0 && skill01On)
        {
            skill01Timer -= Time.deltaTime;
        }
    }

    public override void SkillAttack(int _skillnumber)
    {
        if (!state.StateEnum.Equals(StateEnum.SwitchAttack))
        {
            if (_skillnumber.Equals(0))
            {
                if (skillACoolTimer <= 0f)
                {
                    useWereWolfSkill(0);
                }
                else
                {
                    huntingSkill(0);
                }
            }
            else if (_skillnumber.Equals(1) && !hasSkillNumber[1].Equals(0))
            {
                if (skillBCoolTimer <= 0f)
                {
                    useWereWolfSkill(1);
                }
                else
                {
                    huntingSkill(1);
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
        useSkillNumber = 0;
        damage = 9;
        state.SetStateEnum(gravity.IsGround == true ? StateEnum.Idle : StateEnum.Jump, gravity.IsGround);
        anim.SetBool("isSkillAttack", false);
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
            rigid.velocity = new Vector2(50f * transform.localScale.x, 0f);
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
