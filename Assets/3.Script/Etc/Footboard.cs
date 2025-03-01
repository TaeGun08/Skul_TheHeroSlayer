using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Footboard : MonoBehaviour
{
    private PlatformEffector2D platformEffector;
    private BoxCollider2D boxColl;

    private void Awake()
    {
        TryGetComponent(out platformEffector);
        TryGetComponent(out boxColl);
    }

    private IEnumerator footbaordOffCoroutine(Collider2D _collider)
    {
        Physics2D.IgnoreCollision(boxColl, _collider, true);
        yield return new WaitForSeconds(0.5f);
        Physics2D.IgnoreCollision(boxColl, _collider, false);
    }

    public void FootbaordOff(Collider2D _collider)
    {
        StartCoroutine(footbaordOffCoroutine(_collider));
    }
}
