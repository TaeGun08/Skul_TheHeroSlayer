using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private Dictionary<string, object> managersDictionary = new Dictionary<string, object>();
    public Dictionary<string, object> ManagersDictionary { get { return managersDictionary; } set { managersDictionary = value; } }

    [Header("½ºÄÃ")]
    [SerializeField] private SkulData skul;
    public SkulData Skul => skul;
    private SkulData skulData;
    public SkulData SkulData { get { return skulData; } set { skulData = value; } }
    [SerializeField] private GameObject onSkul;
    public GameObject OnSkul { get { return onSkul; } set { onSkul = value; } }

    [Header("Äµ¹ö½º")]
    [SerializeField] private GameObject canvas;

    private bool isGamePause;
    public bool IsGamePause
    {
        get { return isGamePause; }
        set
        {
            isGamePause = value;
            Time.timeScale = isGamePause == true ? 0 : 1;
        }
    }

    private bool isControllOff;
    public bool IsControllOff { get { return isControllOff; } set { isControllOff = value; } }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        Application.targetFrameRate = 60;

        Instantiate(canvas, transform);
    }

    public void NewGame()
    {
        Destroy(skulData.gameObject);
        PlayerPrefs.DeleteKey("SaveSkul");
    }
}
