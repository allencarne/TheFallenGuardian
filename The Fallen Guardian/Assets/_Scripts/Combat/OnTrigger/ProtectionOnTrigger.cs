using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtectionOnTrigger : MonoBehaviour
{
    public int Stacks;
    public float Duration;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IProtectionable protectionable = collision.GetComponent<IProtectionable>();

        if (protectionable != null)
        {
            protectionable.Protection(Stacks, Duration);
        }
    }
}
