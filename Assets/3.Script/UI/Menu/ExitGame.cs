using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExitGame : InputMoveUI
{
    private void LateUpdate()
    {
        if (keyManager == null)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gameObject.SetActive(false);
            count = 0;
            choice();
        }

        if (Input.GetKeyDown(keyManager.Key.KeyCodes[2]))
        {
            count = 1;
            choice();
        }
        else if (Input.GetKeyDown(keyManager.Key.KeyCodes[3]))
        {
            count = 0;
            choice();
        }

        if (Input.GetKeyDown(keyManager.Key.KeyCodes[7]))
        {
            if (count.Equals(0))
            {
                gameObject.SetActive(false);
                count = 0;
                choice();
            }
            else
            {
                Application.Quit();
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#endif
            }
        }
    }

    protected override void buttonsEvent()
    {
        buttons[0].onClick.AddListener(() =>
        {
            gameObject.SetActive(false);
            count = 0;
            choice();
        });

        buttons[1].onClick.AddListener(() =>
        {
            Application.Quit();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        });
    }
}
