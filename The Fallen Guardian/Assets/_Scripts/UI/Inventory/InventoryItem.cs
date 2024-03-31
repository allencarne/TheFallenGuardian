using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Image image;
    public Item item;
    [HideInInspector] public Transform parentAfterDrag;

    public void OnBeginDrag(PointerEventData eventData)
    {
        image.raycastTarget = false;
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        image.raycastTarget = true;
        transform.SetParent(parentAfterDrag);
        transform.localPosition = Vector3.zero; // Reset position in case drop was unsuccessful

        // Check if the drop target has the "Drop Area" tag
        if (eventData.pointerEnter != null && eventData.pointerEnter.CompareTag("Drop Area"))
        {
            Debug.Log("Drop item");

            Camera mainCamera = Camera.main;

            // Calculate the center of the camera's view in world space
            Vector3 cameraCenter = mainCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, mainCamera.nearClipPlane));

            // Instantiate the item prefab at the calculated position
            Instantiate(item.prefab, cameraCenter, Quaternion.identity);

            // Remove the item from the inventory
            InventorySlot originalSlot = parentAfterDrag.GetComponent<InventorySlot>();
            if (originalSlot != null)
            {
                originalSlot.RemoveItem();
            }
        }
    }
}
