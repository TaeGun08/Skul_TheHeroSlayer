using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Status
{
    public int hp;
    public int curHp;
    public float receivedDamage;
    public int physicalAttackPower;
    public int magicAttackPower;
    public int attackSpeed;
    public int moveSpeed;
    public int focusSpeed;
    public int skillCoolSpeed;
    public int switchCoolSpeed;
    public int essenceCoolSpeed;
    public int crit;
    public float critDamage;
    public bool rookieOn;
}

public class PlayerStatus : MonoBehaviour
{
    private Dictionary<string, Status> playerStatus = new Dictionary<string, Status>();

    [Header("스테이터스")]
    [SerializeField] private Status status;
    public Status Status { get { return status; } set { status = value; } }
    [SerializeField] private Status playingGameStatus;
    public Status PlayingGameStatus { get { return playingGameStatus; } set { playingGameStatus = value; } }

    private void Awake()
    {
        if (!string.IsNullOrEmpty(PlayerPrefs.GetString("SaveStatus")))
        {
            GetSaveStatus();
        }
        else
        {
            playerStatus.Add("Status", status);
            playerStatus.Add("PlayingGameStatus", playingGameStatus);
            ResetStatusData();
            RestartStatus();
            SetSaveStatus();
        }
    }

    public void ResetStatusData()
    {
        status = new Status();
        status.hp = 100;
        status.curHp = status.hp;
        status.receivedDamage = 1.00f;
        status.physicalAttackPower = 100;
        status.magicAttackPower = 100;
        status.attackSpeed = 100;
        status.moveSpeed = 100;
        status.focusSpeed = 100;
        status.skillCoolSpeed = 100;
        status.switchCoolSpeed = 100;
        status.essenceCoolSpeed = 100;
        status.crit = 0;
        status.critDamage = 1.50f;
        status.rookieOn = false;

        playerStatus["Status"] = status;
    }

    public void RestartStatus()
    {
        playingGameStatus = new Status();
        playingGameStatus.hp = status.hp;
        playingGameStatus.curHp = status.hp;
        playingGameStatus.receivedDamage = status.receivedDamage;
        playingGameStatus.physicalAttackPower = status.physicalAttackPower;
        playingGameStatus.magicAttackPower = status.magicAttackPower;
        playingGameStatus.attackSpeed = status.attackSpeed;
        playingGameStatus.moveSpeed = status.moveSpeed;
        playingGameStatus.focusSpeed = status.focusSpeed;
        playingGameStatus.skillCoolSpeed = status.skillCoolSpeed;
        playingGameStatus.switchCoolSpeed = status.switchCoolSpeed;
        playingGameStatus.essenceCoolSpeed = status.essenceCoolSpeed;
        playingGameStatus.crit = status.crit;
        playingGameStatus.critDamage = status.critDamage;
        playingGameStatus.rookieOn = status.rookieOn;

        if (playingGameStatus.rookieOn)
        {
            playingGameStatus.receivedDamage = 0.5f;
        }
        playerStatus["PlayingGameStatus"] = playingGameStatus;
    }

    public void SetSaveStatus()
    {
        string setSaveStatus = JsonConvert.SerializeObject(playerStatus);
        PlayerPrefs.SetString("SaveStatus", setSaveStatus);
    }

    public void GetSaveStatus()
    {
        if (!string.IsNullOrEmpty(PlayerPrefs.GetString("SaveStatus")))
        {
            playerStatus = JsonConvert.DeserializeObject<Dictionary<string, Status>>(PlayerPrefs.GetString("SaveStatus"));
            status = playerStatus["Status"];
            playingGameStatus = playerStatus["PlayingGameStatus"];
        }
    }
}
