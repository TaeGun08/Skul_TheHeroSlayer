using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RookieMode : InputMoveUI
{
    private Setting setting;

    protected override void Start()
    {
        base.Start();
        GetComponentInParent<Setting>().TryGetComponent(out setting);
    }

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

        if (Input.GetKeyDown(keyManager.Key.KeyCodes[7]))
        {
            if (count.Equals(1))
            {
                gameManager.SkulData.GetComponent<PlayerStatus>().Status.receivedDamage -= 0.5f;
                gameManager.SkulData.GetComponent<PlayerStatus>().PlayingGameStatus.receivedDamage -= 0.5f;
                gameObject.SetActive(false);
                count = 0;
                choice();
                setting.RookieOn();
            }
            else
            {
                gameObject.SetActive(false);
                count = 0;
                choice();
                setting.RookieOn();
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
            gameManager.SkulData.GetComponent<PlayerStatus>().Status.receivedDamage -= 0.5f;
            gameManager.SkulData.GetComponent<PlayerStatus>().PlayingGameStatus.receivedDamage -= 0.5f;
            gameObject.SetActive(false);
            count = 0;
            choice();
            setting.RookieOn();
        });
    }
}
