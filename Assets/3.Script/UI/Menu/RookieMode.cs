using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RookieMode : InputMoveUI
{
    private void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            count = 0;
            texts[0].color = changeColor;
            gameObject.SetActive(false);
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
    }

    protected override void buttonsEvent()
    {

    }
}
