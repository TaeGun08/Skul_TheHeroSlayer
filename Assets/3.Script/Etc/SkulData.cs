using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkulA
{
    public int skulIndex;
    public int rank;
}

public class SkulB
{
    public int skulIndex;
    public int rank;
}

public class SkulData : MonoBehaviour
{
    [Header("Skuls")]
    [SerializeField] private List<GameObject> skuls;
    [SerializeField] private List<GameObject> skullCount;

    private void Start()
    {
        Instantiate(skuls[0], transform);
    }
}
