using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    private GameManager gameManager;
    private KeyManager keyManager;

    [Header("¸Þ´º")]
    [SerializeField] private GameObject menu;
    [SerializeField] private TMP_Text[] texts = new TMP_Text[5];
    [SerializeField] private Button[] buttons = new Button[5];
    [SerializeField] private GameObject[] opneUI = new GameObject[4];
    private int count;
    private Color color;
    private Color changeColor;

    private void Awake()
    {
        buttonsEvent();

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
        texts[0].color = changeColor;
        choiceMenu();
    }

    private void Start()
    {
        gameManager = GameManager.Instance;
        if (gameManager.ManagersDictionary.TryGetValue("KeyManager", out object _keyManager))
        {
            keyManager = _keyManager as KeyManager;
        }
    }

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
            choiceMenu();
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
            choiceMenu();
        }

        if (Input.GetKeyDown(keyManager.Key.KeyCodes[0]))
        {
            count--;
            choiceMenu();
        }
        else if (Input.GetKeyDown(keyManager.Key.KeyCodes[1]))
        {
            count++;
            choiceMenu();
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

    private void buttonsEvent()
    {
        buttons[0].onClick.AddListener(() => 
        {
            gameManager.IsGamePause = false;
            menu.SetActive(gameManager.IsGamePause);
        });

        buttons[1].onClick.AddListener(() =>
        {
            opneUI[0].SetActive(true);
        });

        buttons[2].onClick.AddListener(() =>
        {
            opneUI[1].SetActive(true);
        });

        buttons[3].onClick.AddListener(() =>
        {
            opneUI[2].SetActive(true);
        });

        buttons[4].onClick.AddListener(() =>
        {
            opneUI[3].SetActive(true);
        });
    }

    private void choiceMenu()
    {
        if (count < 0)
        {
            count = texts.Length - 1;
        }
        else if (count > texts.Length - 1)
        {
            count = 0;
        }

        for (int i = 0; i < texts.Length; i++)
        {
            texts[i].color = color;
        }

        texts[count].color = changeColor;
    }
}
