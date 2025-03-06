using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstHero : Boss
{
    public override void Hit(int _damage)
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

    protected override void bossPattern()
    {
        throw new System.NotImplementedException();
    }
}
