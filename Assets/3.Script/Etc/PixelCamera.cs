using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class PixelCamera : MonoBehaviour
{
    [Header("�ȼ�����Ʈ")]
    [SerializeField] private PixelPerfectCamera pixelPerfectCamera;
    public PixelPerfectCamera PixelPerfectCamera { get { return pixelPerfectCamera; } set { pixelPerfectCamera = value; } }
}
