using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StateEnum
{
    Idle,
    Walk,
    Jump,
    Fall,
    Dash,
    Attack,
    JumpAttack,
    SkillAttack,
    SwitchAttack,
}

public class State : MonoBehaviour
{
    [Header("ป๓ลย")]
    [SerializeField] private StateEnum stateEnum;
    public StateEnum StateEnum => stateEnum;

    public void SetStateEnum(StateEnum _stateEnum, bool _isGround)
    {
        if (_isGround.Equals(false) 
            && (_stateEnum.Equals(StateEnum.Walk) || _stateEnum.Equals(StateEnum.Idle)))
        {
            return;
        }

        if (stateEnum.Equals(StateEnum.Jump)
            && (_stateEnum.Equals(StateEnum.Walk) || _stateEnum.Equals(StateEnum.Idle)))
        {
            return;
        }

        stateEnum = _stateEnum;
    }
}
