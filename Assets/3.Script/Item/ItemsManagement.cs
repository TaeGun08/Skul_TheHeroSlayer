using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsManagement : MonoBehaviour
{
    [Header("�����۵�")]
    [SerializeField] private Item[] items;
    public Item[] Items { get { return items; } }
}
