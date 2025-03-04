using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeHead : MonoBehaviour
{
    private Rigidbody2D rigid;
    private SpriteRenderer spriteRen;

    [Header("¸Ó¸®")]
    [SerializeField] private int index;
    public int Index { get { return index; } set { index = value; } }
    [SerializeField] private int hasSkillCount;
    public int HasSkillCount { get { return hasSkillCount; } set { hasSkillCount = value; } }
    [SerializeField] private int[] hasSkillNumber = new int[2];
    public int[] HasSkillNumber { get { return hasSkillNumber; } set { hasSkillNumber = value; } }
    [SerializeField] private Sprite[] headSprites;

    private void Start()
    {
        TryGetComponent(out rigid);
        TryGetComponent(out spriteRen);

        if (index.Equals(1))
        {
            hasSkillNumber[0] = 1;
            hasSkillNumber[1] = 2;
        }
        else if (index > 1)
        {
            if (hasSkillCount.Equals(1))
            {
                hasSkillNumber[0] = Random.Range(1, 5);
            }
            else
            {
                hasSkillNumber[0] = Random.Range(1, 5);
                hasSkillNumber[1] = Random.Range(1, 5);
                while (hasSkillNumber[0].Equals(hasSkillNumber[1]))
                {
                    hasSkillNumber[1] = Random.Range(1, 5);
                }
            }
        }

        spriteRen.sprite = headSprites[index - 1];
    }

    public void DropHead()
    {
        rigid.velocity = new Vector2(0f, 5f);
    }
}
