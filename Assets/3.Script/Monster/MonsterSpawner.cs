using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    [Header("몬스터 스포너")]
    [SerializeField] private GameObject[] monsterPrefabs;
    [SerializeField] private Transform[] spawnTrs;
    [SerializeField] private BoxCollider2D openNextScene;
    private List<GameObject> monsters = new List<GameObject>();
    public List<GameObject> Monsters { get { return monsters; } }

    private void Awake()
    {
        for (int i = 0; i < spawnTrs.Length; i++)
        {
            int ran = Random.Range(0, monsterPrefabs.Length);
            monsters.Add(Instantiate(monsterPrefabs[ran], spawnTrs[i].position, Quaternion.identity, transform));
        }

        StartCoroutine(monsterCheckCoroutine());
    }

    private IEnumerator monsterCheckCoroutine()
    {
        WaitForSeconds wfs = new WaitForSeconds(2f);
        int count = 0;
        while (true)
        {
            for (int i = 0; i < monsters.Count; i++)
            {
                if (monsters[i] == null)
                {
                    count++;
                }
            }

            if (count >= monsters.Count)
            {
                openNextScene.enabled = true;
                break;
            }

            count = 0;
            yield return wfs;
        }
    }
}
