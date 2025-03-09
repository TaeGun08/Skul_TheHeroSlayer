using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    private GameManager gameManager;

    [Header("¸Ê ÄÁÆ®·Ñ·¯")]
    [SerializeField] private GameObject marker;
    [SerializeField] private GameObject monsterMarker;
    [SerializeField] private MonsterSpawner monsterSpawner;
    private GameObject playerMarker;
    private List<GameObject> monsterMarkers = new List<GameObject>();

    private void Start()
    {
        gameManager = GameManager.Instance;
    }

    private void LateUpdate()
    {
        if (gameManager.OnSkul != null && playerMarker == null)
        {
            playerMarker = Instantiate(marker, gameManager.OnSkul.transform.position, Quaternion.identity);
        }

        if (playerMarker != null)
        {
            Vector3 pos = gameManager.OnSkul.transform.position;
            pos.z = 40f;
            playerMarker.transform.position = pos;
        }

        if (monsterSpawner == null)
        {
            return;
        }

        if (!monsterSpawner.Monsters.Count.Equals(0) && monsterMarkers.Count.Equals(0))
        {
            for (int i = 0; i < monsterSpawner.Monsters.Count; i++)
            {
                monsterMarkers.Add(Instantiate(monsterMarker,
                    monsterSpawner.Monsters[i].transform.position, Quaternion.identity));
            }
        }

        if (!monsterMarkers.Count.Equals(0))
        {
            for (int i = 0; i < monsterSpawner.Monsters.Count; i++)
            {
                if (monsterSpawner.Monsters[i] == null && monsterMarkers[i] != null)
                {
                    Destroy(monsterMarkers[i]);
                    break;
                }

                if (monsterMarkers[i] != null)
                {
                    Vector3 pos = monsterSpawner.Monsters[i].transform.position;
                    pos.z = 40f;
                    monsterMarkers[i].transform.position = pos;
                }
            }
        }
    }
} 
