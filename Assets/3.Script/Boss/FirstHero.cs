using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FirstHero : Boss
{
    private GameManager gameManager;
    private Fade fade;

    [Header("ÃÊ´ë ¿ë»ç")]
    [SerializeField] private float patternDelayTime;
    [SerializeField] private BoxCollider2D[] hitBox;
    [SerializeField] private BoxCollider2D[] barricade;
    [SerializeField] private GameObject[] vfxPrefabs;
    [SerializeField] private GameObject[] attackPrefabs;
    [SerializeField] private Image hpBar;
    [SerializeField] private GameObject[] maps;
    private int maxHp;
    private int damage;
    private int hitBoxIndex;
    private float patternDelay;
    private Vector3 scale;
    private bool isInvincible;
    private float darkBangDirX;

    private void Start()
    {
        gameManager = GameManager.Instance;
        fade = Fade.Instance;

        StartCoroutine("appearance");

        patternDelay = patternDelayTime;
        scale = transform.localScale;

        Instantiate(vfxPrefabs[0], new Vector3(transform.position.x, 2f, 0f), Quaternion.identity);
        isInvincible = true;
        maxHp = hp;
    }

    private void Update()
    {
        if (gameManager.OnSkul == null)
        {
            return;
        }

        if (state.Equals(BossPatternState.Appearance))
        {
            return;
        }

        if (distanceX(transform.position, gameManager.OnSkul.transform.position) <= 0.1f
         && distanceX(transform.position, gameManager.OnSkul.transform.position) >= -0.1f
         && !patternNumber.Equals(1))
        {
            rigid.velocity = new Vector2(0f, rigid.velocity.y);
        }

        if (state.Equals(BossPatternState.Inactive) && patternDelay > 0)
        {
            patternDelay -= Time.deltaTime;
        }

        if (state.Equals(BossPatternState.Inactive) && patternDelay <= 0)
        {
            state = BossPatternState.Active;
            bossPattern();
        }
    }

    protected override void bossPattern()
    {
        int ranPattern = 0;
        anim.SetTrigger("Attack");
        anim.SetBool("IsActive", true);
        if (phase.Equals(0))
        {
            ranPattern = Random.Range(0, hasPattern);
            anim.SetInteger("PatternNumber", ranPattern);
            commonPattern(ranPattern);
        }
        else
        {
            ranPattern = Random.Range(0, 2);
            anim.SetInteger("PatternNumber", ranPattern);
            darkPattern(ranPattern);
        }
    }

    private void commonPattern(int _pattern)
    {
        switch (_pattern)
        {
            case 0:
                StartCoroutine(slash());
                damage = 12;
                hitBoxIndex = 0;
                break;
            case 1:
                StartCoroutine(backDash());
                break;
            case 2:
                StartCoroutine(dash());
                break;
            case 3:
                if (directionX(transform.position, gameManager.OnSkul.transform.position) > 0)
                {
                    flip(1);
                }
                else
                {
                    flip(-1);
                }
                damage = 12;
                hitBoxIndex = 1;
                break;
            case 4:
                StartCoroutine(takeItDown());
                damage = 20;
                hitBoxIndex = 2;
                break;
            case 5:
                StartCoroutine(slash());
                damage = 8;
                hitBoxIndex = 3;
                break;
            case 6:
                StartCoroutine(comboSlash());
                damage = 5;
                hitBoxIndex = 4;
                break;
        }
    }

    private void darkPattern(int _pattern)
    {
        anim.SetInteger("PatternNumber", _pattern);
        int ran = Random.Range(0, 2);
        switch (_pattern)
        {
            case 0:
                damage = 15;
                if (ran.Equals(0))
                {
                    StartCoroutine(wallDashTakeItDownA());
                }
                else
                {
                    StartCoroutine(wallDashTakeItDownB());
                }
                break;
            case 1:
                damage = 15;
                if (ran.Equals(0))
                {
                    StartCoroutine(darkBangA());
                }
                else
                {
                    StartCoroutine(darkBangB());
                }
                break;
        }
    }

    private float directionX(Vector3 _my, Vector3 _target)
    {
        Vector3 dirVec = (_target - _my).normalized;
        return dirVec.x;
    }

    private float distanceX(Vector3 _my, Vector3 _target)
    {
        _my.y = 0f;
        _target.y = 0f;
        float distance =  Vector2.Distance(_my, _target);
        return distance;
    }

    private void flip(float _scaleX)
    {
        scale.x = _scaleX;
        transform.localScale = scale;
    }

    private IEnumerator appearance()
    {
        yield return new WaitForSeconds(2f);
        anim.SetFloat("Idle", 1);
        state = BossPatternState.Inactive;
        isInvincible = false;
    }

    private IEnumerator changeform()
    {
        yield return new WaitForSeconds(5.5f);
        maps[0].SetActive(false);
        maps[1].SetActive(true);
        anim.SetFloat("Idle", 1);
        state = BossPatternState.Inactive;
        isInvincible = false;
    }

    #region ÃÊ´ë¿ë»ç ÈæÈ­ Àü ÆÐÅÏ
    private IEnumerator slash()
    {
        rigid.velocity = Vector2.zero;
        yield return new WaitForSeconds(0.5f);
        if (directionX(transform.position, gameManager.OnSkul.transform.position) > 0)
        {
            rigid.velocity += new Vector2(30f, 0f);
            flip(1);
        }
        else
        {
            rigid.velocity += new Vector2(-30f, 0f);
            flip(-1);
        }
        yield return null;
        yield return new WaitForSeconds(0.1f);
        rigid.velocity = Vector2.zero;
    }

    private IEnumerator backDash()
    {
        if (directionX(transform.position, gameManager.OnSkul.transform.position) > 0)
        {
            rigid.velocity += new Vector2(-30f, 0f);
            flip(1);
        }
        else
        {
            rigid.velocity += new Vector2(30f, 0f);
            flip(-1);
        }
        yield return null;
        yield return new WaitForSeconds(0.05f);
        rigid.velocity = Vector2.zero;
    }

    private IEnumerator dash()
    {
        yield return new WaitForSeconds(0.7f);
        if (directionX(transform.position, gameManager.OnSkul.transform.position) > 0)
        {
            rigid.velocity += new Vector2(30f, 0f);
            flip(1);
        }
        else
        {
            rigid.velocity += new Vector2(-30f, 0f);
            flip(-1);
        }
        yield return null;
        yield return new WaitForSeconds(0.1f);
        rigid.velocity = Vector2.zero;
    }

    private IEnumerator takeItDown()
    {
        gravity.enabled = false;
        if (directionX(transform.position, gameManager.OnSkul.transform.position) > 0)
        {
            rigid.velocity += new Vector2(10f, 5f);
            flip(1);
        }
        else
        {
            rigid.velocity += new Vector2(-10f, 5f);
            flip(-1);
        }
        yield return null;
        yield return new WaitForSeconds(0.5f);
        rigid.velocity = Vector2.zero;
        rigid.velocity += new Vector2(0, -30f);
        yield return new WaitForSeconds(0.1f);
        gravity.enabled = true;
    }

    private IEnumerator comboSlash()
    {
        yield return new WaitForSeconds(3f);
        EndPattern();
    }
    #endregion

    #region ÃÊ´ë¿ë»ç ÈæÈ­ ÈÄ ÆÐÅÏ
    private IEnumerator wallDashTakeItDownA()
    {
        hitBoxIndex = 4;
        isInvincible = true;
        gravity.enabled = false;
        Vector2 dir = (gameManager.OnSkul.transform.position - transform.position).normalized;
        Vector3 downPos;
        float posY = 0f;
        posY = Random.Range(-1f, 2.8f);
        transform.position = new Vector3(barricade[0].bounds.max.x, posY, 0f); //º®
        flip(1);
        yield return new WaitForSeconds(1.3f);
        downPos = new Vector3(gameManager.OnSkul.transform.position.x, -2.5f, 0f);
        yield return new WaitForSeconds(0.3f);
        transform.position = downPos; //Âï
        if (directionX(transform.position, gameManager.OnSkul.transform.position) > 0)
        {
            flip(1);
        }
        else
        {
            flip(-1);
        }
        yield return new WaitForSeconds(0.9f);
        posY = Random.Range(-1f, 2.8f);
        transform.position = new Vector3(barricade[1].bounds.min.x, posY, 0f); //º®
        flip(-1);
        yield return new WaitForSeconds(1.3f);
        downPos = new Vector3(gameManager.OnSkul.transform.position.x, -2.5f, 0f);
        yield return new WaitForSeconds(0.3f);
        transform.position = downPos; //Âï
        if (directionX(transform.position, gameManager.OnSkul.transform.position) > 0)
        {
            flip(1);
        }
        else
        {
            flip(-1);
        }
        yield return new WaitForSeconds(0.8f);
        posY = Random.Range(-1f, 2.8f);
        transform.position = new Vector3(barricade[0].bounds.max.x, posY, 0f); //º®
        flip(1);
        yield return new WaitForSeconds(1.3f);
        posY = Random.Range(-1f, 2.8f);
        transform.position = new Vector3(barricade[1].bounds.min.x, posY, 0f); //º®
        flip(-1);
        yield return new WaitForSeconds(1.0f);
        downPos = new Vector3(gameManager.OnSkul.transform.position.x, -2.5f, 0f);
        yield return new WaitForSeconds(0.3f);
        transform.position = downPos; //Âï
        if (directionX(transform.position, gameManager.OnSkul.transform.position) > 0)
        {
            flip(1);
        }
        else
        {
            flip(-1);
        }
        yield return new WaitForSeconds(0.5f);
        EndPattern();
    }

    private IEnumerator wallDashTakeItDownB()
    {
        hitBoxIndex = 4;
        isInvincible = true;
        gravity.enabled = false;
        Vector2 dir = (gameManager.OnSkul.transform.position - transform.position).normalized;
        Vector3 downPos;
        float posY = 0f;
        posY = Random.Range(-1f, 2.8f);
        transform.position = new Vector3(barricade[1].bounds.min.x, posY, 0f); //º®
        flip(-1);
        yield return new WaitForSeconds(1.3f);
        downPos = new Vector3(gameManager.OnSkul.transform.position.x, -2.5f, 0f);
        yield return new WaitForSeconds(0.3f);
        transform.position = downPos; //Âï
        if (directionX(transform.position, gameManager.OnSkul.transform.position) > 0)
        {
            flip(1);
        }
        else
        {
            flip(-1);
        }
        yield return new WaitForSeconds(0.9f);
        posY = Random.Range(-1f, 2.8f);
        transform.position = new Vector3(barricade[0].bounds.max.x, posY, 0f); //º®
        flip(1);
        yield return new WaitForSeconds(1.3f);
        downPos = new Vector3(gameManager.OnSkul.transform.position.x, -2.5f, 0f);
        yield return new WaitForSeconds(0.3f);
        transform.position = downPos; //Âï
        if (directionX(transform.position, gameManager.OnSkul.transform.position) > 0)
        {
            flip(1);
        }
        else
        {
            flip(-1);
        }
        yield return new WaitForSeconds(0.8f);
        posY = Random.Range(-1f, 2.8f);
        transform.position = new Vector3(barricade[1].bounds.min.x, posY, 0f); //º®
        flip(-1);
        yield return new WaitForSeconds(1.3f);
        posY = Random.Range(-1f, 2.8f);
        transform.position = new Vector3(barricade[0].bounds.max.x, posY, 0f); //º®
        flip(1);
        yield return new WaitForSeconds(1.0f);
        downPos = new Vector3(gameManager.OnSkul.transform.position.x, -2.5f, 0f);
        yield return new WaitForSeconds(0.3f);
        transform.position = downPos; //Âï
        if (directionX(transform.position, gameManager.OnSkul.transform.position) > 0)
        {
            flip(1);
        }
        else
        {
            flip(-1);
        }
        yield return new WaitForSeconds(0.5f);
        EndPattern();
    }

    public IEnumerator darkBangA()
    {
        isInvincible = true;
        hitBoxIndex = 4;
        darkBangDirX = 1;
        float posY = 0f;
        Vector3 downPos;
        posY = Random.Range(-1f, 2.8f);
        transform.position = new Vector3(barricade[0].bounds.max.x, posY, 0f); //º®
        flip(1);
        gravity.enabled = false;
        yield return new WaitForSeconds(5.5f);
        downPos = new Vector3(gameManager.OnSkul.transform.position.x, -2.5f, 0f);
        yield return new WaitForSeconds(0.3f);
        transform.position = downPos; //Âï
        if (directionX(transform.position, gameManager.OnSkul.transform.position) > 0)
        {
            flip(1);
        }
        else
        {
            flip(-1);
        }
        yield return new WaitForSeconds(0.8f);
        gravity.enabled = true;
        isInvincible = false;
        EndPattern();
    }

    public IEnumerator darkBangB()
    {
        isInvincible = true;
        hitBoxIndex = 4;
        darkBangDirX = -1;
        float posY = 0f;
        Vector3 downPos;
        posY = Random.Range(-1f, 2.8f);
        transform.position = new Vector3(barricade[1].bounds.min.x, posY, 0f); //º®
        flip(-1);
        gravity.enabled = false;
        yield return new WaitForSeconds(5.5f);
        downPos = new Vector3(gameManager.OnSkul.transform.position.x, -2.5f, 0f);
        yield return new WaitForSeconds(0.3f);
        transform.position = downPos; //Âï
        if (directionX(transform.position, gameManager.OnSkul.transform.position) > 0)
        {
            flip(1);
        }
        else
        {
            flip(-1);
        }
        yield return new WaitForSeconds(0.8f);
        gravity.enabled = true;
        isInvincible = false;
        EndPattern();
    }

    public void DarkBang()
    {
        float dirY =14;
        for (int i = 0; i < 9; i++)
        {
            Rigidbody2D rigidBang = Instantiate(attackPrefabs[0],
                 transform.position, Quaternion.identity).GetComponent<Rigidbody2D>();
            rigidBang.velocity = new Vector2(7 * darkBangDirX, dirY);
            dirY -= 2;
        }
    }
    #endregion

    public void Attack()
    {
        Collider2D collider = Physics2D.OverlapBox(hitBox[hitBoxIndex].bounds.center, 
            hitBox[hitBoxIndex].bounds.size, 0.0f, LayerMask.GetMask("Player"));

        if (collider != null)
        {
            gameManager.OnSkul.GetComponent<PlayerController>().Hit(damage, 
                new Vector2(transform.localScale.x, 1f));
        }
    }

    public void EndPattern()
    {
        rigid.velocity = Vector2.zero;
        state = BossPatternState.Inactive;
        patternDelay = patternDelayTime;
        gravity.enabled = true;
        isInvincible = false;
        anim.SetBool("IsActive", false);
    }

    public override void Hit(int _damage, Vector2 _knockback)
    {
        if (isInvincible)
        {
            return;
        }

        hp -= _damage;
        hpBar.fillAmount = (float)hp / maxHp;

        if (hp <= 0)
        {
            if (phase.Equals(0))
            {
                hp = 600;
                maxHp = hp;
                hpBar.color = Color.magenta;
                hpBar.fillAmount = (float)hp / maxHp;
                phase++;
                StopAllCoroutines();
                anim.StopPlayback();
                EndPattern();
                state = BossPatternState.Appearance;
                anim.SetFloat("Idle", 0);
                anim.SetFloat("Phase", 1);
                StartCoroutine(changeform());
                patternDelayTime = 3f;
                isInvincible = true;
            }
            else
            {
                Destroy(gameObject);
                fade.SceneName = "Stage_0";
                fade.FadeInOut(false, true);
            }
        }
    }
}
