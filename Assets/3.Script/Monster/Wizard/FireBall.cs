using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    private Rigidbody2D rigid;
    [SerializeField] private float speed;

    private void Awake()
    {
        TryGetComponent(out rigid);
    }

    public void SetTarget(Transform _targetTrs)
    {
        Vector3 direction = (_targetTrs.position - transform.position).normalized;
        rigid.velocity = direction * speed;
    }
}
