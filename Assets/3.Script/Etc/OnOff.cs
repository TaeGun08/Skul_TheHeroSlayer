using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OnOff : MonoBehaviour
{
    [SerializeField] private GameObject onOff;

    private void LateUpdate()
    {
        if (SceneManager.GetActiveScene().name.Equals("Title") && onOff.activeSelf)
        {
            onOff.SetActive(false);
        }
        else if (!SceneManager.GetActiveScene().name.Equals("Title") && !onOff.activeSelf) 
        {
            onOff.SetActive(true);
        }
    }
}
