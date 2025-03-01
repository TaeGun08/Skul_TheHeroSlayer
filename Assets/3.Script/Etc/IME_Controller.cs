using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IME_Controller : MonoBehaviour
{
    private void LateUpdate()
    {
        if (Input.anyKeyDown && !Input.imeCompositionMode.Equals(IMECompositionMode.Off))
        {
            Input.imeCompositionMode = IMECompositionMode.Off;
        }
    }
}
