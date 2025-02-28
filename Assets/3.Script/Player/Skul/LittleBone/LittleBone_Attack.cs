using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LittleBone_Attack : PlayerAttack
{
    [Header("리틀본 어택")]
    [SerializeField] private LittleBone_Head littleBone_Head;
    private LittleBone_Head littleBone_Head_Clone;

    [Header("기본공격 히트박스")]
    [SerializeField] private BoxCollider2D boxColl;

    private IEnumerator head()
    {
        yield return new WaitForSeconds(0.2f);

        littleBone_Head_Clone.gameObject.SetActive(true);
        gravity.enabled = true;
        isSkillAttack = false;
        anim.SetBool("isSkillAttack", isSkillAttack);
    }

    protected override IEnumerator AttackMoveCoroutine()
    {
        moveDir.enabled = false;
        rigid.velocity += new Vector2(5f * transform.localScale.x, 0f);
        yield return new WaitForSeconds(0.1f);
        moveDir.enabled = true;
        yield return null;
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

            rigid.velocity = Vector2.zero;

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

    public override void SkillAttack(int _skillnumber)
    {
        if (!isSkillAttack)
        {
            isSkillAttack = true;

            moveDir.MoveDir = Vector2.zero;
            rigid.velocity = Vector2.zero;
            gravity.enabled = false;
            gravity.Velocity = 0f;

            if (_skillnumber.Equals(0))
            {
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
            else
            {
                if (littleBone_Head_Clone.gameObject.activeSelf)
                {
                    rigid.velocity = Vector2.zero;
                    transform.position = littleBone_Head_Clone.transform.position;
                    littleBone_Head_Clone.gameObject.SetActive(false);
                    isSkillAttack = false;
                    gravity.enabled = true;
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
    }

    public override void EndSkillAttack()
    {
        isSkillAttack = false;
        anim.SetBool("isSkillAttack", isSkillAttack);
    }

    public override void Hit(int _hitMonsters)
    {
        if (isAttack)
        {

        }
    }
}
