using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Image image;
    public Item item;
    [HideInInspector] public Transform parentAfterDrag;

    public Image dropArea;

    public UnityEvent OnBeginDragging;
    public UnityEvent OnEndDragging;
    public UnityEvent OnEndDragOnSellArea;

    public void Start()
    {
        dropArea = GameObject.FindGameObjectWithTag("Drop Area").GetComponent<Image>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        OnBeginDragging?.Invoke();

        image.raycastTarget = false;
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);

        dropArea.color = new Color(dropArea.color.r, dropArea.color.g, dropArea.color.b, .5f);
    }

    public void OnDrag(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToWorldPointInRectangle(
            (RectTransform)transform.parent, eventData.position, eventData.pressEventCamera, out Vector3 worldPoint);

        transform.position = worldPoint;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        OnEndDragging?.Invoke();

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

        dropArea.color = new Color(dropArea.color.r, dropArea.color.g, dropArea.color.b, .0f);

        if (eventData.pointerEnter != null && eventData.pointerEnter.CompareTag("Sell Area"))
        {
            if (item.sellValue == 0)
            {
                Debug.Log("Can't sell this item");
                return;
            }

            // Invoke this method the number of times as the item.sellValue
            for (int i = 0; i < item.sellValue; i++)
            {
                OnEndDragOnSellArea?.Invoke();
            }

            // Remove the Sold Item
            item.RemoveFromInventory();
        }
    }
}