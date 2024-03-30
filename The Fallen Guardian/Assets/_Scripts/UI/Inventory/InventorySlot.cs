using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    [SerializeField] GameObjectRuntimeSet playerInventoryReference;
    Inventory inventory;

    public Image icon;
    Item item;
    public TextMeshProUGUI amount;

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

    public void OnDrop(PointerEventData eventData)
    {
        InventoryItem inventoryItem = eventData.pointerDrag.GetComponent<InventoryItem>();
        if (inventoryItem != null)
        {
            // If the item is being dropped onto an empty slot
            if (item == null)
            {
                inventoryItem.transform.SetParent(transform);
                inventoryItem.transform.localPosition = Vector3.zero;
            }
            // If the item is being dropped onto a slot with another item, swap them
            else
            {
                InventorySlot otherSlot = inventoryItem.parentAfterDrag.GetComponent<InventorySlot>();
                Item otherItem = otherSlot.item;

                otherSlot.AddItem(item);
                AddItem(otherItem);

                inventoryItem.transform.SetParent(otherSlot.transform);
                inventoryItem.transform.localPosition = Vector3.zero;
            }
        }
    }
}
