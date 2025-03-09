using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageIn : MonoBehaviour
{
    private Fade fade;

    private void Start()
    {
        fade = Fade.Instance;
        fade.FadeInOut(true, false);
    }
}
