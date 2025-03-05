using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerAttack : MonoBehaviour
{
    protected KeyManager keyManager;
    protected InventoryManager inventoryManager;

    protected PlayerStatus playerStatus;

    protected Animator anim;
    protected MoveDirection moveDir;
    protected Rigidbody2D rigid;
    protected Gravity gravity;
    protected State state;
    protected BoxCollider2D boxColl;

    [Header("공격설정")]
    [SerializeField] protected int damage;
    public int Damage { get { return damage; } }
    [SerializeField] protected int maxAttackCount;
    [SerializeField] protected float skillACoolTime;
    public float SkillACoolTime { get { return skillACoolTime; } set { skillACoolTime = value; } }
    [SerializeField] protected float skillBCoolTime;
    public float SkillBCoolTime { get { return skillBCoolTime; } set { skillBCoolTime = value; } }

    protected float skillACoolTimer;
    public float SkillACoolTimer { get { return skillACoolTimer; } set { skillACoolTimer = value; } }
    protected float skillBCoolTimer;
    public float SkillBCoolTimer { get { return skillBCoolTimer; } set { skillBCoolTimer = value; } }

    protected int attackCount;
    public int AttackCount { get { return attackCount; } set { attackCount = value; } }

    protected bool isComboAttack;

    protected bool isSwitchAttack;
    public bool IsSwitchAttack { get { return isSwitchAttack; } set { isSwitchAttack = value; } }

    [SerializeField] protected int hasSkillCount;
    public int HasSkillCount { get { return hasSkillCount; } set { hasSkillCount = value; } }

    [SerializeField] protected int[] hasSkillNumber = new int[2];
    public int[] HasSkillNumber { get { return hasSkillNumber; } set { hasSkillNumber = value; } }

    [Header("히트 이펙트")]
    [SerializeField] protected VFX[] vfx;

    protected virtual void Awake()
    {
        TryGetComponent(out anim);
        TryGetComponent(out moveDir);
        TryGetComponent(out rigid);
        TryGetComponent(out gravity);
        TryGetComponent(out state);
        TryGetComponent(out boxColl);
    }

    protected virtual void Start()
    {
        if (GameManager.Instance.ManagersDictionary.TryGetValue("KeyManager", out object _keyManager))
        {
            keyManager = _keyManager as KeyManager;
        }

        if (GameManager.Instance.ManagersDictionary.TryGetValue("InventoryManager", out object _inventoryManager))
        {
            inventoryManager = _inventoryManager as InventoryManager;
        }

        GetComponentInParent<PlayerStatus>().TryGetComponent(out playerStatus);
    }

    protected abstract IEnumerator AttackMoveCoroutine();

    public abstract void ResetAttack();

    public abstract void Attack();

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
