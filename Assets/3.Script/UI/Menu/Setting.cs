using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Experimental.Rendering.Universal;
using Newtonsoft.Json;
using TMPro;

public class SettingData
{
    public int width = 1920;
    public int height = 1080;
    public int count = 0;
    public bool fullScreen = true;
    public float screenShake = 0.5f;
    public float bgm = 0.5f;
    public float sfx = 0.5f;
    public bool rookieOn = false;
}

public class Setting : InputMoveUI
{
    private SettingData settingData = new SettingData();
    public SettingData SettingData { get { return settingData; } set { settingData = value; } }

    [Header("¼³Á¤")]
    [SerializeField] private Button[] leftButtons;
    [SerializeField] private Button[] rightButtons;
    [SerializeField] private GameObject[] opneUI = new GameObject[2];
    [SerializeField] private TMP_Text[] setTexts;
    [SerializeField] private Slider[] sliders;
    private PlayerStatus playerStatus;
    private Resolution resolution;

    protected override void Awake()
    {
        if (!string.IsNullOrEmpty(PlayerPrefs.GetString("SaveSetting")))
        {
            settingData = JsonConvert.DeserializeObject<SettingData>(PlayerPrefs.GetString("SaveSetting"));
            Screen.SetResolution(settingData.width, settingData.height, settingData.fullScreen);
            setTexts[0].text = $"{settingData.width} x {settingData.height}";
            if (settingData.fullScreen)
            {
                setTexts[1].text = "ÀüÃ¼È­¸é";
            }
            else
            {
                setTexts[1].text = "Ã¢ ¸ðµå";
            }

            if (settingData.rookieOn)
            {
                setTexts[2].text = "ÄÑÁü";
            }
            else
            {
                setTexts[2].text = "²¨Áü";
            }
        }
        else
        {
            settingData = new SettingData();
            Screen.SetResolution(settingData.width, settingData.height, settingData.fullScreen);
            string setSaveSetting = JsonConvert.SerializeObject(settingData);
            PlayerPrefs.SetString("SaveSetting", setSaveSetting);
        }

        base.Awake();
    }

    private void LateUpdate()
    {
        if (resolution == null)
        {
            Camera.main.TryGetComponent(out resolution);
        }

        if (playerStatus == null)
        {
            gameManager.SkulData.TryGetComponent(out playerStatus);
        }

        if (opneUI[0].activeSelf || opneUI[1].activeSelf)
        {
            return;
        }

        if (keyManager == null)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            count = 0;
            texts[0].color = changeColor;
            gameObject.SetActive(false);
            choice();
            SetSetting();
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

        if (Input.GetKeyDown(keyManager.Key.KeyCodes[2]))
        {
            leftEvent();
        }
        else if (Input.GetKeyDown(keyManager.Key.KeyCodes[3]))
        {
            rightEvent();
        }

        if (Input.GetKeyDown(keyManager.Key.KeyCodes[7]))
        {
            choiceEvent();
        }
    }

    private void leftEvent()
    {
        switch (count)
        {
            case 0:
                settingData.count--;
                setScreen();
                break;
            case 1:
                if (settingData.fullScreen)
                {
                    settingData.fullScreen = false;
                }
                else
                {
                    settingData.fullScreen = true;
                }
                break;
            case 2:
                if (sliders[0].value > 0)
                {
                    sliders[0].value -= 0.1f;
                }
                break;
            case 4:
                if (sliders[1].value > 0)
                {
                    sliders[1].value -= 0.1f;
                }
                break;
            case 5:
                if (sliders[2].value > 0)
                {
                    sliders[2].value -= 0.1f;
                }
                break;
            case 6:
                if (settingData.rookieOn)
                {
                    SettingData.rookieOn = false;
                    gameManager.SkulData.GetComponent<PlayerStatus>().Status.receivedDamage += 0.5f;
                    gameManager.SkulData.GetComponent<PlayerStatus>().PlayingGameStatus.receivedDamage += 0.5f;
                    SetSetting();
                }
                else
                {
                    opneUI[1].SetActive(true);
                }
                break;
        }

        if (settingData.fullScreen)
        {
            setTexts[1].text = "ÀüÃ¼È­¸é";
        }
        else
        {
            setTexts[1].text = "Ã¢ ¸ðµå";
        }

        if (settingData.rookieOn)
        {
            setTexts[2].text = "ÄÑÁü";
        }
        else
        {
            setTexts[2].text = "²¨Áü";
        }
    }

