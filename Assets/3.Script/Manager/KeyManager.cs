using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key
{
    public KeyCode[] KeyCodes;

    public Key(int _keySize)
    {
        KeyCodes = new KeyCode[_keySize];
    }
}

public class KeyManager : MonoBehaviour
{
    private Dictionary<int, KeyCode> keyDictionary = new Dictionary<int, KeyCode>();
    private Key key;
    public Key Key => key;

    private void Awake()
    {
        if (!string.IsNullOrEmpty(PlayerPrefs.GetString("SaveKey")))
        {
            keyDictionary = JsonConvert.DeserializeObject<Dictionary<int, KeyCode>>(PlayerPrefs.GetString("SaveKey"));

            key = new Key(keyDictionary.Count);

            for (int i = 0; i < keyDictionary.Count; i++)
            {
                key.KeyCodes[i] = keyDictionary[i];
            }
        }
        else
        {
            EarlyKey();

            key = new Key(keyDictionary.Count);

            for (int i = 0; i < keyDictionary.Count; i++)
            {
                key.KeyCodes[i] = keyDictionary[i];
            }

            PlayerPrefs.SetString("SaveKey", JsonConvert.SerializeObject(keyDictionary));
        }
    }

    /// <summary>
    /// 초기 설정 키
    /// </summary>
    private void EarlyKey()
    {
        keyDictionary.Add(0, KeyCode.UpArrow);
        keyDictionary.Add(1, KeyCode.DownArrow);
        keyDictionary.Add(2, KeyCode.LeftArrow);
        keyDictionary.Add(3, KeyCode.RightArrow);
        keyDictionary.Add(4, KeyCode.Tab);
        keyDictionary.Add(5, KeyCode.F);
        keyDictionary.Add(6, KeyCode.D);
        keyDictionary.Add(7, KeyCode.A);
        keyDictionary.Add(8, KeyCode.S);
        keyDictionary.Add(9, KeyCode.Q);
        keyDictionary.Add(10, KeyCode.W);
        keyDictionary.Add(11, KeyCode.E);
        keyDictionary.Add(12, KeyCode.Space);
    }

    /// <summary>
    /// 설정한 키 초기화
    /// </summary>
    public void ResetKey()
    {
        keyDictionary[0] = KeyCode.UpArrow;
        keyDictionary[1] = KeyCode.DownArrow;
        keyDictionary[2] = KeyCode.LeftArrow;
        keyDictionary[3] = KeyCode.RightArrow;
        keyDictionary[4] = KeyCode.Tab;
        keyDictionary[5] = KeyCode.F;
        keyDictionary[6] = KeyCode.D;
        keyDictionary[7] = KeyCode.A;
        keyDictionary[8] = KeyCode.S;
        keyDictionary[9] = KeyCode.Q;
        keyDictionary[10] = KeyCode.W;
        keyDictionary[11] = KeyCode.E;
        keyDictionary[12] = KeyCode.Space;
    } 

    /// <summary>
    /// 키 변경
    /// </summary>
    public void SetKey(KeyCode keyCode, int _keyIndex)
    {
        KeyCode tempKey = keyDictionary[_keyIndex];
        keyDictionary[_keyIndex] = keyCode;

        for (int i = 0; i < keyDictionary.Count; i++)
        {
            if (!_keyIndex.Equals(i))
            {
                if (keyDictionary[_keyIndex] == keyDictionary[i])
                {
                    keyDictionary[i] = tempKey;
                    return;
                }
            }
        }
    }
}
