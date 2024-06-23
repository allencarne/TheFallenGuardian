using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunSmoke : MonoBehaviour
{
    [SerializeField] GameObject runSmoke;
    [SerializeField] GameObject footStep;

    Vector3 offsetRight = new Vector3(0.1f, 0, 0);
    Vector3 offsetLeft = new Vector3(-0.1f, 0, 0);

    Quaternion rotation = Quaternion.Euler(0, 0, 90);

    Vector3 offsetUp = new Vector3(0, 0.1f, 0);
    Vector3 offsetDown = new Vector3(0, -0.1f, 0);

    float stepDuration = 4f;

    public void AE_RunSmoke()
    {
        GameObject step = Instantiate(runSmoke, transform.position, transform.rotation);
        Destroy(step, stepDuration);
    }

    public void OffsetRight_FootStep_Down()
    {
        GameObject step = Instantiate(footStep, transform.position + offsetRight, transform.rotation);
        Destroy(step, stepDuration);
    }

    public void OffsetLeft_FootStep_Down()
    {
        GameObject step = Instantiate(footStep, transform.position + offsetLeft, transform.rotation);
        Destroy(step, stepDuration);
    }

    public void OffsetUp_FootStep_Left()
    {
        GameObject step = Instantiate(footStep, transform.position + offsetUp, rotation);
        Destroy(step, stepDuration);
    }

    public void OffsetDown_FootStep_Left()
    {
        GameObject step = Instantiate(footStep, transform.position + offsetDown, rotation);
        Destroy(step, stepDuration);
    }
}
