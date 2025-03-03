using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpBarCanvas : MonoBehaviour
{
    private void LateUpdate()
    {
        if (transform.parent.localScale.x < 0)
        {
            transform.localScale = new Vector2(-1f, 1f);
        }
        else
        {
            transform.localScale = new Vector2(1f, 1f);
        }
    }
}
