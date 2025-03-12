using Cinemachine;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resolution : MonoBehaviour
{
    private GameManager gameManager;
    private CanvasManager canvasManager;

    private Setting setting;

    private Camera cam;

    private float aspect = 16f / 9f;
    private bool resolutinTrue;

    private void Awake()
    {
        TryGetComponent(out cam);
    }

    private void Start()
    {
        gameManager = GameManager.Instance;
    }

    private void LateUpdate()
    {
        if (!resolutinTrue && !string.IsNullOrEmpty(PlayerPrefs.GetString("SaveSetting")))
        {
            SetAspect(JsonConvert.DeserializeObject<SettingData>(PlayerPrefs.GetString("SaveSetting")).width,
                JsonConvert.DeserializeObject<SettingData>(PlayerPrefs.GetString("SaveSetting")).height);

            resolutinTrue = true;
        }
    }

    public void SetAspect(int _width, int _height)
    {
        float curResolution = (float)_width / _height;

        Rect viewportRect = cam.rect;

        viewportRect.height = 1f;
        viewportRect.width = 1f;
        viewportRect.y = 0f;
        viewportRect.x = 0f;

        if (aspect > curResolution)
        {
            viewportRect.height = curResolution / aspect;
            viewportRect.y = (1f - viewportRect.height) / 2f;
        }
        else
        {
            viewportRect.width = aspect / curResolution;
            viewportRect.x = (1f - viewportRect.width) / 2f;
        }

        cam.rect = viewportRect;
    }
}
