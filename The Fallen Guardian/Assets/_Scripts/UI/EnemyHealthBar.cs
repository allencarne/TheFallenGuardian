using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField] Enemy enemy;
    [SerializeField] GameObject healthBar;
    [SerializeField] Image healthBarFront;
    [SerializeField] Image healthBarBack;

    [SerializeField] SpriteRenderer bodySprite;
    [SerializeField] GameObject floatingText;
    public GameObject DamageText;
    public GameObject HealText;

    Coroutine lerpingCoroutine;
    float chipSpeed = .5f;
    bool isLerping = false;

    public void UpdateHealthUI()
    {
        if (healthBar.activeInHierarchy) // Check if the GameObject is active
        {
            if (isLerping)
            {
                StopCoroutine(lerpingCoroutine);
                isLerping = false;
            }

            lerpingCoroutine = StartCoroutine(LerpHealthBar());

            if (enemy.Health <= 0)
            {
                healthBar.SetActive(false);
            }
        }
    }

    IEnumerator LerpHealthBar()
    {
        isLerping = true;

        float fillFront = healthBarFront.fillAmount;
        float fillBack = healthBarBack.fillAmount;
        float healthFraction = enemy.Health / enemy.MaxHealth;

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

    public void ShowFloatingText(float amount, GameObject text)
    {
        Vector3 offset = new Vector3(0f, 1, 0);

        if (text)
        {
            GameObject textPrefab = Instantiate(text, transform.position + offset, Quaternion.identity);
            TextMeshPro textMesh = textPrefab.GetComponentInChildren<TextMeshPro>();
            textMesh.text = amount.ToString();
            Destroy(textPrefab, 1);
        }
    }
}
