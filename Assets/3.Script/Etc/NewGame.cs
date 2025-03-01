using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NewGame : MonoBehaviour
{
    private GameManager gameManager;
    private KeyManager keyManager;

    [Header("»õ °ÔÀÓ")]
    [SerializeField] private Button[] buttons = new Button[2];
    [SerializeField] private TMP_Text[] texts = new TMP_Text[2];
    [SerializeField] private GameObject menu;
    private int count;
    private Color color;
    private Color changeColor;

    private void Awake()
    {
        color.r = 0.1886792f;
        color.g = 0.1810101f;
        color.b = 0.1753293f;
        color.a = 1f;

        changeColor.r = 0.5943396f;
        changeColor.g = 0.4921148f;
        changeColor.b = 0.4121129f;
        changeColor.a = 1f;
    }

    private void OnEnable()
    {
        count = 0;
        choiceEnd();
    }

    private void Start()
    {
        gameManager = GameManager.Instance;

        if (gameManager.ManagersDictionary.TryGetValue("KeyManager", out object _keyManager))
        {
            keyManager = _keyManager as KeyManager;
        }

        buttons[0].onClick.AddListener(() =>
        {
            SceneManager.LoadSceneAsync(1);
            gameManager.NewGame();
            count = 0;
            gameObject.SetActive(false);
            menu.SetActive(false);
            gameManager.IsGamePause = false;
        });

        buttons[1].onClick.AddListener(() =>
        {
            gameObject.SetActive(false);
            count = 0;
        });

        texts[count].color = changeColor;
    }

    private void LateUpdate()
    {
        if (Input.GetKeyDown(keyManager.Key.KeyCodes[2]))
        {
            count--;
            choiceEnd();
        }
        else if (Input.GetKeyDown(keyManager.Key.KeyCodes[3]))
        {
            count++;
            choiceEnd();
        }

        if (Input.GetKeyDown(keyManager.Key.KeyCodes[7]) && gameObject.activeSelf)
        {
            if (count.Equals(0))
            {
                SceneManager.LoadSceneAsync(1);
                gameManager.NewGame();
                count = 0;
                gameObject.SetActive(false);
                menu.SetActive(false);
                gameManager.IsGamePause = false;
            }
            else
            {
                gameObject.SetActive(false);
                count = 0;
            }
        }
    }

    private void choiceEnd()
    {
        if (count < 0)
        {
            count = 0;
        }
        else if (count > texts.Length - 1)
        {
            count = texts.Length - 1;
        }

        for (int i = 0; i < texts.Length; i++)
        {
            texts[i].color = color;
        }

        texts[count].color = changeColor;
    }
}
