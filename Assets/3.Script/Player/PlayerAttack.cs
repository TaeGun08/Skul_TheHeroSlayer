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

    protected bool isSwitchAttack;
    public bool IsSwitchAttack { get { return isSwitchAttack; } set { isSwitchAttack = value; } }

    [SerializeField] protected int hasSkillCount;
    public int HasSkillCount { get { return hasSkillCount; } set { hasSkillCount = value; } }

    protected int[] hasSkillNumber = new int[2];
    public int[] HasSkillNumber { get { return hasSkillNumber; } set { hasSkillNumber = value; } }

    protected virtual void Awake()
    {
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

        GetComponentInParent<PlayerStatus>().TryGetComponent(out playerStatus);
    }

    protected abstract IEnumerator AttackMoveCoroutine();

    public abstract void ResetAttack(int _info);

    public abstract void Attack(int _info);

    public abstract void ResetSkillAttack();

    public abstract void SkillAttack(int _skillNumber);

    public abstract void ComboAttack();

    public abstract void EndAttack();

    public abstract void EndSkillAttack();

    public abstract IEnumerator SwitchAttackCoroutine();

    public virtual void GetRandomSkill()
    {
        if (hasSkillCount.Equals(1))
        {
            hasSkillNumber[0] = Random.Range(0, 4);
        }
        else
        {
            hasSkillNumber[0] = Random.Range(0, 4);
            hasSkillNumber[1] = Random.Range(0, 4);
            while (hasSkillNumber[0].Equals(hasSkillNumber[1]))
            {
                hasSkillNumber[1] = Random.Range(0, 4);
            }
        }
    }
}
