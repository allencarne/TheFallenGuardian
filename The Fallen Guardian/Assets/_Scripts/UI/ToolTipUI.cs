using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ToolTipUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] GameObject ToolTip;

    public void OnPointerEnter(PointerEventData eventData)
    {
        ToolTip.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ToolTip.SetActive(false);
    }
}
