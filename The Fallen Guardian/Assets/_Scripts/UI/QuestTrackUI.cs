using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestTrackUI : MonoBehaviour
{
    public TextMeshProUGUI QuestNameText;
    public TextMeshProUGUI QuestTrack1Text;
    public TextMeshProUGUI QuestTrack2Text;
    public TextMeshProUGUI QuestTrack3Text;
    public TextMeshProUGUI QuestTrack4Text;
    public TextMeshProUGUI QuestTrack5Text;

    public void ResetTrackUI()
    {
        QuestNameText.text = "";

        QuestTrack1Text.text = "";
        QuestTrack1Text.color = Color.white;

        QuestTrack2Text.text = "";
        QuestTrack2Text.color = Color.white;

        QuestTrack3Text.text = "";
        QuestTrack3Text.color = Color.white;

        QuestTrack4Text.text = "";
        QuestTrack4Text.color = Color.white;

        QuestTrack5Text.text = "";
        QuestTrack5Text.color = Color.white;
    }

    public void SetTrackUI(Quest quest)
    {
        QuestNameText.text = quest.QuestName;
        QuestTrack1Text.text = quest.QuestObjective1;
        QuestTrack2Text.text = quest.QuestObjective2;
        QuestTrack3Text.text = quest.QuestObjective3;
        QuestTrack4Text.text = quest.QuestObjective4;
        QuestTrack5Text.text = quest.QuestObjective5;
    }
}
