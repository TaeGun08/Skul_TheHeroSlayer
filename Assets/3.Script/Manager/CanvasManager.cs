using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    private Dictionary<string, object> uiDictionary = new Dictionary<string, object>();
    public Dictionary<string, object> UIDictionary { get { return uiDictionary; } set { uiDictionary = value; } }

    private void Start()
    {
        if (!GameManager.Instance.ManagersDictionary.ContainsKey("CanvasManager"))
        {
            GameManager.Instance.ManagersDictionary.Add("CanvasManager", this);
        }
        else
        {
            GameManager.Instance.ManagersDictionary["CanvasManager"] = this;
        }
    }
}
