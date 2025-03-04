using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private KeyManager keyManager;

    private SkulData skulData;

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
    private float dashTime;

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

        GetComponentInParent<SkulData>().TryGetComponent(out skulData);
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
        yield return new WaitForSeconds(dashTime);
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

        if ((state.StateEnum.Equals(StateEnum.Dash) && dashCoolTimer <= 0.22f) 
            || state.StateEnum.Equals(StateEnum.Attack)
            || state.StateEnum.Equals(StateEnum.SkillAttack)
            || state.StateEnum.Equals(StateEnum.JumpAttack)
            || state.StateEnum.Equals(StateEnum.SwitchAttack)
            || playerAttack.IsSwitchAttack)
        {
            return;
        }

        Vector2 moveDir = moveDirection.MoveDir;
        Vector2 scale = transform.localScale;

        state.SetStateEnum(StateEnum.Idle, gravity.IsGround);
        moveDir.x = 0;

        if (Input.GetKey(keyManager.Key.KeyCodes[2])
        && Input.GetKey(keyManager.Key.KeyCodes[3]))
        {
            state.SetStateEnum(StateEnum.Idle, gravity.IsGround);
            moveDir.x = 0;
            moveDirection.MoveDir = moveDir;
            return;
        }

        //왼쪽
        if (Input.GetKey(keyManager.Key.KeyCodes[2]))
        {
            state.SetStateEnum(StateEnum.Walk, gravity.IsGround);
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
            state.SetStateEnum(StateEnum.Walk, gravity.IsGround);
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
        if (keyManager == null || state.StateEnum.Equals(StateEnum.SwitchAttack))
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
            if (jumpCount > 0)
            {
                playerEffect.Jump.transform.position = boxColl.bounds.min;
                playerEffect.Jump.transform.position = new Vector3(transform.position.x, boxColl.bounds.min.y, 0f);
                playerEffect.Jump.gameObject.SetActive(true);
            }
            state.SetStateEnum(StateEnum.Jump, gravity.IsGround);
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

    public void InputDash(float _dashForce, float _dashCoolTime, int _maxDashCount, bool _gravityDash, float _dashTime, float _dashUpForce)
    {
        if (keyManager == null || state.StateEnum.Equals(StateEnum.SwitchAttack))
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
            playerEffect.Dash[dashCount].transform.position = transform.position;
            playerEffect.Dash[dashCount].transform.position = boxColl.bounds.min;
            playerEffect.Dash[dashCount].transform.position -= new Vector3(transform.localScale.x * 0.5f, 0f, 0f);
            playerEffect.Dash[dashCount].Scale = transform.localScale;
            playerEffect.Dash[dashCount].gameObject.SetActive(true);
            dashTime = _dashTime;
            playerAttack.EndAttack();
            playerAttack.IsAttack = false;
            gravity.Velocity = 0f;
            anim.FallTime = 0f;
            dashCoolTimer = 0f;
            state.SetStateEnum(StateEnum.Dash, gravity.IsGround);
            moveDirection.enabled = false;
            isDash = true;
            rigid.velocity = Vector2.zero;

            if (_gravityDash)
            {
                gravity.enabled = true;
                rigid.velocity += new Vector2(_dashForce * transform.localScale.x, _dashUpForce);
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
        if (playerAttack == null)
        {
            return;
        }

        playerAttack.ResetAttack(_formChange);

        playerAttack.ResetSkillAttack();

        if (keyManager == null || state.StateEnum.Equals(StateEnum.SwitchAttack)
            || state.StateEnum.Equals(StateEnum.Dash))
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

    public void InputChangeHead()
    {
        if (keyManager == null)
        {
            return;
        }

        if (Input.GetKeyDown(keyManager.Key.KeyCodes[5]))
        {
            Collider2D collider = Physics2D.OverlapBox(boxColl.bounds.center, boxColl.bounds.size, 0.0f,
                LayerMask.GetMask("ChangeHead"));

            if (collider != null)
            {
                skulData.GetHead(transform, collider.GetComponent<ChangeHead>());
            }
        }
    }

    public void InputSwitchSkul()
    {
        if (keyManager == null)
        {
            return;
        }

        if (Input.GetKeyDown(keyManager.Key.KeyCodes[12]))
        {
            if (!skulData.Skul.SkulIndexB.Equals(0) && !playerAttack.IsSwitchAttack)
            {
                StopCoroutine("jumpingCoroutine");
                StopCoroutine("dashingCoroutine");
                skulData.SwitchSkul();
            }
        }
    }

    public void InputGetItem()
    {
        if (keyManager == null)
        {
            return;
        }

        if (Input.GetKeyDown(keyManager.Key.KeyCodes[5]))
        {
            Collider2D collider = Physics2D.OverlapBox(boxColl.bounds.center, boxColl.bounds.size, 0.0f,
                LayerMask.GetMask("Item"));

            if (collider != null)
            {

            }
        }
    }
}
