using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private GameManager gameManager;

    private PlayerStatus playerStatus;

    private PlayerInput input;
    private PlayerAnimation anim;
    private SpriteRenderer spriteRen;
    private State state;
    private Rigidbody2D rigid;
    private MoveDirection moveDir;

    [Header("Skul")]
    [SerializeField] private int skulIndex;
    public int SkulIndex => skulIndex;
    [Space]
    [SerializeField] private float speed;
    [Space]
    [SerializeField] private float jumpForce;
    [SerializeField] private int jumpCount;
    [Space]
    [SerializeField] private float dashForce;
    [SerializeField] private float dashUpForce;
    [SerializeField] private float dashTime;
    [SerializeField] private float dashCoolTime;
    [SerializeField] private int dashCount;
    [SerializeField] private bool gravityDash;
    [Space]
    [SerializeField] private int formChange;

    private void Awake()
    {
        TryGetComponent(out spriteRen);
        TryGetComponent(out state);
        TryGetComponent(out rigid);
        TryGetComponent(out moveDir);
        TryGetComponent(out anim);
    }

    private void Start()
    {
        gameManager = GameManager.Instance;

        GetComponentInParent<PlayerStatus>().TryGetComponent(out playerStatus);
        TryGetComponent(out input);
    }

    private void Update()
    {
        if (gameManager.IsGamePause)
        {
            return;
        }

        input.InputMoveLeftOrRight(speed);
        input.InputJump(jumpForce, jumpCount);
        input.InputDash(dashForce, dashCoolTime, dashCount, gravityDash, dashTime, dashUpForce);
        input.InputFootholdFall();
        input.InputAttack(ref formChange);
        input.InputChangeHead();
        input.SwitchSkul();
        anim.PlayerAnim();
    }

    private IEnumerator hitColorChange()
    {
        spriteRen.color = Color.gray;
        yield return new WaitForSeconds(0.1f);
        spriteRen.color = Color.white;
    }

    private IEnumerator knockbackCoroutine(Vector2 _knockback)
    {
        rigid.velocity = Vector2.zero;
        rigid.velocity = _knockback.normalized * 5f;
        yield return new WaitForSeconds(0.1f);
        rigid.velocity = Vector2.zero;
        moveDir.enabled = true;
        yield return null;
    }

    public void Hit(int _damage, Vector2 _knockback)
    {
        if (state.StateEnum.Equals(StateEnum.Dash)
            || state.StateEnum.Equals(StateEnum.SwitchAttack)
            || state.StateEnum.Equals(StateEnum.SkillAttack))
        {
            return;
        }

        rigid.velocity = Vector2.zero;
        StartCoroutine(knockbackCoroutine(_knockback));
        moveDir.enabled = false;

        playerStatus.PlayingGameStatus.curHp -= _damage;
        StopCoroutine("hitColorChange");
        StartCoroutine("hitColorChange");

        if (playerStatus.PlayingGameStatus.curHp <= 0)
        {
            playerStatus.RestartStatus();
            gameManager.NewGame();
            SceneManager.LoadSceneAsync(1);
        }
    }
}
