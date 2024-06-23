using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDrop : MonoBehaviour
{
    [SerializeField] Item[] droppableItems;
    Item selectedItem;

    public void DropItem()
    {
        if (droppableItems.Length != 0)
        {
            // Get a random Item from the array
            int randomIndex = Random.Range(0, droppableItems.Length);

            Item selectedItem = droppableItems[randomIndex];

            // Get a Random number between 0 and 100
            int randomChance = Random.Range(0, 101); // 101 because upper limit is exclusive

            // Check if the randomChance is within the drop chance of the selected item
            if (randomChance <= selectedItem.dropChance)
            {
                // Spawn the item
                Instantiate(selectedItem.prefab, transform.position, Quaternion.identity);
            }
        }
    }
}
