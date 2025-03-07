using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstHero : Boss
{
    private GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.Instance;

        StartCoroutine("appearance");
    }

    private void Update()
    {
        if (state.Equals(BossPatternState.Inactive))
        {
            int ranPattern = Random.Range(0, hasPattern);
            state = BossPatternState.Active;
            bossPattern();
        }
    }

    protected override void bossPattern()
    {

    }

    private IEnumerator appearance()
    {
        yield return new WaitForSeconds(2f);
        anim.SetFloat("Appearance", 1);
    }

    public override void Hit(int _damage, Vector2 _knockback)
    {
        hp -= _damage;

        if (hp <= 0)
        {
            if (patternNumber.Equals(0))
            {
                hp = 1000;
                patternNumber++;
                Debug.Log("1Æä ³¡");
            }
            else
            {
                Debug.Log("Á×¾î¥’");
            }
        }
    }
}
