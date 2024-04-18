using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemToolTip : MonoBehaviour
{
    public InventoryItem inventoryItem;

    [SerializeField] Image icon;

    private void Awake()
    {
        inventoryItem = GetComponent<InventoryItem>();
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
