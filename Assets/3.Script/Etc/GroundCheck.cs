using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    private BoxCollider2D boxColl;

    [SerializeField] private bool ground;
    public bool Ground => ground;

    private void Awake()
    {
        TryGetComponent(out boxColl);
    }

    private void LateUpdate()
    {
        groundCheck();
    }

    private void groundCheck()
    {
        if (Physics2D.BoxCast(boxColl.bounds.center, boxColl.bounds.size, 0.0f, Vector2.zero, 0.0f, LayerMask.GetMask("Ground")))
        {
            ground = true;
        }
        else
        {
            ground = false;
        }
    }
}
