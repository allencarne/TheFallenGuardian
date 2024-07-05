using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceBar : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] PlayerStats stats;

    [Header("Resource bar")]
    [SerializeField] Image barFront;
    [SerializeField] Image barBack;

    [Header("Variables")]
    [HideInInspector] float chipSpeed = 2f;
    [HideInInspector] float lerpTimer;

    private void Start()
    {

    }

    private void Update()
    {
        switch (stats.PlayerClass)
        {
            case PlayerClass.Beginner:
                UpdateFuryBar();
                break;
            case PlayerClass.Warrior:
                UpdateFuryBar();
                break;
            case PlayerClass.Magician:

                break;
            case PlayerClass.Archer:

                break;
            case PlayerClass.Rogue:

                break;
        }
    }

    void UpdateFuryBar()
    {
        float fillFront = barFront.fillAmount;
        float fillBack = barBack.fillAmount;
        float furyFraction = stats.Fury / stats.MaxFury;

        if (fillBack > furyFraction)
        {
            barFront.fillAmount = furyFraction;
            barBack.color = Color.red;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete = percentComplete * percentComplete;
            barBack.fillAmount = Mathf.Lerp(fillBack, furyFraction, percentComplete);
        }

        if (fillFront < furyFraction)
        {
            barBack.color = Color.white;
            barBack.fillAmount = furyFraction;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete = percentComplete * percentComplete;
            barFront.fillAmount = Mathf.Lerp(fillFront, barBack.fillAmount, percentComplete);
        }
    }
}
