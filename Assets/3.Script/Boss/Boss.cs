using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BossPatternState
{
    Appearance,
    Inactive,
    Active,
}

public abstract class Boss : MonoBehaviour, Hit
{
    protected Rigidbody2D rigid;
    protected Animator anim;
    protected Gravity gravity;

    [Header("º¸½º")]
    [SerializeField] protected int hp;
    [SerializeField] protected int hasPattern;
    [SerializeField] protected int patternNumber;
    [SerializeField] protected int phase;
    [SerializeField] protected BossPatternState state;

    protected virtual void Awake()
    {
        TryGetComponent(out rigid);
        TryGetComponent(out anim);
        TryGetComponent(out gravity);
    }

    protected abstract void bossPattern();

    public abstract void Hit(int _damage, Vector2 _knockback);
}
