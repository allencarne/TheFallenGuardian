using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NPCVendor : MonoBehaviour
{
    [SerializeField] GameObject VendorUI;
    [SerializeField] TextMeshProUGUI interactText;

    [SerializeField] Item[] items;
    [SerializeField] Image[] itemIcons;
    [SerializeField] TextMeshProUGUI[] itemNames;
    [SerializeField] TextMeshProUGUI[] itemPrices;

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
            // Get The Rigid Body of the Player
            Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
            // Make sure the PlayerRB never sleeps
            rb.sleepMode = RigidbodySleepMode2D.NeverSleep;
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

                VendorUI.SetActive(true);
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
            VendorUI.SetActive(false);
            // Get Player RB
            Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
            // Reset Player RB Settings
            rb.sleepMode = RigidbodySleepMode2D.StartAwake;
        }
    }
}
