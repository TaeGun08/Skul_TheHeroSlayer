using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    private int[] getItemIndex = new int[9];
    private Item[] itemAbility = new Item[9];
    private ItemsManagement itemsManagement;

    private void Awake()
    {
        TryGetComponent(out itemsManagement);

        if (!string.IsNullOrEmpty(PlayerPrefs.GetString("SaveItem")))
        {
            getItemIndex = JsonConvert.DeserializeObject<int[]>(PlayerPrefs.GetString("SaveItem"));
            for (int i = 0; i < getItemIndex.Length; i++)
            {
                if (!getItemIndex[i].Equals(0))
                {
                    Item item = Instantiate(itemsManagement.Items[i], transform);
                    itemAbility[i] = item;
                    item.Hide();
                }
            }
        }
        else
        {
            string setSaveItem = JsonConvert.SerializeObject(getItemIndex);
            PlayerPrefs.SetString("SaveItem", setSaveItem);
        }
    }

    private void Start()
    {
        if (!GameManager.Instance.ManagersDictionary.ContainsKey("InventoryManager"))
        {
            GameManager.Instance.ManagersDictionary.Add("InventoryManager", this);
        }
    }

    public void SetItem(Item _item)
    {
        for (int i = 0; i < getItemIndex.Length; i++)
        {
            if (getItemIndex[i].Equals(0))
            {
                getItemIndex[i] = _item.Index;
                itemAbility[i] = _item;
                return;
            }
        }
    }

    public void UseItemAbility(StateEnum _stateEnum)
    {
        switch (_stateEnum) 
        {
            case StateEnum.Attack:
                for (int i = 0; i < getItemIndex.Length; i++)
                {
                    if (getItemIndex[i].Equals(1))
                    {
                        itemAbility[i].ItemAbility();
                    }
                }
                break;
            case StateEnum.Dash:
                for (int i = 0; i < getItemIndex.Length; i++)
                {
                    if (getItemIndex[i].Equals(2))
                    {
                        itemAbility[i].ItemAbility();
                    }
                }
                break;
        }
    }
}
