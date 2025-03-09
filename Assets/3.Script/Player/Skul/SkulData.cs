using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Skul
{
    public int SkulIndexA;
    public int SkulIndexB;

    public int UpgradeA;
    public int UpgradeB;

    public int[] SkillNumberA = new int[2];
    public int[] SkillNumberB = new int[2];
}

public class SkulData : MonoBehaviour
{
    private GameManager gameManager;

    private PlayerEffect playerEffect;
    private PlayerUI playerUI;

    [Header("Skuls")]
    [SerializeField] private List<GameObject> skuls;
    [SerializeField] private GameObject[] hasSkul = new GameObject[2];
    [SerializeField] private ChangeHead changeHead;
    private Skul skul = new Skul();
    public Skul Skul => skul;
    [SerializeField] private float timer;
    public float Timer => timer;
    private float skillACool;
    private float skillBCool;

    private void Awake()
    {
        TryGetComponent(out playerEffect);
        TryGetComponent(out playerUI);
    }

    private void Start()
    {
        gameManager = GameManager.Instance;

        if (!string.IsNullOrEmpty(PlayerPrefs.GetString("SaveSkul")))
        {
            skul = JsonConvert.DeserializeObject<Skul>(PlayerPrefs.GetString("SaveSkul"));

            SkulChangeUI skulChangeUIA;
            SkulChangeUI skulChangeUIB;

            if (skul.SkulIndexB.Equals(0))
            {
                hasSkul[0] = Instantiate(skuls[skul.SkulIndexA - 1], transform);
                hasSkul[0].GetComponent<SkulChangeUI>().TryGetComponent(out skulChangeUIA);
                playerUI.PlayerStateUI.Icon[0].sprite = skulChangeUIA.Icon;

                PlayerAttack playerAttackScA = hasSkul[0].GetComponent<PlayerAttack>();
                playerAttackScA.HasSkillNumber[0] = skul.SkillNumberA[0];
                playerAttackScA.HasSkillNumber[1] = skul.SkillNumberA[1];

                playerUI.PlayerStateUI.Icon[1].gameObject.SetActive(false);
            }
            else
            {
                hasSkul[0] = Instantiate(skuls[skul.SkulIndexA - 1], transform);
                hasSkul[1] = Instantiate(skuls[skul.SkulIndexB - 1], transform);
                hasSkul[1].SetActive(false);

                hasSkul[0].GetComponent<SkulChangeUI>().TryGetComponent(out skulChangeUIA);
                hasSkul[1].GetComponent<SkulChangeUI>().TryGetComponent(out skulChangeUIB);
                playerUI.PlayerStateUI.Icon[0].sprite = skulChangeUIA.Icon;
                playerUI.PlayerStateUI.Icon[1].sprite = skulChangeUIB.Icon;

                PlayerAttack playerAttackScA = hasSkul[0].GetComponent<PlayerAttack>();
                playerAttackScA.HasSkillNumber[0] = skul.SkillNumberA[0];
                playerAttackScA.HasSkillNumber[1] = skul.SkillNumberA[1];

                PlayerAttack playerAttackScB = hasSkul[1].GetComponent<PlayerAttack>();
                playerAttackScB.HasSkillNumber[0] = skul.SkillNumberB[0];
                playerAttackScB.HasSkillNumber[1] = skul.SkillNumberB[1];

                playerUI.PlayerStateUI.Icon[1].gameObject.SetActive(true);
            }

            if (!skul.SkillNumberA[0].Equals(0))
            {
                playerUI.PlayerStateUI.Skill_Icon[0].sprite = skulChangeUIA.Skill_Icon[skul.SkillNumberA[0] - 1];
                if (!skul.SkillNumberA[1].Equals(0))
                {
                    playerUI.PlayerStateUI.Skill_Icon[1].sprite = skulChangeUIA.Skill_Icon[skul.SkillNumberA[1] - 1];
                    playerUI.PlayerStateUI.Skill_Icon[1].gameObject.SetActive(true);
                }
                else
                {
                    playerUI.PlayerStateUI.Skill_Icon[1].gameObject.SetActive(false);
                }
            }

            playerEffect.SpriteRen = null;
            gameManager.OnSkul = hasSkul[0];
        }
        else
        {
            GameObject littlebone = Instantiate(skuls[0], transform);
            gameManager.OnSkul = littlebone;
            hasSkul[0] = littlebone;
            skul.SkulIndexA = littlebone.GetComponent<PlayerController>().SkulIndex;
            skul.SkillNumberA[0] = 1;
            skul.SkillNumberA[1] = 2;
            string setSaveSkul = JsonConvert.SerializeObject(skul);
            PlayerPrefs.SetString("SaveSkul", setSaveSkul);
        }
    }

