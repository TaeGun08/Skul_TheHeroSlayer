using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitCheck : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer.Equals(LayerMask.NameToLayer("Player")))
        {
            Destroy(gameObject);
        }
        else if (collision.gameObject.layer.Equals(LayerMask.NameToLayer("Ground"))
            || collision.gameObject.layer.Equals(LayerMask.NameToLayer("Wall"))
            || collision.gameObject.layer.Equals(LayerMask.NameToLayer("Footboard")))
        {
            Destroy(gameObject);
        }
    }
}
