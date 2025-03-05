using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key
{
    public KeyCode[] KeyCodes = new KeyCode[13];
}

public class KeyManager : MonoBehaviour
{
    private Dictionary<string, Key> keyDictionary = new Dictionary<string, Key>();
    private Key key = new Key();
    public Key Key => key;

    private void Awake()
    {
        if (!string.IsNullOrEmpty(PlayerPrefs.GetString("SaveKeyCode")))
        {
            keyDictionary = JsonConvert.DeserializeObject<Dictionary<string, Key>>(PlayerPrefs.GetString("SaveKeyCode"));

            key = keyDictionary["KeyCode"];
        }
        else
        {
            key.KeyCodes[0] = KeyCode.UpArrow;
            key.KeyCodes[1] = KeyCode.DownArrow;
            key.KeyCodes[2] = KeyCode.LeftArrow;
            key.KeyCodes[3] = KeyCode.RightArrow;
            key.KeyCodes[4] = KeyCode.Tab;
            key.KeyCodes[5] = KeyCode.F;
            key.KeyCodes[6] = KeyCode.D;
            key.KeyCodes[7] = KeyCode.A;
            key.KeyCodes[8] = KeyCode.S;
            key.KeyCodes[9] = KeyCode.Q;
            key.KeyCodes[10] = KeyCode.W;
            key.KeyCodes[11] = KeyCode.E;
            key.KeyCodes[12] = KeyCode.Space;

            keyDictionary.Add("KeyCode", key);

            PlayerPrefs.SetString("SaveKeyCode", JsonConvert.SerializeObject(keyDictionary));
        }
    }
    private void Start()
    {
        if (!GameManager.Instance.ManagersDictionary.ContainsKey("KeyManager"))
        {
            GameManager.Instance.ManagersDictionary.Add("KeyManager", this);
        }
    }

    /// <summary>
    /// 설정한 키 초기화
    /// </summary>
    public void ResetKey()
    {
        key.KeyCodes[0] = KeyCode.UpArrow;
        key.KeyCodes[1] = KeyCode.DownArrow;
        key.KeyCodes[2] = KeyCode.LeftArrow;
        key.KeyCodes[3] = KeyCode.RightArrow;
        key.KeyCodes[4] = KeyCode.Tab;
        key.KeyCodes[5] = KeyCode.F;
        key.KeyCodes[6] = KeyCode.D;
        key.KeyCodes[7] = KeyCode.A;
        key.KeyCodes[8] = KeyCode.S;
        key.KeyCodes[9] = KeyCode.Q;
        key.KeyCodes[10] = KeyCode.W;
        key.KeyCodes[11] = KeyCode.E;
        key.KeyCodes[12] = KeyCode.Space;

        PlayerPrefs.SetString("SaveKeyCode", JsonConvert.SerializeObject(keyDictionary));
    } 

    /// <summary>
    /// 키 변경
    /// </summary>
    public void SetKey(KeyCode keyCode, int _keyIndex)
    {
        KeyCode tempKey = key.KeyCodes[_keyIndex];
        key.KeyCodes[_keyIndex] = keyCode;

        for (int i = 0; i < key.KeyCodes.Length; i++)
        {
            if (!_keyIndex.Equals(i))
            {
                if (key.KeyCodes[_keyIndex] == key.KeyCodes[i])
                {
                    key.KeyCodes[i] = tempKey;
                    break;
                }
            }
        }

        PlayerPrefs.SetString("SaveKeyCode", JsonConvert.SerializeObject(keyDictionary));
    }
}
