using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LittleBone_Attack : PlayerAttack
{
    [Header("기본공격 히트박스")]
    [SerializeField] private BoxCollider2D boxColl;

    public override void Attack()
    {
        if (isAttack)
        {
            attackCount++;
        }
        //boxColl.gameObject.SetActive(true);
    }

    public override void ReAttack()
    {
        if (isAttack)
        {
            anim.SetFloat("AttackCount", attackCount);
            isAttack = false;
        }
    }

    public override void SkillAttack()
    {
        if (isAttack)
        {
            isAttack = false;
        }
    }

    public override void EndAttack()
    {
        if (isAttack)
        {
            isAttack = false;
        }
    }
}
