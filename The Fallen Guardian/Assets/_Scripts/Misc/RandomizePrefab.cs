using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizePrefab : MonoBehaviour
{
    [SerializeField] GameObject[] prefabs;

    [SerializeField] bool flip;

    [SerializeField] SpriteRenderer defaultImage;
    [SerializeField] SpriteRenderer defaultShadow;

    private void Start()
    {
        defaultImage.enabled = false;
        defaultShadow.enabled = false;

        if (prefabs.Length > 0)
        {
            int randomIndex = Random.Range(0, prefabs.Length);

            GameObject selectedPrefab = prefabs[randomIndex];
            GameObject instance = Instantiate(selectedPrefab, transform.position, Quaternion.identity, transform);

            if (flip)
            {
                int randomScale = Random.Range(0, 2);

                if (randomScale == 0)
                {
                    instance.transform.localScale = new Vector3(-1, 1, 1);
                }
                else
                {
                    instance.transform.localScale = new Vector3(1, 1, 1);
                }
            }
        }
    }
}
