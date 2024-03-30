using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "ScriptableObjects/Inventory/Item")]
public class Item : ScriptableObject
{
    public GameObjectRuntimeSet playerInventoryReference;
    Inventory inventory;

    new public string name;
    public Sprite icon;
    public int quantity;
    public bool isStackable;

    public virtual void Use()
    {
        //Debug.Log("using " + name);
    }

    public void RemoveFromInventory()
    {
        inventory = playerInventoryReference.GetItemIndex(0).GetComponent<Inventory>();

        inventory.Remove(this);
    }
}
