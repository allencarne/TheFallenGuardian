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
    public int slotIndex; // Index of this slot in the inventory array

    private void Start()
    {
        inventory = playerInventoryReference.GetItemIndex(0).GetComponent<Inventory>();
    }

    public void AddItem(Item newItem)
    {
        item = newItem; // Assign the new item to the slot
        inventory.items[slotIndex] = newItem; // Update the inventory array
        icon.sprite = newItem.icon;
        icon.enabled = true;
    }

    public void ClearSlot()
    {
        item = null; // Clear the item in the slot
        inventory.items[slotIndex] = null; // Update the inventory array
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
        // Get the InventoryItem component of the dragged item
        InventoryItem inventoryItem = eventData.pointerDrag.GetComponent<InventoryItem>();
        if (inventoryItem != null)
        {
            // Get the InventorySlot component of the original slot
            InventorySlot originalSlot = inventoryItem.parentAfterDrag.GetComponent<InventorySlot>();

            // If the slot is empty, move the item to this slot
            if (item == null)
            {
                // Clear the original slot (if any item exists in it)
                originalSlot.ClearSlot();
                // Set the parent of the dragged item to this slot
                inventoryItem.transform.SetParent(transform);
                // Reset the local position of the dragged item
                inventoryItem.transform.localPosition = Vector3.zero;
                // Add the dragged item to this slot's inventory
                AddItem(inventoryItem.GetComponent<Item>());
            }
            // If the slot already contains an item, swap them
            else
            {
                // Get the InventorySlot component of the slot where the item is being dropped
                InventorySlot otherSlot = inventoryItem.parentAfterDrag.GetComponent<InventorySlot>();
                // Get the item currently in the other slot
                Item otherItem = otherSlot.item;

                // Add the current item of this slot to the other slot
                otherSlot.AddItem(item);
                // Add the other item to this slot
                AddItem(otherItem);

                // Set the parent of the dragged item to the other slot
                inventoryItem.transform.SetParent(otherSlot.transform);
                // Reset the local position of the dragged item
                inventoryItem.transform.localPosition = Vector3.zero;
            }
        }
    }
}
