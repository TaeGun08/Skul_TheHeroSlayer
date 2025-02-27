using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StateEnum
{
    Idle,
    Walk,
    Attack,
}

public class State : MonoBehaviour
{
    [Header("����")]
    [SerializeField] private StateEnum stateEnum;
    public StateEnum StateEnum { get { return stateEnum; } set { stateEnum = value; } }
}
