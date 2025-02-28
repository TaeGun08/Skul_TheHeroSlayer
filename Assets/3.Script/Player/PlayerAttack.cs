using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerAttack : MonoBehaviour
{
    protected Animator anim;
    protected KeyManager keyManager;
    protected MoveDirection moveDir;
    protected Rigidbody2D rigid;
    protected Gravity gravity;

    [Header("공격설정")]
    [SerializeField] protected int maxAttackCount;
    [SerializeField] protected float attackResetTime;
    protected float attackTimer;

    protected int attackCount;
    public int AttackCount { get { return attackCount; } set { attackCount = value; } }

    protected bool isAttack;
    public bool IsAttack { get { return isAttack; } set { isAttack = value; } }

    protected bool isSkillAttack;
    public bool IsSkillAttack { get { return isSkillAttack; } set { isSkillAttack = value; } }

    protected bool isJumpAttack;
    public bool IsJumpAttack { get { return isJumpAttack; } set { isJumpAttack = value; } }

    protected bool isComboAttack;

    protected virtual void Awake()
    {
        TryGetComponent(out anim);
        TryGetComponent(out moveDir);
        TryGetComponent(out rigid);
        TryGetComponent(out gravity);
    }

    protected virtual void Start()
    {
        FindAnyObjectByType<KeyManager>().TryGetComponent(out keyManager);
    }

    protected abstract IEnumerator AttackMoveCoroutine();

    public abstract void ResetAttack(int _info);

    public abstract void Attack(int _info);

    public abstract void SkillAttack(int _skillNumber);

    public abstract void ComboAttack();

    public abstract void EndAttack();

    public abstract void EndSkillAttack();

    public abstract void Hit(int _hitMonsters);
}
