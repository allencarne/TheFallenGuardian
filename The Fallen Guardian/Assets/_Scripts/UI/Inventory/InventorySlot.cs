using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    [SerializeField] GameObjectRuntimeSet playerInventoryReference;
    Inventory inventory;

    public Image icon;
    Item item;

    private void Start()
    {
        inventory = playerInventoryReference.GetItemIndex(0).GetComponent<Inventory>();
    }

    public void AddItem(Item newItem)
    {
        item = newItem;

        icon.sprite = item.icon;
        icon.enabled = true;
    }

    public void ClearSlot()
    {
        item = null;

        icon.sprite = null;
        icon.enabled = false;
    }

    public void UseItem()
    {
        if (item != null)
        {
            item.Use();
        }
    }

    public void RemoveItem()
    {
        inventory.Remove(item);
    }
}
