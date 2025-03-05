using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenDagger : Item
{
    [Header("히든대거")]
    [SerializeField] private GameObject projectile;
    private List<GameObject> projectilePool = new List<GameObject>();

    private void pooling()
    {
        if (!projectilePool.Count.Equals(0))
        {
            for (int i =0; i < projectilePool.Count; i++)
            {
                if (!projectilePool[i].activeSelf)
                {
                    projectilePool[i].SetActive(true);
                    projectilePool[i].transform.localScale = gameManager.OnSkul.transform.localScale;
                    projectilePool[i].transform.position += new Vector3(gameManager.OnSkul.transform.localScale.x, 0f, 0f);
                    projectilePool[i].transform.position = gameManager.OnSkul.transform.position;
                    MoveDirection moveDirA = projectilePool[i].GetComponent<MoveDirection>();

                    moveDirA.MoveDir = new Vector2(gameManager.OnSkul.transform.localScale.x * 15f, 0f);
                    return;
                }
            }
        }

        MoveDirection moveDirB = Instantiate(projectile, gameManager.OnSkul.transform.position,
           Quaternion.identity, transform).GetComponent<MoveDirection>();

        moveDirB.MoveDir = new Vector2(gameManager.OnSkul.transform.localScale.x * 15f, 0f);
        moveDirB.gameObject.transform.localScale = gameManager.OnSkul.transform.localScale;
        moveDirB.gameObject.transform.position += new Vector3(gameManager.OnSkul.transform.localScale.x, 0f, 0f);

        projectilePool.Add(moveDirB.gameObject);
    }

    public override void ItemAbility()
    {
        pooling();
    }
}
