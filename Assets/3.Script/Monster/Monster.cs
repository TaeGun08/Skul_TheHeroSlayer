using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Monster : MonoBehaviour, Hit
{
    protected Animator anim;
    protected SpriteRenderer spriteRen;
    protected Rigidbody2D rigid;
    protected State state;
    protected Gravity gravity;
    protected MoveDirection moveDir;

    [Header("∏ÛΩ∫≈Õ")]
    [SerializeField] protected int hp;
    protected int curHp;
    [SerializeField] protected float speed;
    [SerializeField] protected int damage;
    [SerializeField] protected int pattern;
    protected bool isAttack;
    protected bool isHit;
    [SerializeField] protected bool isknockback;
    public bool Isknockback { get { return isknockback; } set { isknockback = value; } }
    [SerializeField] protected bool isHitAnim;
    public bool IsHitAnim { get { return isHitAnim; } set { isHitAnim = value; } }
    protected bool playerCheck;
    [SerializeField] protected Image hpBar;

    protected virtual void Awake()
    {
        TryGetComponent(out anim);
        TryGetComponent(out spriteRen);
        TryGetComponent(out rigid);
        TryGetComponent(out state);
        TryGetComponent(out gravity);
        TryGetComponent(out moveDir);

        curHp = hp;
    }

    protected abstract void attack(Collider2D _collider = null);

    protected IEnumerator hitColorChangeCoroutine()
    {
        spriteRen.color = Color.gray;
        yield return new WaitForSeconds(0.1f);
        spriteRen.color = Color.white;
    }

    protected IEnumerator knockbackCoroutine(Vector2 _knockback)
    {
        rigid.velocity = Vector2.zero;
        rigid.velocity = _knockback.normalized * 5f;
        yield return new WaitForSeconds(0.1f);
        rigid.velocity = Vector2.zero;
        moveDir.enabled = true;
        yield return null;
    }

    public virtual void AttackEnd()
    {
        isAttack = false;
        state.SetStateEnum(StateEnum.Idle, gravity.IsGround);
    }

    public virtual void HitEnd()
    {
        isHit = false;
        moveDir.MoveOff = false;
    }

    public virtual void Hit(int _damage, Vector2 _knockback)
    {
        curHp -= _damage;
        hpBar.fillAmount = (float)curHp / hp;
        if (isknockback)
        {
            rigid.velocity = Vector2.zero;
            moveDir.MoveOff = true;
            moveDir.MoveDir = Vector2.zero;
            moveDir.enabled = false;
            state.SetStateEnum(StateEnum.Idle, gravity.IsGround);
            StartCoroutine(knockbackCoroutine(_knockback));
        }

        if (isHitAnim)
        {
            anim.SetTrigger("Hit");
        }

        StopCoroutine("hitColorChangeCoroutine");
        StartCoroutine("hitColorChangeCoroutine");

        if (curHp < 0)
        {
            Destroy(gameObject);
        }
    }
}
