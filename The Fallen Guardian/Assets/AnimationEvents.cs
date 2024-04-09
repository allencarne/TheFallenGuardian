using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimationEvents : MonoBehaviour
{
    public UnityEvent OnImpactEnd;
    public UnityEvent OnRecoveryEnd;

    public void AE_ImpactEnd()
    {
        OnImpactEnd?.Invoke();
    }

    public void AE_RecoveryEnd()
    {
        OnRecoveryEnd?.Invoke();
    }
}
