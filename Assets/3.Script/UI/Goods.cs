using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Goods : MonoBehaviour
{
    private CanvasManager canvasManager;

    [Header("보유재화")]
    [SerializeField] private TMP_Text[] goods = new TMP_Text[3];

    private void OnEnable()
    {
        GetComponentInParent<CanvasManager>().TryGetComponent(out canvasManager);
        canvasManager.UIDictionary.Add("Goods", this);
    }

    public void SetGoods(int _index, int _goods)
    {
        goods[_index].text = $"{_goods}";
    }
}
