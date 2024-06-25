using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class NPCVendor : MonoBehaviour
{
    [SerializeField] PlayerStats playerStats;

    [SerializeField] GameObject vendorUI;
    [SerializeField] Image ShopUIImage;
    [SerializeField] TextMeshProUGUI interactText;
    [SerializeField] Transform itemPosition;

    [SerializeField] Item[] items;
    [SerializeField] Image[] itemIcons;
    [SerializeField] TextMeshProUGUI[] itemNames;
    [SerializeField] TextMeshProUGUI[] itemPrices;

    [SerializeField] GameObject confirmVendorUI;
    [SerializeField] TextMeshProUGUI confirmVendorText;
    Item selectedItem;

    public UnityEvent OnItemPurchased;
    public UnityEvent OnClubPurchased;

    private void Start()
    {
        // 0
        itemIcons[0].sprite = items[0].icon;
        itemNames[0].text = items[0].name;
        itemPrices[0].text = "Price: " + items[0].cost; 
        itemIcons[0].enabled = true;
        // 1
        itemIcons[1].sprite = items[1].icon;
        itemNames[1].text = items[1].name;
        itemPrices[1].text = "Price: " + items[1].cost;
        itemIcons[1].enabled = true;
        // 2
        itemIcons[2].sprite = items[2].icon;
        itemNames[2].text = items[2].name;
        itemPrices[2].text = "Price: " + items[2].cost;
        itemIcons[2].enabled = true;
        // 3
        itemIcons[3].sprite = items[3].icon;
        itemNames[3].text = items[3].name;
        itemPrices[3].text = "Price: " + items[3].cost;
        itemIcons[3].enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Update Interact Text
            interactText.text = "Press <color=red>F</color> To Interact";
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        // If Quest is Not Accepted
        if (collision.CompareTag("Player"))
        {
            if (collision.GetComponent<PlayerInputHandler>().InteractInput)
            {
                // Reset Interact Text
                interactText.text = "";

                vendorUI.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Reset Interact Text
            interactText.text = "";
            // Disable UI
            vendorUI.SetActive(false);
        }
    }

    public void ButtonPressed(Image clickedImage)
    {
        // Determine the index of the clicked image within the array
        int imageIndex = System.Array.IndexOf(itemIcons, clickedImage);

        // Check if the clicked image is valid and within bounds
        if (imageIndex >= 0 && imageIndex < items.Length)
        {
            // Retrieve the corresponding item using the index
            selectedItem = items[imageIndex];

            if (playerStats.Gold >= selectedItem.cost)
            {
                confirmVendorText.text = "Are You Sure You Would Like To Purchase " + selectedItem.name + " For " + selectedItem.cost + " Gold ?";
                confirmVendorUI.SetActive(true);
            }
            else
            {
                Debug.Log("Player does not buy Item");
            }
        }
    }

    public void YesButton()
    {
        if (selectedItem != null)
        {
            Instantiate(selectedItem.prefab, itemPosition);
            playerStats.Gold -= selectedItem.cost;

            OnItemPurchased?.Invoke();

            confirmVendorUI.SetActive(false);

            // Buy a club quest
            if (selectedItem.name == "Club")
            {
                OnClubPurchased?.Invoke();
            }
        }
    }

    public void NoButton()
    {
        confirmVendorUI.SetActive(false);
    }

    public void CloseVendorUIButton()
    {
        vendorUI.SetActive(false);
    }

    public void BeginDragging()
    {
        ShopUIImage.color = Color.green;
    }

    public void EndDragging()
    {
        ShopUIImage.color = Color.white;
    }
}
