using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SkulA
{
    public int SkulIndex = -1;
    public int Rank;
}

[System.Serializable]
public class SkulB
{
    public int SkulIndex = -1;
    public int Rank;
}

public class SkulData : MonoBehaviour
{
    private GameManager gameManager;

    [Header("Skuls")]
    [SerializeField] private List<GameObject> skuls;
    [SerializeField] private List<GameObject> skullCount;

    [SerializeField] private SkulA skulA = new SkulA();
    [SerializeField] private SkulB skulB = new SkulB();

    private PlayerController playerController;

    private void Start()
    {
        gameManager = GameManager.Instance;

        Instantiate(skuls[0], transform).TryGetComponent(out playerController);
        skullCount.Add(playerController.gameObject);
        gameManager.OnSkul = skullCount[0];
        skulA.SkulIndex = playerController.SkulIndex;
    }
}
