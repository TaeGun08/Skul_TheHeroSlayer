using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerAttack : MonoBehaviour
{
    protected KeyManager keyManager;

    protected PlayerStatus playerStatus;

    protected Animator anim;
    protected MoveDirection moveDir;
    protected Rigidbody2D rigid;
    protected Gravity gravity;
    protected State state;

    [Header("공격설정")]
    [SerializeField] protected int maxAttackCount;
    [SerializeField] protected float skillACollTime;
    public float SkillACollTime { get { return skillACollTime; } set { skillACollTime = value; } }
    [SerializeField] protected float skillBCollTime;
    public float SkillBCollTime { get { return skillBCollTime; } set { skillBCollTime = value; } }

    protected float skillACollTimer;
    public float SkillACollTimer { get { return skillACollTimer; } set { skillACollTimer = value; } }
    protected float skillBCollTimer;
    public float SkillBCollTimer { get { return skillBCollTimer; } set { skillBCollTimer = value; } }

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
        TryGetComponent(out playerStatus);
        TryGetComponent(out anim);
        TryGetComponent(out moveDir);
        TryGetComponent(out rigid);
        TryGetComponent(out gravity);
        TryGetComponent(out state);
    }

    protected virtual void Start()
    {
        if (GameManager.Instance.ManagersDictionary.TryGetValue("KeyManager", out object _keyManager))
        {
            keyManager = _keyManager as KeyManager;
        }
    }

    protected abstract IEnumerator AttackMoveCoroutine();

    public abstract void ResetAttack(int _info);

    public abstract void Attack(int _info);

    public abstract void ResetSkillAttack();

    public abstract void SkillAttack(int _skillNumber);

    public abstract void ComboAttack();

    public abstract void EndAttack();

    public abstract void EndSkillAttack();

    public abstract void Hit(int _hitMonsters);
}
