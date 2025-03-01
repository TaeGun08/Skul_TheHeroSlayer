using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class KeyText : MonoBehaviour, IPointerEnterHandler
{
    private KeyManager keyManager;

    private ControllMenu controllMenu;

    private TMP_Text text;

    [Header("≈∞ ¿Œµ¶Ω∫")]
    [SerializeField] private int index;

    public void OnPointerEnter(PointerEventData eventData)
    {
        controllMenu.Count = index;
        controllMenu.ChoiceText(index);
    }

    private void Awake()
    {
        TryGetComponent(out text);
    }

    private void Start()
    {
        if (GameManager.Instance.ManagersDictionary.TryGetValue("KeyManager", out object _keyManager))
        {
            keyManager = _keyManager as KeyManager;
        }

        GetComponentInParent<ControllMenu>().TryGetComponent(out controllMenu);

        text.text = keyManager.Key.KeyCodes[index].ToString();
    }

    private void LateUpdate()
    {
        text.text = keyManager.Key.KeyCodes[index].ToString();
    }
}
