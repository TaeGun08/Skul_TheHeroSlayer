using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NewGame : InputMoveUI
{
    private Fade fade;

    [Header("»õ °ÔÀÓ")]
    [SerializeField] private GameObject menu;

    protected override void Start()
    {
        base.Start();
        fade = Fade.Instance;
    }

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

        if (Input.GetKeyDown(keyManager.Key.KeyCodes[7]) && gameObject.activeSelf)
        {
            if (count.Equals(0))
            {
                gameObject.SetActive(false);
                count = 0;
            }
            else
            {
                fade.SceneName = "Stage_0";
                fade.FadeInOut(false, true);
                count = 0;
                gameObject.SetActive(false);
                menu.SetActive(false);
                gameManager.IsGamePause = false;
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
            fade.SceneName = "Stage_0";
            fade.FadeInOut(false, true);
            count = 0;
            gameObject.SetActive(false);
            menu.SetActive(false);
            gameManager.IsGamePause = false;
        });
    }
}
