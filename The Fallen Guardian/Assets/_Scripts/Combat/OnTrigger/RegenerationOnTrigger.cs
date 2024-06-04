using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegenerationOnTrigger : MonoBehaviour
{
    public int Stacks;
    public float Duration;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IRegenerationable regenerationable = collision.GetComponent<IRegenerationable>();

        if (regenerationable != null)
        {
            regenerationable.Regenerate(Stacks, Duration);
        }
    }
}
