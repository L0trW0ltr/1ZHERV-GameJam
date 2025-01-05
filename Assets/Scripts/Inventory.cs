using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory Instance { get; private set; }

    private List<string> items = new List<string>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddItem(string itemName)
    {
        if (!items.Contains(itemName))
        {
            items.Add(itemName);
            Debug.Log($"Added '{itemName}' to the inventory.");
        }
        else
        {
            Debug.Log($"Item '{itemName}' is already in the inventory.");
        }
    }

    public bool HasItem(string itemName)
    {
        return items.Contains(itemName);
    }

    public void RemoveItem(string itemName)
    {
        if (items.Contains(itemName))
        {
            items.Remove(itemName);
            Debug.Log($"Removed '{itemName}' from the inventory.");
        }
        else
        {
            Debug.Log($"Item '{itemName}' not found in inventory.");
        }
    }

    public void ClearInventory()
    {
        items.Clear();
        Debug.Log("Inventory cleared.");
    }
}
