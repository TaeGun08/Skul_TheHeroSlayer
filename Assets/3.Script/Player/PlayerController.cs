using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerInput input;
    private PlayerAnimation anim;

    private void Start()
    {
        TryGetComponent(out input);
        TryGetComponent(out anim);
    }

    private void Update()
    {
        
    }
}
