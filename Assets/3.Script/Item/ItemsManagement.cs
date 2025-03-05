using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsManagement : MonoBehaviour
{
    [Header("아이템들")]
    [SerializeField] private Item[] items;
    public Item[] Items { get { return items; } }
}
