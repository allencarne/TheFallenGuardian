using UnityEngine;
using UnityEngine.UI;

public class EquipmentSlot : MonoBehaviour
{
    [SerializeField] int index;

    [SerializeField] EquipmentManager equipmentManager;

    public Image icon;

    Item item;

    public void AddItem(Item newItem)
    {
        item = newItem;

        icon.sprite = item.icon;
        icon.enabled = true;
    }

    public void ClearSlot()
    {
        item = null;

        icon.sprite = null;
        icon.enabled = false;
    }

    public void UseItem()
    {
        if (item != null)
        {
            Debug.Log("test");

            equipmentManager.UnEquip(index);
            //item.Use();
        }
    }
}
