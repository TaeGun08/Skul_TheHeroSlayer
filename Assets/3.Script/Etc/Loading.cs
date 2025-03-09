using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loading : MonoBehaviour
{
    private Fade fade;

    private float timer;
    private bool sceneTrue;

    private void Start()
    {
        fade = Fade.Instance;
        timer = 1f;
    }

    private void LateUpdate()
    {
        if (timer > 0)
        {
            timer -= Time.unscaledDeltaTime;
        }
        else
        {
            if (!sceneTrue)
            {
                SceneManager.LoadSceneAsync(fade.SceneName);
            }
            sceneTrue = true;
        }
    }
}
