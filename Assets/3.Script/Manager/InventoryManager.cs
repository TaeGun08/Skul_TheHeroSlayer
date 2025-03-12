using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    private int[] getItemIndex = new int[9];
    private Item[] itemAbility = new Item[9];
    private int count;

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
                    for (int j = 0; j < itemsManagement.Items.Length; j++)
                    {
                        if (itemsManagement.Items[j].Index.Equals(getItemIndex[i]))
                        {
                            Item item = Instantiate(itemsManagement.Items[j], transform);
                            itemAbility[count] = item;
                            item.Hide();
                            count++;
                            break;
                        }
                    }
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
        else
        {
            GameManager.Instance.ManagersDictionary["InventoryManager"] = this;
        }
    }

    private void LateUpdate()
    {
        if (string.IsNullOrEmpty(PlayerPrefs.GetString("SaveItem")) && !itemAbility.Length.Equals(0))
        {
            for (int i = 0; i < itemAbility.Length; i++)
            {
                if (itemAbility[i] != null)
                {
                    Destroy(itemAbility[i].gameObject);
                }
                getItemIndex[i] = 0;
            }
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

    public void SetSaveInventory()
    {
        string setSaveItem = JsonConvert.SerializeObject(getItemIndex);
        PlayerPrefs.SetString("SaveItem", setSaveItem);
    }

    public void ResetInven()
    {
        PlayerPrefs.DeleteKey("SaveItem");
        for (int i = 0; i < itemAbility.Length; i++)
        {
            if (itemAbility[i] != null)
            {
                Destroy(itemAbility[i].gameObject);
            }
        }
    }
}
