using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffect : MonoBehaviour
{
    private GameManager gameManager;

    private SpriteRenderer spriteRen;
    public SpriteRenderer SpriteRen { set { spriteRen = value; } }

    [Header("VFX")]
    [SerializeField] private DashPrefab dashPrefab;
    private List<DashPrefab> dashPool = new List<DashPrefab>();

    private void Awake()
    {
        for (int i = 0; i < 14; i++)
        {
            dashPool.Add(Instantiate(dashPrefab, transform));
        }
    }

    private void Start()
    {
        gameManager = GameManager.Instance;
    }

    private void LateUpdate()
    {
        if (spriteRen == null && gameManager.OnSkul != null)
        {
            gameManager.OnSkul.TryGetComponent(out spriteRen);
        }
    }

    private IEnumerator DashVFX()
    {
        WaitForSeconds wfs = new WaitForSeconds(0.02f);
        int count = 0;
        for (int i = 0; i < dashPool.Count; i++)
        {
            if (!dashPool[i].gameObject.activeSelf && count < 7)
            {
                if (spriteRen != null)
                {
                    dashPool[i].gameObject.SetActive(true);
                    dashPool[i].Sprite = spriteRen.sprite;
                    dashPool[i].IsDash = true;
                    dashPool[i].transform.position = spriteRen.transform.position;
                    dashPool[i].transform.localScale = spriteRen.transform.localScale;
                    count++;
                }
                yield return wfs;
            }
        }
    }

    public void StopDashVFX()
    {
        StopCoroutine("DashVFX");
    }

    public void StartDashVFX()
    {
        StartCoroutine("DashVFX");
    }
}
