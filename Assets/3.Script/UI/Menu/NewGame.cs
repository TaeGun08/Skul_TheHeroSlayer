using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NewGame : InputMoveUI
{
    [Header("»õ °ÔÀÓ")]
    [SerializeField] private GameObject menu;

    private void LateUpdate()
    {
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
                SceneManager.LoadSceneAsync(1);
                gameManager.NewGame();
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
            SceneManager.LoadSceneAsync(1);
            gameManager.NewGame();
            count = 0;
            gameObject.SetActive(false);
            menu.SetActive(false);
            gameManager.IsGamePause = false;
        });
    }
}
