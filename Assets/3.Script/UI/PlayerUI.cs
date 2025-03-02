using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    private KeyManager keyManager;
    private CanvasManager canvasManager;

    private PlayerStateUI playerState;

    private PlayerStatus playerStatus;

    private GameObject skul;
    private PlayerAttack playerAttack;

    private void Awake()
    {
        TryGetComponent(out playerStatus);
    }

    private void Start()
    {
        if (GameManager.Instance.ManagersDictionary.TryGetValue("KeyManager", out object _keyManager))
        {
            keyManager = _keyManager as KeyManager;
        }

        if (GameManager.Instance.ManagersDictionary.TryGetValue("CanvasManager", out object _canvasManager))
        {
            canvasManager = _canvasManager as CanvasManager;
        }

        if (canvasManager.UIDictionary.TryGetValue("PlayerState", out object _playerState))
        {
            playerState = _playerState as PlayerStateUI;
        }

        skul = GameManager.Instance.OnSkul;
        skul.GetComponent<PlayerAttack>().TryGetComponent(out playerAttack);
    }

    private void LateUpdate()
    {
        playerState.SetCoolTime(0, playerAttack.SkillACollTimer, playerAttack.SkillACollTime);
        playerState.SetCoolTime(1, playerAttack.SkillBCollTimer, playerAttack.SkillBCollTime);
        playerState.SetKeyText(0, keyManager.Key.KeyCodes[9]);
        playerState.SetKeyText(1, keyManager.Key.KeyCodes[10]);
        playerState.HpBar.value = playerStatus.PlayingGameStatus.curHp / playerStatus.PlayingGameStatus.hp;
    }
}
