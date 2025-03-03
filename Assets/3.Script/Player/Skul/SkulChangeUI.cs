using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkulChangeUI : MonoBehaviour
{
    [Header("UI¾ÆÀÌÄÜ")]
    [SerializeField] private Sprite icon;
    public Sprite Icon => icon;
    [SerializeField] private Sprite[] skill_Icon;
    public Sprite[] Skill_Icon=> skill_Icon;
}
