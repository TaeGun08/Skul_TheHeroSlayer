using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BossPatternState
{
    Inactive,
    Active,
}

public abstract class Boss : MonoBehaviour, Hit
{
    protected Animator anim;

    [Header("º¸½º")]
    [SerializeField] protected int hp;
    [SerializeField] protected int hasPattern;
    [SerializeField] protected int patternNumber;
    [SerializeField] protected BossPatternState state;

    protected virtual void Awake()
    {
        TryGetComponent(out anim);
    }

    protected abstract void bossPattern();

    public abstract void Hit(int _damage, Vector2 _knockback);
}
