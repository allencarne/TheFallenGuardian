using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBar : MonoBehaviour
{
    public PlayerStats stats;

    [SerializeField] GameObject healthBar;
    [SerializeField] TextMeshProUGUI playerName;
    [SerializeField] Image healthBarFront;
    [SerializeField] Image healthBarBack;

    [SerializeField] SpriteRenderer bodySprite;

    Coroutine lerpingCoroutine;
    float chipSpeed = .5f;
    bool isLerping = false;

    private void Start()
    {
        playerName.text = stats.PlayerName;
    }

    public void UpdateHealthUI()
    {
        if (isLerping)
        {
            StopCoroutine(lerpingCoroutine);
            isLerping = false;
        }

        lerpingCoroutine = StartCoroutine(LerpHealthBar());
    }

    IEnumerator LerpHealthBar()
    {
        isLerping = true;

        float fillFront = healthBarFront.fillAmount;
        float fillBack = healthBarBack.fillAmount;
        float healthFraction = stats.Health / stats.MaxHealth;

        if (fillBack > healthFraction)
        {
            healthBarFront.fillAmount = healthFraction;
            healthBarBack.color = Color.white;

            float elapsedTime = 0;

            while (elapsedTime < chipSpeed)
            {
                elapsedTime += Time.deltaTime;
                float percentComplete = elapsedTime / chipSpeed;
                percentComplete = percentComplete * percentComplete;
                healthBarBack.fillAmount = Mathf.Lerp(fillBack, healthFraction, percentComplete);
                yield return null;
            }
        }

        if (fillFront < healthFraction)
        {
            healthBarBack.color = Color.green;
            healthBarBack.fillAmount = healthFraction;

            float elapsedTime = 0;

            while (elapsedTime < chipSpeed)
            {
                elapsedTime += Time.deltaTime;
                float percentComplete = elapsedTime / chipSpeed;
                percentComplete = percentComplete * percentComplete;
                healthBarFront.fillAmount = Mathf.Lerp(fillFront, healthBarBack.fillAmount, percentComplete);
                yield return null;
            }
        }

        isLerping = false;
    }

    public IEnumerator FlashEffect(Color color)
    {
        float flashDuration = 0.1f;

        bodySprite.color = color;
        yield return new WaitForSeconds(flashDuration / 2);

        bodySprite.color = Color.white;
        yield return new WaitForSeconds(flashDuration / 2);

        bodySprite.color = color;
        yield return new WaitForSeconds(flashDuration / 2);

        bodySprite.color = Color.white;
        yield return new WaitForSeconds(flashDuration / 2);

        // Reset to original color
        bodySprite.color = Color.white;
    }
}
