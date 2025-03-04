using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThornsAttack : MonoBehaviour
{
    private BoxCollider2D boxColl;
    private Animator anim;

    private Collider2D coll;
    private float timer;
    private bool isAttack;

    private Vector2 playerPos;
    public Vector2 PlayerPos { get { return playerPos; } set { playerPos = value; } }

    private void Awake()
    {
        TryGetComponent(out boxColl);
        TryGetComponent(out anim);
    }

    private void Start()
    {
        transform.position = playerPos;
    }

    private void Update()
    {
        coll = Physics2D.OverlapBox(boxColl.bounds.center, boxColl.bounds.size, 0.0f,
            LayerMask.GetMask("Player"));
        timer += Time.deltaTime;
        if (timer >= 3f && !isAttack)
        {
            isAttack = true;
            anim.SetBool("isAttack", isAttack);
        }
    }

    private void LateUpdate()
    {
        RaycastHit2D hitA = Physics2D.Raycast(transform.position, Vector2.down, 30f, LayerMask.GetMask("Footboard"));
        if (hitA.collider != null)
        {
            if (hitA.collider.gameObject.layer.Equals(LayerMask.NameToLayer("Footboard")))
            {
                Vector2 pos = transform.position;
                pos.y = hitA.collider.bounds.max.y + 0.1f;
                transform.position = pos;
                return;
            }
        }

        RaycastHit2D hitB = Physics2D.Raycast(transform.position, Vector2.down, 30f, LayerMask.GetMask("Ground"));
        if (hitB.collider != null)
        {
            if (hitB.collider.gameObject.layer.Equals(LayerMask.NameToLayer("Ground")))
            {
                Vector2 pos = transform.position;
                pos.y = hitB.collider.bounds.max.y + 0.1f;
                transform.position = pos;
            }
        }
    }

    public void Attack()
    {
        if (coll == null)
        {
            return;
        }

        if (coll.gameObject.layer.Equals(LayerMask.NameToLayer("Player")))
        {
            coll.GetComponent<PlayerController>().Hit(10, new Vector2(0f, 10f));
        }
    }

    public void AttakcDestroy()
    {
        Destroy(gameObject);
    }
}
