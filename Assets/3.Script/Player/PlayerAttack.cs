using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerAttack : MonoBehaviour
{
    protected Animator anim;

    protected virtual void Awake()
    {
        TryGetComponent(out anim);
    }

    protected int attackCount;
    public int AttackCount { get { return attackCount; } set { attackCount = value; } }

    protected int skillNumber;
    public int SkillNumber { get { return skillNumber; } set { skillNumber = value; } }

    protected bool isAttack;
    public bool IsAttack { get { return isAttack; } set { isAttack = value; } }

    public abstract void Attack();

    public abstract void ReAttack();

    public abstract void SkillAttack();

    public abstract void EndAttack();
}
