using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireWizard : Monster
{
    [Header("파이어 위자드")]
    [SerializeField] private GameObject fireBall;
    [SerializeField] private Transform spawnFireBallPos;
    [SerializeField] private CircleCollider2D playerCheckColl; 
    [SerializeField] private float attackCoolTime;
    private int fireBallCount;
    private Coroutine shootFireBallCo;

    private void Update()
    {
        hitCollCheck();
        anim.SetBool("isAttack", isAttack);

        if (isHit && shootFireBallCo != null)
        {
            StopCoroutine(shootFireBallCo);
            shootFireBallCo = null;
        }
    }

    private IEnumerator shootFireBallCoroutine(Transform _targetTrs)
    {
        yield return new WaitForSeconds(2f);
        float timer = 0f;
        float delayTimer = 0f;
        WaitForSeconds wfs = new WaitForSeconds(0.01f);
        while (isAttack)
        {
            if (fireBallCount < 3)
            {
                delayTimer += Time.deltaTime;
                if (delayTimer >= 0.5f)
                {
                    FireBall fire = Instantiate(fireBall, spawnFireBallPos.position, Quaternion.identity).GetComponent<FireBall>();
                    fire.SetTarget(_targetTrs);
                    fireBallCount++;
                    delayTimer = 0f;
                }
            }
            else
            {
                timer += Time.deltaTime;
                if (timer >= attackCoolTime)
                {
                    timer = 0f;
                    fireBallCount = 0;
                }
            }
            yield return wfs;
        }

        yield return null;
    } 

    private void hitCollCheck()
    {
        Collider2D collider = Physics2D.OverlapCircle(playerCheckColl.bounds.center, playerCheckColl.radius, 
            LayerMask.GetMask("Player"));
        attack(collider);
    }

    protected override void attack(Collider2D _collider = null)
    {
        if (_collider == null)
        {
            return;
        }

        if (!isAttack && !isHit
            && _collider.gameObject.layer.Equals(LayerMask.NameToLayer("Player")))
        {
            isAttack = true;
            anim.SetBool("isAttackReady", true);
            shootFireBallCo = StartCoroutine(shootFireBallCoroutine(_collider.transform));
        }
    }

    public override void Hit(int _damage, Vector2 _knockback)
    {
        base.Hit(_damage, _knockback);
        isHit = true;
    }
}
