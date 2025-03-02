using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class PixelCamera : MonoBehaviour
{
    [Header("«»ºø∆€∆Â∆Æ")]
    [SerializeField] private PixelPerfectCamera pixelPerfectCamera;
    public PixelPerfectCamera PixelPerfectCamera { get { return pixelPerfectCamera; } set { pixelPerfectCamera = value; } }
}
