using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    public Inventory inventory;
    public InventoryItem invenItem;

    public Image icon;
    public TextMeshProUGUI amountText;
    public int slotIndex; // Index of this slot in the inventory array

    public void AddItem(Item newItem)
    {
        invenItem.item = newItem;

        inventory.items[slotIndex] = newItem; // Update the inventory array
        icon.sprite = newItem.icon;
        icon.enabled = true;
        icon.color = new Color(icon.color.r, icon.color.g, icon.color.b, 1f); // Make the icon fully opaque

        // Update the stack amount text if available
        if (amountText != null && newItem.quantity > 1)
        {
            amountText.text = newItem.quantity.ToString();
        }
        else
        {
            amountText.text = ""; // Clear the text if quantity is 1 or less
        }
    }

    public void ClearSlot()
    {
        invenItem.item = null;

        inventory.items[slotIndex] = null; // Update the inventory array
        icon.sprite = null;
        icon.enabled = true; // Ensure the Image component is always enabled
        icon.color = new Color(icon.color.r, icon.color.g, icon.color.b, 0f);

        // Clear the stack amount text if available
        if (amountText != null)
        {
            amountText.text = "";
        }
    }

    public void UseItem()
    {
        if (invenItem.item != null)
        {
            invenItem.item.Use();
        }
    }

    public void RemoveItem()
    {
        inventory.Remove(invenItem.item);
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
            if (invenItem.item == null)
            {
                // Set the parent of the dragged item to this slot
                inventoryItem.transform.SetParent(transform);
                // Reset the local position of the dragged item
                inventoryItem.transform.localPosition = Vector3.zero;

                // Add the dragged item to this slot's inventory only if it's not null
                if (inventoryItem.item != null)
                {
                    AddItem(inventoryItem.item);
                }

                // Clear the original slot (if any item exists in it)
                originalSlot.ClearSlot();
            }
            // If the slot already contains an item, swap them
            else
            {
                // Get the InventorySlot component of the slot where the item is being dropped
                InventorySlot otherSlot = inventoryItem.parentAfterDrag.GetComponent<InventorySlot>();
                // Get the item currently in the other slot
                Item otherItem = otherSlot.invenItem.item;

                // Add the current item of this slot to the other slot
                otherSlot.AddItem(invenItem.item);
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
