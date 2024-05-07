using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestTrackUI : MonoBehaviour
{
    public TextMeshProUGUI QuestNameText;
    public TextMeshProUGUI QuestTrack1Text;
    public TextMeshProUGUI QuestTrack2Text;
    public TextMeshProUGUI QuestTrack3Text;
    public TextMeshProUGUI QuestTrack4Text;
    public TextMeshProUGUI QuestTrack5Text;

    [SerializeField] RectTransform panelRectTransform;

    public void ResetTrackUI()
    {
        QuestNameText.text = "";

        QuestTrack1Text.text = "";
        QuestTrack1Text.color = Color.white;
        QuestTrack1Text.fontStyle &= ~FontStyles.Strikethrough;

        QuestTrack2Text.text = "";
        QuestTrack2Text.color = Color.white;
        QuestTrack2Text.fontStyle &= ~FontStyles.Strikethrough;

        QuestTrack3Text.text = "";
        QuestTrack3Text.color = Color.white;
        QuestTrack3Text.fontStyle &= ~FontStyles.Strikethrough;

        QuestTrack4Text.text = "";
        QuestTrack4Text.color = Color.white;
        QuestTrack4Text.fontStyle &= ~FontStyles.Strikethrough;

        QuestTrack5Text.text = "";
        QuestTrack5Text.color = Color.white;
        QuestTrack5Text.fontStyle &= ~FontStyles.Strikethrough;
    }

    public void SetTrackUI(Quest quest)
    {
        QuestNameText.text = quest.QuestName;
        QuestTrack1Text.text = quest.QuestObjective1;
        QuestTrack2Text.text = quest.QuestObjective2;
        QuestTrack3Text.text = quest.QuestObjective3;
        QuestTrack4Text.text = quest.QuestObjective4;
        QuestTrack5Text.text = quest.QuestObjective5;

        int emptyObjectivesCount = 0;
        if (string.IsNullOrEmpty(quest.QuestObjective1)) emptyObjectivesCount++;
        if (string.IsNullOrEmpty(quest.QuestObjective2)) emptyObjectivesCount++;
        if (string.IsNullOrEmpty(quest.QuestObjective3)) emptyObjectivesCount++;
        if (string.IsNullOrEmpty(quest.QuestObjective4)) emptyObjectivesCount++;
        if (string.IsNullOrEmpty(quest.QuestObjective5)) emptyObjectivesCount++;

        Debug.Log(emptyObjectivesCount);

        switch (emptyObjectivesCount)
        {
            case 0:
                panelRectTransform.anchoredPosition = new Vector2(panelRectTransform.anchoredPosition.x, -72);
                panelRectTransform.sizeDelta = new Vector2(panelRectTransform.sizeDelta.x, 100);
                break;
            case 1:
                panelRectTransform.anchoredPosition = new Vector2(panelRectTransform.anchoredPosition.x, -64.5f);
                panelRectTransform.sizeDelta = new Vector2(panelRectTransform.sizeDelta.x, 85);
                break;
            case 2:
                panelRectTransform.anchoredPosition = new Vector2(panelRectTransform.anchoredPosition.x, -55.5f);
                panelRectTransform.sizeDelta = new Vector2(panelRectTransform.sizeDelta.x, 67);
                break;
            case 3:
                panelRectTransform.anchoredPosition = new Vector2(panelRectTransform.anchoredPosition.x, -47);
                panelRectTransform.sizeDelta = new Vector2(panelRectTransform.sizeDelta.x, 50);
                break;
            case 4:
                panelRectTransform.anchoredPosition = new Vector2(panelRectTransform.anchoredPosition.x, -39.5f);
                panelRectTransform.sizeDelta = new Vector2(panelRectTransform.sizeDelta.x, 35);
                break;
        }
    }

    public void SetTrackCounterUI(Quest quest, int count, int amount)
    {
        QuestNameText.text = quest.QuestName;
        QuestTrack1Text.text = quest.QuestObjective1 + " " + count + " / " + amount;

        // Set Rect
        panelRectTransform.anchoredPosition = new Vector2(panelRectTransform.anchoredPosition.x, -39.5f);
        panelRectTransform.sizeDelta = new Vector2(panelRectTransform.sizeDelta.x, 35);
    }

    public void Track1()
    {
        QuestTrack1Text.fontStyle |= FontStyles.Strikethrough;
        QuestTrack1Text.color = Color.black;
    }

    public void Track2()
    {
        QuestTrack2Text.fontStyle |= FontStyles.Strikethrough;
        QuestTrack2Text.color = Color.black;
    }

    public void Track3()
    {
        QuestTrack3Text.fontStyle |= FontStyles.Strikethrough;
        QuestTrack3Text.color = Color.black;
    }

    public void Track4()
    {
        QuestTrack4Text.fontStyle |= FontStyles.Strikethrough;
        QuestTrack4Text.color = Color.black;
    }

    public void Track5()
    {
        QuestTrack5Text.fontStyle |= FontStyles.Strikethrough;
        QuestTrack5Text.color = Color.black;
    }
}
