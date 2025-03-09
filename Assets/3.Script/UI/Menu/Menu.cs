using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Menu : InputMoveUI
{
    [Header("¸Þ´º")]
    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject[] opneUI = new GameObject[4];

    private void LateUpdate()
    {
        if (!menu.activeSelf
            && (opneUI[0].activeSelf
            || opneUI[1].activeSelf
            || opneUI[2].activeSelf
            || opneUI[3].activeSelf))
        {
            for (int i = 0; i < opneUI.Length; i++)
            {
                opneUI[i].SetActive(false);
            }

            count = 0;
            texts[0].color = changeColor;
            choice();
        }

        if (opneUI[0].activeSelf
            || opneUI[1].activeSelf
            || opneUI[2].activeSelf
            || opneUI[3].activeSelf)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gameManager.IsGamePause = gameManager.IsGamePause == false ? true : false;
            menu.SetActive(gameManager.IsGamePause);
            count = 0;
            texts[0].color = changeColor;
            choice();
        }

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

        if (Input.GetKeyDown(keyManager.Key.KeyCodes[7]))
        {
            if (count.Equals(0))
            {
                gameManager.IsGamePause = false;
                menu.SetActive(gameManager.IsGamePause);
            }
            else
            {
                opneUI[count - 1].SetActive(true);
            }
        }
    }

    protected override void buttonsEvent()
    {
        buttons[0].onClick.AddListener(() => 
        {
            gameManager.IsGamePause = false;
            menu.SetActive(gameManager.IsGamePause);
        });

        buttons[1].onClick.AddListener(() =>
        {
            opneUI[0].SetActive(true);
            count = 0;
            texts[0].color = changeColor;
            choice();
        });

        buttons[2].onClick.AddListener(() =>
        {
            opneUI[1].SetActive(true);
            count = 0;
            texts[0].color = changeColor;
            choice();
        });

        buttons[3].onClick.AddListener(() =>
        {
            opneUI[2].SetActive(true);
            count = 0;
            texts[0].color = changeColor;
            choice();
        });

        buttons[4].onClick.AddListener(() =>
        {
            opneUI[3].SetActive(true);
            count = 0;
            texts[0].color = changeColor;
            choice();
        });
    }
}
