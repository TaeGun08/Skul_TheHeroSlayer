using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeHead : MonoBehaviour
{
    private Rigidbody2D rigid;

    [Header("¸Ó¸®")]
    [SerializeField] private int index;
    public int Index { get { return index; } set { index = value; } }
    [SerializeField] private int hasSkillCount;
    public int HasSkillCount { get { return hasSkillCount; } set { hasSkillCount = value; } }
    private int[] hasSkillNumber = new int[2];
    public int[] HasSkillNumber { get { return hasSkillNumber; } set { hasSkillNumber = value; } }

    private void Awake()
    {
        TryGetComponent(out rigid);

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
    }

    public void DropHead()
    {
        rigid.velocity = new Vector2(0f, 5f);
    }
}
