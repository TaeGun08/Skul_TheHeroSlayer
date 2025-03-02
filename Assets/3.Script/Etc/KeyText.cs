using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class KeyText : TouchUI
{
    private KeyManager keyManager;

    private TMP_Text keyText;

    private void Awake()
    {
        TryGetComponent(out keyText);
    }

    private void Start()
    {
        if (GameManager.Instance.ManagersDictionary.TryGetValue("KeyManager", out object _keyManager))
        {
            keyManager = _keyManager as KeyManager;
        }

        GetComponentInParent<InputMoveUI>().TryGetComponent(out inputMoveUI);

        keyText.text = keyManager.Key.KeyCodes[count].ToString();
    }

    private void LateUpdate()
    {
        keyText.text = keyManager.Key.KeyCodes[count].ToString();
    }
}
