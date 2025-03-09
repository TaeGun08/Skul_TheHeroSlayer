using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Title : MonoBehaviour
{
    private Fade fade;

    private bool inputOn;
    private float timer;

    private void Start()
    {
        fade = Fade.Instance;
        timer = 3f;
    }

    private void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                inputOn = true;
            }
        }

        if (Input.anyKeyDown && inputOn)
        {
            fade.FadeInOut(false, false);
            inputOn = false;
        }
    }
}
