using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemTier
{
    Legendery,
    Unique,
    Rare,
    Common,
}

public abstract class Item : MonoBehaviour
{
    protected GameManager gameManager;

    protected Rigidbody2D rigid;
    protected Collider2D coll;
    protected SpriteRenderer spriteRen;

    [Header("æ∆¿Ã≈€")]
    [SerializeField] protected int index;
    public int Index { get { return index; } }
    [SerializeField] protected ItemTier itemTier;
    [SerializeField] protected Sprite itmeIcon;
    public Sprite ItemIcon { get { return ItemIcon; } set { itmeIcon = value; } }
    [SerializeField] protected Sprite itmeImage;
    public Sprite ItmeImage { get { return itmeImage; } set { itmeImage = value; } }

    protected virtual void Awake()
    {
        TryGetComponent(out rigid);
        TryGetComponent(out coll);
        TryGetComponent(out spriteRen);
    }

    protected virtual void Start()
    {
        gameManager = GameManager.Instance;
    }

    public abstract void ItemAbility();

    public virtual void Hide()
    {
        rigid.gravityScale = 0f;
        coll.enabled = false;
        spriteRen.enabled = false;
    }
}
