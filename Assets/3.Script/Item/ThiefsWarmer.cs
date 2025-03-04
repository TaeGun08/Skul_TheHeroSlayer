using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThiefsWarmer : Item
{
    [Header("도적 보호대")]
    [SerializeField] private GameObject shuriken;

    public override void ItemAbility(PlayerStatus _playerStatus, GameObject _skul)
    {

    }
}
