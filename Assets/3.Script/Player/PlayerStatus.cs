using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Status
{
    public int hp = 100;
    public int curHp = 100;
    public float receivedDamage = 1.00f;
    public int physicalAttackPower = 100;
    public int magicAttackPower = 100;
    public int attackSpeed = 100;
    public int moveSpeed = 100;
    public int focusSpeed = 100;
    public int skillCoolSpeed = 100;
    public int switchCoolSpeed = 100;
    public int essenceCoolSpeed = 100;
    public int crit = 0;
    public float critDamage = 1.5f;
}

public class PlayerStatus : MonoBehaviour
{
    private Dictionary<string, Status> playerStatus = new Dictionary<string, Status>();

    [Header("스테이터스")]
    [SerializeField] private Status status = new Status();
    public Status Status { get { return status; } set { status = value; } }

    private void Awake()
    {
        playerStatus.Add("Status", status);
    }

    public void SetSaveStatus()
    {
        string setSaveStatus = JsonConvert.SerializeObject(playerStatus);
        PlayerPrefs.SetString("SaveStatus", setSaveStatus);
    }

    public void GetSaveStatus()
    {
        if (string.IsNullOrEmpty(PlayerPrefs.GetString("SaveStatus")))
        {
            playerStatus = JsonConvert.DeserializeObject<Dictionary<string, Status>>(PlayerPrefs.GetString("SaveStatus"));
            status = playerStatus["Status"];
        }
    }
}
