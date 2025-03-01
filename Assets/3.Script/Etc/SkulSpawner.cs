using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkulSpawner : MonoBehaviour
{
    private GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.Instance;
        if (gameManager.SkulData == null)
        {
            gameManager.SkulData = Instantiate(gameManager.Skul, gameManager.transform);
        }
    }

    private void Update()
    {
        if (gameManager.SkulData == null)
        {
            gameManager.SkulData = Instantiate(gameManager.Skul, gameManager.transform);
        }
    }
}