    private IEnumerator switchTimer()
    {
        WaitForSeconds wfs = new WaitForSeconds(0.01f);
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            yield return wfs;
        }
        yield return null;
    }

    private IEnumerator activeFalseSkillATimer()
    {
        WaitForSeconds wfs = new WaitForSeconds(0.01f);
        while (skillACool > 0)
        {
            skillACool -= Time.deltaTime;
            yield return wfs;
        }
    }

    private IEnumerator activeFalseSkillBTimer()
    {
        WaitForSeconds wfs = new WaitForSeconds(0.01f);
        while (skillBCool > 0)
        {
            skillBCool -= Time.deltaTime;
            yield return wfs;
        }
    }

    public void GetHead(Transform _trs, ChangeHead _changeHead)
    {
        SkulChangeUI skulChangeUIA;
        SkulChangeUI skulChangeUIB;

        if (hasSkul[1] == null)
        {
            int index = skul.SkulIndexA;
            skul.SkulIndexB = index;
            skul.SkulIndexA = _changeHead.Index;

            GameObject skulObj = hasSkul[0];
            hasSkul[1] = skulObj;
            hasSkul[0] = Instantiate(skuls[skul.SkulIndexA - 1], _trs.position, Quaternion.identity, transform);
            hasSkul[0].transform.localScale = hasSkul[1].transform.localScale;

            int skill1 = skul.SkillNumberA[0];
            skul.SkillNumberB[0] = skill1;
            skul.SkillNumberA[0] = _changeHead.HasSkillNumber[0];

            int skill2 = skul.SkillNumberA[1];
            skul.SkillNumberB[1] = skill2;
            skul.SkillNumberA[1] = _changeHead.HasSkillNumber[1];

            PlayerAttack playerAttackScA = hasSkul[0].GetComponent<PlayerAttack>();
            playerAttackScA.HasSkillNumber[0] = skul.SkillNumberA[0];
            playerAttackScA.HasSkillNumber[1] = skul.SkillNumberA[1];

            PlayerAttack playerAttackScB = hasSkul[1].GetComponent<PlayerAttack>();
            skillACool = playerAttackScB.SkillACoolTimer;
            skillBCool = playerAttackScB.SkillBCoolTimer;

            hasSkul[1].SetActive(false);
        }
        else
        {
            ChangeHead changeHeadSc = Instantiate(changeHead, _trs.position, Quaternion.identity);
            changeHeadSc.DropHead();
            changeHeadSc.Index = skul.SkulIndexA;
            changeHeadSc.HasSkillNumber[0] = skul.SkillNumberA[0];
            changeHeadSc.HasSkillNumber[1] = skul.SkillNumberA[1];

            PlayerAttack playerAttackScA = hasSkul[0].GetComponent<PlayerAttack>();
            playerAttackScA.HasSkillNumber[0] = skul.SkillNumberA[0];
            playerAttackScA.HasSkillNumber[1] = skul.SkillNumberA[1];

            PlayerAttack playerAttackScB = hasSkul[1].GetComponent<PlayerAttack>();
            skillACool = playerAttackScB.SkillACoolTimer;
            skillBCool = playerAttackScB.SkillBCoolTimer;

            Vector2 scale = hasSkul[0].transform.localScale;
            Destroy(hasSkul[0]);
            skul.SkulIndexA = _changeHead.Index;
            skul.SkillNumberA[0] = _changeHead.HasSkillNumber[0];
            skul.SkillNumberA[1] = _changeHead.HasSkillNumber[1];
            hasSkul[0] = Instantiate(skuls[skul.SkulIndexA - 1], _trs.position, Quaternion.identity, transform);
            hasSkul[0].transform.localScale = scale;
        }

        hasSkul[0].GetComponent<SkulChangeUI>().TryGetComponent(out skulChangeUIA);
        hasSkul[1].GetComponent<SkulChangeUI>().TryGetComponent(out skulChangeUIB);
        playerUI.PlayerStateUI.Icon[0].sprite = skulChangeUIA.Icon;
        playerUI.PlayerStateUI.Icon[1].sprite = skulChangeUIB.Icon;

        if (!skul.SkillNumberA[0].Equals(0))
        {
            playerUI.PlayerStateUI.Skill_Icon[0].sprite = skulChangeUIA.Skill_Icon[skul.SkillNumberA[0] -1];
            if (!skul.SkillNumberA[1].Equals(0))
            {
                playerUI.PlayerStateUI.Skill_Icon[1].sprite = skulChangeUIA.Skill_Icon[skul.SkillNumberA[1] - 1];
                playerUI.PlayerStateUI.Skill_Icon[1].gameObject.SetActive(true);
            }
            else
            {
                playerUI.PlayerStateUI.Skill_Icon[1].gameObject.SetActive(false);
            }
        }

        playerUI.PlayerStateUI.Icon[1].gameObject.SetActive(true);
        playerEffect.SpriteRen = null;
        gameManager.OnSkul = hasSkul[0];

        playerUI.Skul = null;

        StopCoroutine("activeFalseSkillATimer");
        StartCoroutine("activeFalseSkillATimer");
        StopCoroutine("activeFalseSkillBTimer");
        StartCoroutine("activeFalseSkillBTimer");

        Destroy(_changeHead.gameObject);
    }

    public void SwitchSkul()
    {
        if (timer <= 0f)
        {
            SkulChangeUI skulChangeUIA = null;
            SkulChangeUI skulChangeUIB = null;

            int index = skul.SkulIndexA;
            skul.SkulIndexA = skul.SkulIndexB;
            skul.SkulIndexB = index;

            int skill1 = skul.SkillNumberA[0];
            skul.SkillNumberA[0] = skul.SkillNumberB[0];
            skul.SkillNumberB[0] = skill1;

            int skill2 = skul.SkillNumberA[1];
            skul.SkillNumberA[1] = skul.SkillNumberB[1];
            skul.SkillNumberB[1] = skill2;

            GameObject skulObj = hasSkul[0];
            hasSkul[0] = hasSkul[1];
            hasSkul[1] = skulObj;
            hasSkul[0].SetActive(true);
            hasSkul[1].SetActive(false);
            hasSkul[0].transform.position = hasSkul[1].transform.position;
            hasSkul[0].transform.localScale = hasSkul[1].transform.localScale;

            PlayerAttack playerAttackScA = hasSkul[0].GetComponent<PlayerAttack>();
            playerAttackScA.IsSwitchAttack = true;
            StartCoroutine(playerAttackScA.SwitchAttackCoroutine());
            playerEffect.SpriteRen = null;
            timer = 7f;
            gameManager.OnSkul = hasSkul[0];
            playerAttackScA.SkillACoolTimer = skillACool;
            playerAttackScA.SkillBCoolTimer = skillBCool;

            PlayerAttack playerAttackScB = hasSkul[1].GetComponent<PlayerAttack>();
            skillACool = playerAttackScB.SkillACoolTimer;
            skillBCool = playerAttackScB.SkillBCoolTimer;

            hasSkul[0].GetComponent<SkulChangeUI>().TryGetComponent(out skulChangeUIA);
            hasSkul[1].GetComponent<SkulChangeUI>().TryGetComponent(out skulChangeUIB);
            playerUI.PlayerStateUI.Icon[0].sprite = skulChangeUIA.Icon;
            playerUI.PlayerStateUI.Icon[1].sprite = skulChangeUIB.Icon;

            if (!skul.SkillNumberA[0].Equals(0))
            {
                playerUI.PlayerStateUI.Skill_Icon[0].sprite = skulChangeUIA.Skill_Icon[skul.SkillNumberA[0] - 1];
                if (!skul.SkillNumberA[1].Equals(0))
                {
                    playerUI.PlayerStateUI.Skill_Icon[1].sprite = skulChangeUIA.Skill_Icon[skul.SkillNumberA[1] - 1];
                    playerUI.PlayerStateUI.Skill_Icon[1].gameObject.SetActive(true);
                }
                else
                {
                    playerUI.PlayerStateUI.Skill_Icon[1].gameObject.SetActive(false);
                }
            }

            playerUI.Skul = null;

            StopCoroutine("activeFalseSkillATimer");
            StartCoroutine("activeFalseSkillATimer");
            StopCoroutine("activeFalseSkillBTimer");
            StartCoroutine("activeFalseSkillBTimer");
            StartCoroutine(switchTimer());
        }
    }

    public void SetSaveSkulData()
    {
        string setSaveSkul = JsonConvert.SerializeObject(skul);
        PlayerPrefs.SetString("SaveSkul", setSaveSkul);
    }
}
