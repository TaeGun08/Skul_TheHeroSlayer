using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchUI : MonoBehaviour, IPointerEnterHandler
{
    [Header("��ġ�� UI")]
    [SerializeField] protected InputMoveUI inputMoveUI;
    [SerializeField] protected int count;

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        if (inputMoveUI != null)
        {
            inputMoveUI.Count = count;
            inputMoveUI.ChoiceText(count);
        }
    }
}
