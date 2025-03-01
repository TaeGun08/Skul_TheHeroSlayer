using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveRotate : MonoBehaviour
{
    [Header("È¸Àü")]
    [SerializeField] private float speed;
    public float Speed { set { speed = value; } }

    private void FixedUpdate()
    {
        if (gameObject.activeSelf)
        {
            transform.eulerAngles += new Vector3(0f, 0f, speed);
        }
    }
}
