using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private KeyManager keyManager;

    private MoveDirection moveDirection;
    private State state;
    private Gravity gravity;
    private PlayerAnimation anim;
    private PlayerEffect playerEffect;
    private PlayerAttack playerAttack;

    private Rigidbody2D rigid;
    private BoxCollider2D boxColl;

    private int jumpCount;

    private bool isDash;
    private int dashCount;
    private float dashCoolTimer;

    private void Awake()
    {
        TryGetComponent(out state);
        TryGetComponent(out moveDirection);
        TryGetComponent(out rigid);
        TryGetComponent(out gravity);
        TryGetComponent(out anim);
        playerEffect = GetComponentInParent<PlayerEffect>();
        TryGetComponent(out playerAttack);
        TryGetComponent(out boxColl);
    }

    private void Start()
    {
        if (GameManager.Instance.ManagersDictionary.TryGetValue("KeyManager", out object _keyManager))
        {
            keyManager = _keyManager as KeyManager;
        }
    }

    private IEnumerator jumpingCoroutine()
    {
        yield return new WaitForSeconds(0.05f);
        moveDirection.enabled = true;
        yield return new WaitForSeconds(0.1f);
        gravity.enabled = true;
        yield return null;
    }

    private IEnumerator dashingCoroutine()
    {
        playerEffect.StartDashVFX();
        yield return new WaitForSeconds(0.1f);
        moveDirection.enabled = true;
        yield return null;
        anim.FallTime = 0f;
        moveDirection.enabled = false;
        rigid.velocity = Vector2.zero;
        yield return null;
    }

    private void attack()
    {
        moveDirection.enabled = true;
        anim.FallTime = 0f;
        gravity.Velocity = 0f;
        StopCoroutine("dashingCoroutine");
    }

    public void InputMoveLeftOrRight(float _moveSpeed)
    {
        if (keyManager == null)
        {
            return;
        }

        if (state.StateEnum.Equals(StateEnum.Dash) && dashCoolTimer <= 0.22f 
            || state.StateEnum.Equals(StateEnum.Attack)
            || state.StateEnum.Equals(StateEnum.SkillAttack)
            || state.StateEnum.Equals(StateEnum.JumpAttack))
        {
            return;
        }

        Vector2 moveDir = moveDirection.MoveDir;
        Vector2 scale = transform.localScale;

        state.SetSateEnum(StateEnum.Idle, gravity.IsGround);
        moveDir.x = 0;

        if (Input.GetKey(keyManager.Key.KeyCodes[2])
        && Input.GetKey(keyManager.Key.KeyCodes[3]))
        {
            state.SetSateEnum(StateEnum.Idle, gravity.IsGround);
            moveDir.x = 0;
            moveDirection.MoveDir = moveDir;
            return;
        }

        //왼쪽
        if (Input.GetKey(keyManager.Key.KeyCodes[2]))
        {
            state.SetSateEnum(StateEnum.Walk, gravity.IsGround);
            moveDir.x = -_moveSpeed;

            if (scale.x > 0)
            {
                scale.x *= -1;
                transform.localScale = scale;
            }
        }
        //오른쪽
        if (Input.GetKey(keyManager.Key.KeyCodes[3]))
        {
            state.SetSateEnum(StateEnum.Walk, gravity.IsGround);
            moveDir.x = _moveSpeed;

            if (scale.x < 0)
            {
                scale.x *= -1;
                transform.localScale = scale;
            }
        }

        moveDirection.MoveDir = moveDir;
    }

    public void InputJump(float _jumpForce, int _maxJumpCount)
    {
        if (keyManager == null)
        {
            return;
        }

        if (rigid.velocity.y <= 0 && gravity.IsGround)
        {
            jumpCount = 0;
        }

        if (jumpCount.Equals(0) && !gravity.IsGround)
        {
            jumpCount = 1;
        }

        if (Input.GetKeyDown(keyManager.Key.KeyCodes[7]) 
            && _maxJumpCount > jumpCount
            && !playerAttack.IsJumpAttack)
        {
            state.SetSateEnum(StateEnum.Jump, gravity.IsGround);
            gravity.IsGround = false;
            gravity.enabled = false;
            playerAttack.IsAttack = false;
            anim.FallTime = 0f;
            gravity.Velocity = 0f;
            rigid.velocity = new Vector2(rigid.velocity.x, _jumpForce);
            jumpCount++;
            playerEffect.StopDashVFX();
            StopCoroutine("dashingCoroutine");
            StopCoroutine("jumpingCoroutine");
            StartCoroutine("jumpingCoroutine");
        }
    }

    public void InputDash(float _dashForce, float _dashCoolTime, int _maxDashCount, bool _gravityDash)
    {
        if (keyManager == null)
        {
            return;
        }

        if (isDash)
        {
            dashCoolTimer += Time.deltaTime;
            if (dashCoolTimer >= _dashCoolTime)
            {
                isDash = false;
                dashCoolTimer = 0f;
                dashCount = 0;
            }

            if (dashCoolTimer >= 0.3f && !state.StateEnum.Equals(StateEnum.Jump) && dashCount > 0)
            {
                moveDirection.enabled = true;
                gravity.enabled = true;
            }
        }

        if (Input.GetKeyDown(keyManager.Key.KeyCodes[8])
            && dashCount < _maxDashCount
                && dashCoolTimer < 0.3f)
        {
            playerAttack.EndAttack();
            playerAttack.IsAttack = false;
            gravity.Velocity = 0f;
            anim.FallTime = 0f;
            dashCoolTimer = 0f;
            state.SetSateEnum(StateEnum.Dash, gravity.IsGround);
            moveDirection.enabled = false;
            isDash = true;
            rigid.velocity = Vector2.zero;

            if (_gravityDash)
            {
                gravity.enabled = true;
                rigid.velocity += new Vector2(_dashForce * transform.localScale.x, 5f);
            }
            else
            {
                gravity.enabled = false;
                rigid.velocity += new Vector2(_dashForce * transform.localScale.x, 0f);
            }

            dashCount++;
            StopCoroutine("jumpingCoroutine");
            StopCoroutine("dashingCoroutine");
            StartCoroutine("dashingCoroutine");
        }
    }

    public void InputFootholdFall()
    {
        if (keyManager == null)
        {
            return;
        }

        //아래
        //점프
        if (Input.GetKey(keyManager.Key.KeyCodes[1])
            && Input.GetKeyDown(keyManager.Key.KeyCodes[7]))
        {
            RaycastHit2D hit = Physics2D.BoxCast(new Vector2(boxColl.bounds.center.x, boxColl.bounds.min.y),
            new Vector2(boxColl.bounds.size.x, boxColl.bounds.size.y * 0.1f), 0.0f, Vector2.down, 0.1f,
            LayerMask.GetMask("Footboard"));

            if (hit.collider != null)
            {
                rigid.velocity = new Vector2(rigid.velocity.x, -3f);
                gravity.enabled = true;
                Footboard footboard = hit.collider.gameObject.GetComponent<Footboard>();
                footboard.FootbaordOff(boxColl);
            }
        }
    }

    public void InputAttack(ref int _formChange)
    {
        if (keyManager == null)
        {
            return;
        }

        playerAttack.ResetAttack(_formChange);

        playerAttack.ResetSkillAttack();

        if (Input.GetKeyDown(keyManager.Key.KeyCodes[6]))
        {
            attack();

            if (rigid.velocity.y <= 0 && !gravity.IsGround)
            {
                playerAttack.IsJumpAttack = true;
            }

            if (gravity.IsGround)
            {
                rigid.velocity = Vector2.zero;
                moveDirection.MoveDir = Vector2.zero;
                moveDirection.enabled = false;
            }

            playerAttack.Attack(_formChange);
        }

        if (Input.GetKeyDown(keyManager.Key.KeyCodes[9]))
        {
            attack();
            playerAttack.SkillAttack(0);
        }

        if (Input.GetKeyDown(keyManager.Key.KeyCodes[10]))
        {
            attack();
            playerAttack.SkillAttack(1);
        }
    }
}
