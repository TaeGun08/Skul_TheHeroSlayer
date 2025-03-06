using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    private GameManager gameManager;
    private KeyManager keyManager;
    private CanvasManager canvasManager;

    private PlayerStateUI playerStateUI;
    public PlayerStateUI PlayerStateUI => playerStateUI;

    private SkulData skulData;
    private PlayerStatus playerStatus;

    [SerializeField] private GameObject skul;
    public GameObject Skul { get { return skul; } set { skul = value; } }
    private PlayerAttack playerAttack;

    private void Awake()
    {
        TryGetComponent(out skulData);
        TryGetComponent(out playerStatus);
    }

    private void Start()
    {
        gameManager = GameManager.Instance;

        if (gameManager.ManagersDictionary.TryGetValue("KeyManager", out object _keyManager))
        {
            keyManager = _keyManager as KeyManager;
        }

        if (gameManager.ManagersDictionary.TryGetValue("CanvasManager", out object _canvasManager))
        {
            canvasManager = _canvasManager as CanvasManager;
        }

        if (canvasManager.UIDictionary.TryGetValue("PlayerState", out object _playerState))
        {
            playerStateUI = _playerState as PlayerStateUI;
        }
    }

    private void LateUpdate()
    {
        if (gameManager.OnSkul == null)
        {
            return;
        }
        else if (gameManager.OnSkul != null && skul == null)
        {
            skul = gameManager.OnSkul;
            skul.GetComponent<PlayerAttack>().TryGetComponent(out playerAttack);
        }

        if (playerAttack != null)
        {
            playerStateUI.SetCoolTime(0, playerAttack.SkillACoolTimer, playerAttack.SkillACoolTime);
            playerStateUI.SetCoolTime(1, playerAttack.SkillBCoolTimer, playerAttack.SkillBCoolTime);
            playerStateUI.SetKeyText(0, keyManager.Key.KeyCodes[9]);
            playerStateUI.SetKeyText(1, keyManager.Key.KeyCodes[10]);
            playerStateUI.SetHp(playerStatus.PlayingGameStatus.curHp, playerStatus.PlayingGameStatus.hp);
            playerStateUI.SetSwitchTimer(skulData.Timer);
        }
    }
}
