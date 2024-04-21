using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NPCQuestGiver : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI interactText;
    [SerializeField] GameObject QuestUI;
    public bool isQuestAccepted = false;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            interactText.text = "Press " + "F" + " To Interact";

            Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
            rb.sleepMode = RigidbodySleepMode2D.NeverSleep;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (collision.GetComponent<PlayerInputHandler>().InteractInput)
            {
                interactText.text = "";

                QuestUI.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            interactText.text = "";
            QuestUI.SetActive(false);

            Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
            rb.sleepMode = RigidbodySleepMode2D.StartAwake;
        }
    }

    public void AcceptQuest()
    {
        isQuestAccepted = true;

        QuestUI.SetActive(false);
    }

    public void DeclineQuest()
    {
        isQuestAccepted = false;

        QuestUI.SetActive(false);
    }
}
