using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitCheck : MonoBehaviour
{
    private GameManager gameManager;
    private PlayerAttack playerAttack;

    [Header("히트 체크")]
    [SerializeField] private float damage;
    [SerializeField] private bool playerOrMonsterHit;
    [SerializeField] private bool disposable;
    [SerializeField] private bool poolOn;
    [SerializeField] private VFX vfx;

    private void Start()
    {
        gameManager = GameManager.Instance;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer.Equals(LayerMask.NameToLayer("Player")) && !playerOrMonsterHit)
        {
            PlayerController playerSc = collision.GetComponent<PlayerController>();
            playerSc.Hit((int)damage, new Vector2(0f, 3f));

            if (poolOn)
            {
                gameObject.SetActive(false);
            }

            if (disposable)
            {
                Destroy(gameObject);
            }
        }
        else if (collision.gameObject.layer.Equals(LayerMask.NameToLayer("Monster")) && playerOrMonsterHit)
        {
            playerAttack = gameManager.OnSkul.GetComponent<PlayerAttack>();
            collision.GetComponent<Monster>().Hit((int)(playerAttack.Damage * damage), Vector2.zero);
            Instantiate(vfx, collision.transform.position, Quaternion.identity).GetComponent<VFX>().Scale = -transform.localScale;

            if (poolOn)
            {
                gameObject.SetActive(false);
            }

            if (disposable)
            {
                Destroy(gameObject);
            }
        }
        else if (collision.gameObject.layer.Equals(LayerMask.NameToLayer("Ground"))
            || collision.gameObject.layer.Equals(LayerMask.NameToLayer("Wall")))
        {
            if (poolOn)
            {
                gameObject.SetActive(false);
            }

            if (disposable)
            {
                Destroy(gameObject);
            }
        }
    }
}
