using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class InputMoveUI : MonoBehaviour
{
    protected GameManager gameManager;
    protected KeyManager keyManager;

    [Header("¿òÁ÷ÀÏ UI")]
    [SerializeField] protected TMP_Text[] texts;
    [SerializeField] protected Button[] buttons;
    protected int count;
    public int Count { set { count = value; } }
    protected Color color;
    protected Color changeColor;

    protected virtual void Awake()
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

    protected virtual void OnEnable()
    {
        count = 0;
        texts[0].color = changeColor;
        choice();
    }

    protected  virtual void Start()
    {
        gameManager = GameManager.Instance;
        if (gameManager.ManagersDictionary.TryGetValue("KeyManager", out object _keyManager))
        {
            keyManager = _keyManager as KeyManager;
        }

        buttonsEvent();
    }

    protected abstract void buttonsEvent();

    protected virtual void choice()
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

    public virtual void ChoiceText(int _index)
    {
        for (int i = 0; i < texts.Length; i++)
        {
            texts[i].color = color;
        }

        texts[_index].color = changeColor;
    }
}
