using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ControllMenu : MonoBehaviour
{
    private KeyManager keyManager;

    [Header("ÄÁÆ®·Ñ")]
    [SerializeField] private Button[] keyButtons = new Button[13];
    [SerializeField] private TMP_Text[] texts = new TMP_Text[15];
    [SerializeField] private Button initButton;
    [SerializeField] private Button backButton;
    [SerializeField] private GameObject changeObj;
    [SerializeField] private bool isChangeKey;
    private Color color;
    private Color changeColor;
    private int count;
    public int Count { set { count = value; } }
    private float delay;

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

    private void OnGUI()
    {
        if (Event.current.type == EventType.KeyDown && isChangeKey)
        {
            KeyCode key = Event.current.keyCode;
            if (key != KeyCode.Escape)
            {
                keyManager.SetKey(key, count);
                isChangeKey = false;
                changeObj.SetActive(false);
                delay = 0.5f;
                StartCoroutine("timerCoroutine");
            }
        }
    }

    private void Start()
    {
        if (GameManager.Instance.ManagersDictionary.TryGetValue("KeyManager", out object _keyManager))
        {
            keyManager = _keyManager as KeyManager;
        }

        texts[count].color = changeColor;
        delay = 0f;

        for (int i = 0; i < keyButtons.Length; i++)
        {
            keyButtons[i].onClick.AddListener(() =>
            {
                changeObj.SetActive(true);
                isChangeKey = true;
            });
        }

        initButton.onClick.AddListener(() =>
        {
            keyManager.ResetKey();
        });

        backButton.onClick.AddListener(() =>
        {
            gameObject.SetActive(false);
            count = 0;
        });
    }

    private void LateUpdate()
    {
        if (isChangeKey)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                changeObj.SetActive(false);
                isChangeKey = false;
            }
        }
        else
        {
            if (Input.GetKeyDown(keyManager.Key.KeyCodes[0]))
            {
                count--;
                choiceKey();
            }
            else if (Input.GetKeyDown(keyManager.Key.KeyCodes[1]))
            {
                count++;
                choiceKey();
            }

            if (Input.GetKeyDown(keyManager.Key.KeyCodes[7]) && delay <= 0f)
            {
                if (count.Equals(texts.Length - 2))
                {
                    keyManager.ResetKey();
                    count = 0;
                    choiceKey();
                }
                else if (count.Equals(texts.Length - 1))
                {
                    gameObject.SetActive(false);
                    count = 0;
                    choiceKey();
                }
                else
                {
                    changeObj.SetActive(true);
                    isChangeKey = true;
                }
            }
        }
    }

    private IEnumerator timerCoroutine()
    {
        while (!isChangeKey)
        {
            delay -= Time.unscaledDeltaTime;
            yield return null;
        }
    }

    private void choiceKey()
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

    public void ChoiceText(int _index)
    {
        for (int i = 0; i < texts.Length; i++)
        {
            texts[i].color = color;
        }

        texts[_index].color = changeColor;
    }
}
