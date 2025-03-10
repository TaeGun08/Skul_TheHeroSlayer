using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resolution : MonoBehaviour
{
    private Camera cam;

    private float aspect = 16f / 9f;

    private void Awake()
    {
        TryGetComponent(out cam);
    }

    private void Start()
    {
        SetAspect(1920, 1080);
    }

    public void SetAspect(int _width, int _height)
    {
        float curResolution = (float)_width / _height;

        Rect viewportRect = cam.rect;

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
