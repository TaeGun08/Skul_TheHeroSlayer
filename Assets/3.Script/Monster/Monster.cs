using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Monster : MonoBehaviour
{
    protected Animator anim;
    protected SpriteRenderer spriteRen;
    protected Rigidbody2D rigid;

    [Header("∏ÛΩ∫≈Õ")]
    [SerializeField] protected int hp;
    [SerializeField] protected float speed;
    [SerializeField] protected int damage;
    [SerializeField] protected int pattern;
    protected bool isAttack;
    [SerializeField] protected bool isHit;

    protected virtual void Awake()
    {
        TryGetComponent(out anim);
        TryGetComponent(out spriteRen);
        TryGetComponent(out rigid);
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
    }

    public virtual void AttackEnd()
    {
        isAttack = false;
    }

    public virtual void HitEnd()
    {
        isHit = false;
    }

    public virtual void Hit(int _damage, Vector2 _knockback)
    {
        hp -= _damage;
        StartCoroutine(knockbackCoroutine(_knockback));
        anim.SetTrigger("Hit");
        StopCoroutine("hitColorChangeCoroutine");
        StartCoroutine("hitColorChangeCoroutine");

        if (hp < 0)
        {
            Destroy(gameObject);
        }
    }
}
