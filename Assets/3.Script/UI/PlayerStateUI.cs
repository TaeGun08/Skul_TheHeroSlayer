using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStateUI : MonoBehaviour
{
    private CanvasManager canvasManager;

    [Header("ป๓ลย")]
    [SerializeField] private Image[] icon = new Image[2];
    [SerializeField] private Image[] skill_Icon = new Image[2];
    [SerializeField] private Image[] coolTime_Img = new Image[2];
    [SerializeField] private TMP_Text[] keyText = new TMP_Text[3];
    [SerializeField] private Slider hpBar;
    public Slider HpBar { get { return hpBar; } set { hpBar = value; } }

    private void OnEnable()
    {
        GetComponentInParent<CanvasManager>().TryGetComponent(out canvasManager);
        canvasManager.UIDictionary.Add("PlayerState", this);
    }

    public void SetIcon(int _index, Sprite _sprite)
    {
        icon[_index].sprite = _sprite;
    }

    public void SetSkill_Icon(int _index, Sprite _sprite)
    {
        skill_Icon[_index].sprite = _sprite;
    }

    public void SetCoolTime(int _index, float _coolTimer, float _collTime)
    {
        coolTime_Img[_index].fillAmount = _coolTimer / _collTime;
    }

    public void SetKeyText(int _index, KeyCode _key)
    {
        keyText[_index].text = _key.ToString();
    }
}
