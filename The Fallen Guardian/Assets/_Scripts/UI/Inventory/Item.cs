using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "ScriptableObjects/Inventory/Item")]
public class Item : ScriptableObject
{
    new public string name;
    public Sprite icon;

    public virtual void Use()
    {
        //Debug.Log("using " + name);
    }

    public void RemoveFromInventory()
    {
        //Inventory.instance.Remove(this);
    }
}
