using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizeObject : MonoBehaviour
{
    [SerializeField] Sprite[] sprites;
    [SerializeField] SpriteRenderer currentSprite;

    [SerializeField] bool flip;

    private void Start()
    {
        if (sprites.Length > 0)
        {
            int randomIndex = Random.Range(0, sprites.Length);

            currentSprite.sprite = sprites[randomIndex];
        }

        if (flip)
        {
            int randomScale = Random.Range(0, 2);

            if (randomScale == 0)
            {
                currentSprite.transform.localScale = new Vector3(-1, 1, 1);
            }
            else
            {
                currentSprite.transform.localScale = new Vector3(1, 1, 1);
            }
        }
    }
}
