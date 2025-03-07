using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPos : MonoBehaviour
{
    [Header("리스폰 위치")]
    [SerializeField] private Transform spawnTrs;
    public Transform SpawnTrs { get { return spawnTrs; } set { spawnTrs = value; } }
}
