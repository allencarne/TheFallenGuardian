using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "ScriptableObjects/Inventory/Item")]
public class Item : ScriptableObject
{
    [Header("Inventory")]
    public GameObjectRuntimeSet playerInventoryReference;
    Inventory inventory;

    [Header("Item")]
    public GameObject prefab;

    [Header("Stats")]
    new public string name;
    public Sprite icon;
    public int quantity;
    public int cost;
    public int sellValue;
    [Range(0, 100)] public float dropChance;

    [Header("Bools")]
    public bool isStackable;
    public bool isCurrency;

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
