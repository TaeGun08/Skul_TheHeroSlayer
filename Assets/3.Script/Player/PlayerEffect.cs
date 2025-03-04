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
    [SerializeField] private VFX[] vfxs;
    private VFX jump;
    public VFX Jump { get { return jump; } set { jump = value; } }
    private VFX[] dash = new VFX[2];
    public VFX[] Dash { get { return dash; } set { dash = value; } }

    private void Awake()
    {
        for (int i = 0; i < 14; i++)
        {
            dashPool.Add(Instantiate(dashPrefab, transform));
        }

        jump = Instantiate(vfxs[0], transform);
        dash[0] = Instantiate(vfxs[1], transform);
        dash[1] = Instantiate(vfxs[1], transform);
        jump.Scale = transform.localScale;
        dash[0].Scale = transform.localScale;
        dash[1].Scale = transform.localScale;
        jump.gameObject.SetActive(false);
        dash[0].gameObject.SetActive(false);
        dash[1].gameObject.SetActive(false);
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
