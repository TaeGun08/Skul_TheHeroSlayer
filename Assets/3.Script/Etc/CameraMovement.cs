using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraMovement : MonoBehaviour
{
    private GameManager gameManager;
    private CinemachineVirtualCamera virtualCamera;

    private void Awake()
    {
        TryGetComponent(out virtualCamera);
    }

    private void Start()
    {
        gameManager = GameManager.Instance;
    }

    private void Update()
    {
        if (virtualCamera.Follow == null)
        {
            virtualCamera.Follow = gameManager.OnSkul.transform;
        }
    }
}
