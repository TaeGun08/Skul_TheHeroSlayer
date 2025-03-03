using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataReset : InputMoveUI
{
    [Header("데이터 초기화")]
    [SerializeField] private Setting setting;

    private void LateUpdate()
    {
        if (Input.GetKeyDown(keyManager.Key.KeyCodes[2]))
        {
            count--;
            choice();
        }
        else if (Input.GetKeyDown(keyManager.Key.KeyCodes[3]))
        {
            count++;
            choice();
        }

        if (Input.GetKeyDown(keyManager.Key.KeyCodes[7]))
        {
            if (count.Equals(0))
            {
                gameObject.SetActive(false);
                count = 0;
                texts[0].color = changeColor;
                choice();
            }
            else
            {
                gameObject.SetActive(false);
                count = 0;
                texts[0].color = changeColor;
                choice();
                PlayerPrefs.DeleteAll();
                setting.ResetSetting();
            }
        }
    }

    protected override void buttonsEvent()
    {
        buttons[0].onClick.AddListener(() =>
        {
            gameObject.SetActive(false);
            count = 0;
            texts[0].color = changeColor;
            choice();
        });

        buttons[1].onClick.AddListener(() =>
        {
            gameObject.SetActive(false);
            count = 0;
            texts[0].color = changeColor;
            choice();
            PlayerPrefs.DeleteAll();
            setting.ResetSetting();
        });
    }
}
