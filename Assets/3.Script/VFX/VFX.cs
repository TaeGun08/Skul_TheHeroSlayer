using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFX : MonoBehaviour
{
    private Animator anim;

    private Vector2 scale;
    public Vector2 Scale { set { scale = value; } }

    private void OnEnable()
    {
        anim.Play("Idle");
    }

    private void Awake()
    {
        TryGetComponent(out anim);
    }

    private void LateUpdate()
    {
        transform.localScale = scale;
    }

    public void ActiveFalse()
    {
        gameObject.SetActive(false);
    }

    public void DestroyOn()
    {
        Destroy(gameObject);
    }
}
