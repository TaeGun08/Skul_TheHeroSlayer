using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashPrefab : MonoBehaviour
{
    private SpriteRenderer spriteRen;
    [SerializeField] private Sprite sprite;
    public Sprite Sprite { set { sprite = value; } }

    private bool isDash;
    public bool IsDash { set { isDash = value; } }

    private void Awake()
    {
        TryGetComponent(out spriteRen);
        gameObject.SetActive(false);
    }

    private void LateUpdate()
    {
        if (isDash)
        {
            StopCoroutine("transparent");
            StartCoroutine("transparent");
            isDash = false;
        }
    }

    private IEnumerator transparent()
    {
        spriteRen.sprite = sprite;
        WaitForSeconds wfs = new WaitForSeconds(0.01f);
        Color colorAp = spriteRen.color;
        while (colorAp.a > 0f)
        {
            colorAp.a -= Time.deltaTime;
            spriteRen.color = colorAp;
            yield return wfs;
        }

        gameObject.SetActive(false);
        colorAp.a = 1f;
        spriteRen.color = colorAp;
        yield return null;
    }
}
