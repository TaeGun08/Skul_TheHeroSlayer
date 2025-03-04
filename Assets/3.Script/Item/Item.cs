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
    [Header("æ∆¿Ã≈€")]
    [SerializeField] protected ItemTier itemTier;
    [SerializeField] protected Sprite itmeIcon;
    public Sprite ItemIcon { get { return ItemIcon; } set { itmeIcon = value; } }
    [SerializeField] protected Sprite itmeImage;
    public Sprite ItmeImage { get { return itmeImage; } set { itmeImage = value; } }
    protected bool itemAbilityOn;
    public bool ItemAbilityOn { get { return itemAbilityOn;  } set { itemAbilityOn = value; } }

    public abstract void ItemAbility(PlayerStatus _playerStatus, GameObject _skul);
}
