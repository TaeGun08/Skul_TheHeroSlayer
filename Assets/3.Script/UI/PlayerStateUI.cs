using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStateUI : MonoBehaviour
{
    private CanvasManager canvasManager;

    [Header("ป๓ลย")]
    [SerializeField] private Image[] icon;
    public Image[] Icon { get { return icon; } set { icon = value; } }
    [SerializeField] private Image[] skill_Icon;
    public Image[] Skill_Icon { get { return skill_Icon; } set { skill_Icon = value; } }
    [SerializeField] private Image[] coolTime_Img;
    [SerializeField] private TMP_Text[] keyText;
    [SerializeField] private TMP_Text hpText;
    [SerializeField] private Slider hpBar;
    [SerializeField] private Image switchTimerImage;

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

    public void SetHp(int _curHp, int _hp)
    {
        hpText.text = $"{_curHp} / {_hp}";
        hpBar.value = (float)_curHp / _hp;
    }

    public void SetKeyText(int _index, KeyCode _key)
    {
        keyText[_index].text = _key.ToString();
    }

    public void SetSwitchTimer(float _timer)
    {
        switchTimerImage.fillAmount = _timer / 7;
    }
}
