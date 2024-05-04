using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NPCHealthBar : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI npcName;

    private void Start()
    {
        npcName.text = gameObject.name;
    }
}
