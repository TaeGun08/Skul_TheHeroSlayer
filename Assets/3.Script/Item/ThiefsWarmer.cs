using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThiefsWarmer : Item
{
    [Header("µµ¿˚ ∆»≈‰Ω√")]
    [SerializeField] private GameObject shuriken;
    private List<GameObject> shurikensPool = new List<GameObject>();

    private void pooling()
    {
        if (!shurikensPool.Count.Equals(0))
        {
            for (int i = 0; i < shurikensPool.Count; i++)
            {
                if (!shurikensPool[i].activeSelf)
                {
                    shurikensPool[i].SetActive(true);
                    shurikensPool[i].transform.localScale = gameManager.OnSkul.transform.localScale;
                    shurikensPool[i].transform.position += new Vector3(gameManager.OnSkul.transform.localScale.x, 0f, 0f);
                    shurikensPool[i].transform.position = gameManager.OnSkul.transform.position;

                    MoveDirection moveDirA = shurikensPool[i].GetComponent<MoveDirection>();
                    moveDirA.MoveDir = new Vector2(gameManager.OnSkul.transform.localScale.x * 6f, 0f);
                    return;
                }
            }
        }

        MoveDirection moveDirB = Instantiate(shuriken, gameManager.OnSkul.transform.position,
            Quaternion.identity, transform).GetComponent<MoveDirection>();
        moveDirB.MoveDir = new Vector2(gameManager.OnSkul.transform.localScale.x * 6f, 0f);

        shurikensPool.Add(moveDirB.gameObject);
    }

    public override void ItemAbility()
    {
        pooling();
    }
}
