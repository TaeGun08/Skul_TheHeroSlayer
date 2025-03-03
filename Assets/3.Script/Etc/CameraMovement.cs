using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraMovement : MonoBehaviour
{
    private GameManager gameManager;
    private CinemachineVirtualCamera virtualCamera;
    private GameObject skul;

    private void Awake()
    {
        TryGetComponent(out virtualCamera);
    }

    private void Start()
    {
        gameManager = GameManager.Instance;
    }

    private void LateUpdate()
    {
        if (gameManager.OnSkul == null)
        {
            return;
        }
        else
        {
            if (skul != null && !skul.Equals(gameManager.OnSkul))
            {
                virtualCamera.Follow = null;
            }
        }

        if (virtualCamera.Follow == null)
        {
            skul = gameManager.OnSkul;
            virtualCamera.Follow = gameManager.OnSkul.transform;
        }
    }
}
