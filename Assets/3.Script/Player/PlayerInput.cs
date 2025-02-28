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

    private int jumpCount;
    private bool isJumping;

    private bool isDash;
    private int dashCount;
    private float dashCoolTimer;
    private bool isDashing;

    private void Awake()
    {
        TryGetComponent(out state);
        TryGetComponent(out moveDirection);
        TryGetComponent(out rigid);
        TryGetComponent(out gravity);
        TryGetComponent(out anim);
        playerEffect = GetComponentInParent<PlayerEffect>();
        TryGetComponent(out playerAttack);
    }

    private void Start()
    {
        FindAnyObjectByType<KeyManager>().TryGetComponent(out keyManager);
    }

    private IEnumerator jumpingCoroutine()
    {
        yield return new WaitForSeconds(0.05f);
        moveDirection.enabled = true;
        yield return new WaitForSeconds(0.1f);
        gravity.enabled = true;
        isJumping = false;
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

    private IEnumerator attackingCoroutine()
    {
        //moveDirection.enabled = false;
        //yield return null;
        //while (playerAttack.IsAttack)
        //{
        //    //if (Input.GetKey(keyManager.Key.KeyCodes[2])
        //    //   || Input.GetKey(keyManager.Key.KeyCodes[3]))
        //    //{
        //    //    rigid.velocity = Vector2.zero;
        //    //    rigid.velocity = new Vector2(3 * transform.localScale.x, rigid.velocity.y);
        //    //}

        //    yield return null;
        //}
        //moveDirection.enabled = true;
        yield return null;
    }

    private void attack()
    {
        state.StateEnum = StateEnum.Attack;
        moveDirection.enabled = true;
        gravity.enabled = true;
        anim.FallTime = 0f;
        gravity.Velocity = 0f;
        isDashing = false;
        anim.IsDash = false;
        StopCoroutine("dashingCoroutine");
        StopCoroutine("attackingCoroutine");
        StartCoroutine("attackingCoroutine");
    }

    public void InputMoveLeftOrRight(float _moveSpeed)
    {
        if (isDashing && dashCoolTimer <= 0.22f || playerAttack.IsAttack || playerAttack.IsSkillAttack)
        {
            return;
        }

        Vector2 moveDir = moveDirection.MoveDir;
        Vector2 scale = transform.localScale;

        if (gravity.IsGround)
        {
            state.StateEnum = StateEnum.Idle;
        }
        moveDir.x = 0;

        if (Input.GetKey(keyManager.Key.KeyCodes[2])
        && Input.GetKey(keyManager.Key.KeyCodes[3]))
        {
            if (gravity.IsGround)
            {
                state.StateEnum = StateEnum.Idle;
            }
            moveDir.x = 0;
            moveDirection.MoveDir = moveDir;
            return;
        }

        //왼쪽
        if (Input.GetKey(keyManager.Key.KeyCodes[2]))
        {
            if (gravity.IsGround)
            {
                state.StateEnum = StateEnum.Walk;
            }
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
            if (gravity.IsGround)
            {
                state.StateEnum = StateEnum.Walk;
            }
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
        if (rigid.velocity.y <= 0 && gravity.IsGround)
        {
            anim.IsJump = false;
            jumpCount = 0;
        }

        if (Input.GetKeyDown(keyManager.Key.KeyCodes[7]) 
            && _maxJumpCount > jumpCount
            && !playerAttack.IsJumpAttack)
        {
            if (rigid.velocity.y <= 0 && !gravity.IsGround)
            {
                jumpCount = 1;
            }

            isJumping = true;
            gravity.enabled = false;
            playerAttack.IsAttack = false;
            anim.IsJump = true;
            anim.FallTime = 0f;
            gravity.Velocity = 0f;
            isDashing = false;
            anim.IsDash = false;
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
        if (isDash)
        {
            dashCoolTimer += Time.deltaTime;
            if (dashCoolTimer >= _dashCoolTime)
            {
                isDash = false;
                dashCoolTimer = 0f;
                dashCount = 0;
            }

            if (dashCoolTimer >= 0.3f && isDashing && dashCount > 0)
            {
                moveDirection.enabled = true;
                gravity.enabled = true;
                isDashing = false;
                anim.IsDash = false;
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
            moveDirection.enabled = false;
            isDashing = true;
            isDash = true;
            rigid.velocity = Vector2.zero;
            anim.IsDash = true;

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
        //아래
        //점프
        if (Input.GetKeyDown(keyManager.Key.KeyCodes[1])
            && Input.GetKeyDown(keyManager.Key.KeyCodes[7]))
        {

        }
    }

    public void InputAttack(ref int _formChange)
    {
        playerAttack.ResetAttack(_formChange);

        if (isJumping)
        {
            return;
        }

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
