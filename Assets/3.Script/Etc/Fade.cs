using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
    public static Fade Instance;

    private GameManager gameManager;
    private InventoryManager inventoryManager;

    private Image fadeImage;
    [Header("페이드인아웃")]
    [SerializeField] private float speed;
    private string sceneName;
    public string SceneName { get { return sceneName; } set { sceneName = value; } }

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

        if (!string.IsNullOrEmpty(PlayerPrefs.GetString("SaveScene")))
        {
            sceneName = PlayerPrefs.GetString("SaveScene");
        }
        else
        {
            sceneName = "Stage_0";
            PlayerPrefs.SetString("SaveScene", sceneName);
        }

        TryGetComponent(out fadeImage);
    }

    private void Start()
    {
        gameManager = GameManager.Instance;
        if (gameManager.ManagersDictionary.TryGetValue("InventoryManager", out object _inventoryManager))
        {
            inventoryManager = _inventoryManager as InventoryManager;
        }

        FadeInOut(true, false);
    }

    private IEnumerator fadeInOutCoroutine(bool _fadeIn, bool _newGame)
    {
        yield return new WaitForEndOfFrame();

        float targetFrame = 1 / Time.unscaledDeltaTime;
        float timer = 3f;
        while (targetFrame < 34)
        {
            if (timer > 0)
            {
                timer -= Time.unscaledDeltaTime;
            }
            else
            {
                break;
            }
            yield return null;
        }

        Color color = fadeImage.color;
        fadeImage.raycastTarget = true;

        if (_fadeIn)
        {
            while (color.a > 0)
            {
                color.a -= Time.unscaledDeltaTime * speed;
                if (color.a <= 0)
                {
                    color.a = 0f;
                }
                fadeImage.color = color;
                yield return null;
            }
        }
        else
        {
            while (color.a < 1)
            {
                color.a += Time.unscaledDeltaTime * speed;
                if (color.a >= 1)
                {
                    color.a = 1f;
                }
                fadeImage.color = color;
                yield return null;
            }
            SceneManager.LoadSceneAsync("Loading");
        }

        PlayerPrefs.SetString("SaveScene", sceneName);
        if (gameManager.SkulData != null)
        {
            gameManager.SkulData.SetSaveSkulData();
            PlayerStatus playerStatusSc =  gameManager.SkulData.GetComponent<PlayerStatus>();
            if (_newGame)
            {
                playerStatusSc.RestartStatus();
                gameManager.NewGame();
                inventoryManager.ResetInven();
            }
            playerStatusSc.SetSaveStatus();
            inventoryManager.SetSaveInventory();
        }

        fadeImage.raycastTarget = false;
    }

    public void FadeInOut(bool _fadeIn, bool _newGame)
    {
        StartCoroutine(fadeInOutCoroutine(_fadeIn, _newGame));
    }
}
