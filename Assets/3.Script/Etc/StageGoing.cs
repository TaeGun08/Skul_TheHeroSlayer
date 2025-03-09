using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageGoing : MonoBehaviour
{
    private Fade fade;

    [Header("스테이지 가기")]
    [SerializeField] private string sceneName;

    private void Start()
    {
        fade = Fade.Instance;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer.Equals(LayerMask.NameToLayer("Player")))
        {
            fade.SceneName = sceneName;
            fade.FadeInOut(false, false);
        }
    }
}
