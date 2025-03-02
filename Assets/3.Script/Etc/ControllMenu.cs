using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ControllMenu : InputMoveUI
{
    [Header("ÄÁÆ®·Ñ")]
    [SerializeField] private Button initButton;
    [SerializeField] private Button backButton;
    [SerializeField] private GameObject changeObj;
    [SerializeField] private bool isChangeKey;
    private float delay;

    private void OnGUI()
    {
        if (Event.current.type == EventType.KeyDown && isChangeKey)
        {
            KeyCode key = Event.current.keyCode;
            if (key != KeyCode.Escape)
            {
                keyManager.SetKey(key, count);
                isChangeKey = false;
                changeObj.SetActive(false);
                delay = 0.5f;
                StartCoroutine("timerCoroutine");
            }
        }
    }

    private void LateUpdate()
    {
        if (isChangeKey)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                changeObj.SetActive(false);
                isChangeKey = false;
            }
        }
        else
        {
            if (Input.GetKeyDown(keyManager.Key.KeyCodes[0]))
            {
                count--;
                choice();
            }
            else if (Input.GetKeyDown(keyManager.Key.KeyCodes[1]))
            {
                count++;
                choice();
            }

            if (Input.GetKeyDown(keyManager.Key.KeyCodes[2]))
            {
                count = 0;
                choice();
            }
            else if (Input.GetKeyDown(keyManager.Key.KeyCodes[3]))
            {
                count = 7;
                choice();
            }

            if (Input.GetKeyDown(keyManager.Key.KeyCodes[7]) && delay <= 0f)
            {
                if (count.Equals(texts.Length - 2))
                {
                    keyManager.ResetKey();
                    count = 0;
                    choice();
                }
                else if (count.Equals(texts.Length - 1))
                {
                    gameObject.SetActive(false);
                    count = 0;
                    choice();
                }
                else
                {
                    changeObj.SetActive(true);
                    isChangeKey = true;
                }
            }
        }
    }

    private IEnumerator timerCoroutine()
    {
        while (!isChangeKey)
        {
            delay -= Time.unscaledDeltaTime;
            yield return null;
        }
    }

    protected override void buttonsEvent()
    {
        texts[count].color = changeColor;
        delay = 0f;

        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].onClick.AddListener(() =>
            {
                changeObj.SetActive(true);
                isChangeKey = true;
            });
        }

        initButton.onClick.AddListener(() =>
        {
            keyManager.ResetKey();
        });

        backButton.onClick.AddListener(() =>
        {
            gameObject.SetActive(false);
            count = 0;
        });
    }
}
