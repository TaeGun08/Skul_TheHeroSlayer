using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyTimer : MonoBehaviour
{
    [Header("ÆÄ±« ½Ã°£")]
    [SerializeField] private float time;
    private float timer;
    [SerializeField] private bool falseOrDetroy;

    private void OnDisable()
    {
        timer = time;
    }

    private void Start()
    {
        timer = time;
    }

    private void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            if (!falseOrDetroy)
            {
                gameObject.SetActive(false);
            }
            else
            {
                Destroy(gameObject);
            }

            timer = time;
        }
    }
}
