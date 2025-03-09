using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputNextScene : MonoBehaviour
{
    private GameManager gameManager;
    private KeyManager keyManager;

    private Fade fade;

    [Header("¥Ÿ¿Ω æ¿")]
    [SerializeField] private string sceneName;
    private bool playerIn;

    private void Start()
    {
        gameManager = GameManager.Instance;
        fade = Fade.Instance;

        if (gameManager.ManagersDictionary.TryGetValue("KeyManager", out object _keyManager))
        {
            keyManager = _keyManager as KeyManager;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer.Equals(LayerMask.NameToLayer("Player")))
        {
            playerIn = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer.Equals(LayerMask.NameToLayer("Player")))
        {
            playerIn = false;
        }
    }

    private void LateUpdate()
    {
        if (playerIn)
        {
            if (Input.GetKeyDown(keyManager.Key.KeyCodes[5]))
            {
                fade.SceneName = sceneName;
                fade.FadeInOut(false, false);
            }
        }
    }
}