    private void rightEvent()
    {
        switch (count)
        {
            case 0:
                settingData.count++;
                setScreen();
                break;
            case 1:
                if (settingData.fullScreen)
                {
                    settingData.fullScreen = false;
                }
                else
                {
                    settingData.fullScreen = true;
                }
                break;
            case 2:
                if (sliders[0].value < 1)
                {
                    sliders[0].value += 0.1f;
                }
                break;
            case 4:
                if (sliders[1].value < 1)
                {
                    sliders[1].value += 0.1f;
                }
                break;
            case 5:
                if (sliders[2].value < 1)
                {
                    sliders[2].value += 0.1f;
                }
                break;
            case 6:
                if (settingData.rookieOn)
                {
                    SettingData.rookieOn = false;
                    gameManager.SkulData.GetComponent<PlayerStatus>().Status.receivedDamage += 0.5f;
                    gameManager.SkulData.GetComponent<PlayerStatus>().PlayingGameStatus.receivedDamage += 0.5f;
                    SetSetting();
                }
                else
                {
                    opneUI[1].SetActive(true);
                }
                break;
        }

        if (settingData.fullScreen)
        {
            setTexts[1].text = "ÀüÃ¼È­¸é";
        }
        else
        {
            setTexts[1].text = "Ã¢ ¸ðµå";
        }

        if (settingData.rookieOn)
        {
            setTexts[2].text = "ÄÑÁü";
        }
        else
        {
            setTexts[2].text = "²¨Áü";
        }
    }

    private void setScreen()
    {
        if (settingData.count < 0)
        {
            settingData.count = 18;
        }
        else if (settingData.count > 18)
        {
            settingData.count = 0;
        }

        switch (settingData.count)
        {
            case 0:
                settingData.width = 640;
                settingData.height = 480;
                break;
            case 1:
                settingData.width = 720;
                settingData.height = 480;
                break;
            case 2:
                settingData.width = 720;
                settingData.height = 576;
                break;
            case 3:
                settingData.width = 800;
                settingData.height = 600;
                break;
            case 4:
                settingData.width = 1024;
                settingData.height = 768;
                break;
            case 5:
                settingData.width = 1152;
                settingData.height = 864;
                break;
            case 6:
                settingData.width = 1176;
                settingData.height = 664;
                break;
            case 7:
                settingData.width = 1280;
                settingData.height = 720;
                break;
            case 8:
                settingData.width = 1280;
                settingData.height = 768;
                break;
            case 9:
                settingData.width = 1280;
                settingData.height = 800;
                break;
            case 10:
                settingData.width = 1280;
                settingData.height = 960;
                break;
            case 11:
                settingData.width = 1280;
                settingData.height = 960;
                break;
            case 12:
                settingData.width = 1280;
                settingData.height = 1024;
                break;
            case 13:
                settingData.width = 1360;
                settingData.height = 768;
                break;
            case 14:
                settingData.width = 1440;
                settingData.height = 1080;
                break;
            case 15:
                settingData.width = 1600;
                settingData.height = 900;
                break;
            case 16:
                settingData.width = 1600;
                settingData.height = 1024;
                break;
            case 17:
                settingData.width = 1680;
                settingData.height = 1050;
                break;
            case 18:
                settingData.width = 1920;
                settingData.height = 1080;
                break;
        }
        setTexts[0].text = $"{settingData.width} x {settingData.height}";
    }

    private void choiceEvent()
    {
        switch (count)
        {
            case 3:
                opneUI[0].SetActive(true);
                break;
            case 7:
                gameObject.SetActive(false);
                texts[0].color = changeColor;
                choice();
                SetSetting();
                break;
        }
    }

    public void RookieOn()
    {
        settingData.rookieOn = true;
        if (settingData.rookieOn)
        {
            setTexts[2].text = "ÄÑÁü";
        }
        else
        {
            setTexts[2].text = "²¨Áü";
        }
        SetSetting();
    }

    public void SetSetting()
    {
        Screen.SetResolution(settingData.width, settingData.height, settingData.fullScreen);
        resolution.SetAspect();
        string setSaveSetting = JsonConvert.SerializeObject(settingData);
        PlayerPrefs.SetString("SaveSetting", setSaveSetting);
    }

    public void ResetSetting()
    {
        settingData = new SettingData();
        setTexts[0].text = $"{settingData.width} x {settingData.height}";
        setTexts[1].text = "ÀüÃ¼È­¸é";
        setTexts[2].text = "²¨Áü";
        for (int i = 0; i < sliders.Length; i++)
        {
            sliders[i].value = 0.5f;
        }
        SetSetting();
    }

    protected override void buttonsEvent()
    {
        buttons[0].onClick.AddListener(() =>
        {
            opneUI[0].SetActive(true);
        });

        buttons[1].onClick.AddListener(() =>
        {
            gameObject.SetActive(false);
            texts[0].color = changeColor;
            choice();
            SetSetting();
        });

        leftButtons[2].onClick.AddListener(() =>
        {
            if (playerStatus != null)
            {
                opneUI[1].SetActive(true);
            }
        });

        rightButtons[2].onClick.AddListener(() =>
        {
            if (playerStatus != null)
            {
                opneUI[1].SetActive(true);
            }
        });
    }
}
