using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDrop : MonoBehaviour
{
    [SerializeField] GameObject[] droppableItems;

    public void DropItem()
    {
        // Get a random Item from the array
        int randomIndex = Random.Range(0, droppableItems.Length);

        // Spawn the random item
        Instantiate(droppableItems[randomIndex], transform.position, Quaternion.identity);
    }
}
