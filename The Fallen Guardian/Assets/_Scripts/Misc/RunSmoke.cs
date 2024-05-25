using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunSmoke : MonoBehaviour
{
    [SerializeField] GameObject runSmoke;

    public void AE_RunSmoke()
    {
        Instantiate(runSmoke, transform.position, transform.rotation);
    }
}
