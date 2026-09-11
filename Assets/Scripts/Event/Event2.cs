using System.Collections;
using UnityEngine;

public class LightFlickerEvent : BaseEvent
{
    [Header("Light Reference")]
    [SerializeField] private Light targetLight;

    [Header("Flicker Settings")]
    [SerializeField] private float duration = 4f;
    [SerializeField] private float minIntensity = 0.1f;
    [SerializeField] private float maxIntensity = 1.2f;
    [SerializeField] private float flickerSpeed = 0.05f;

    [Header("Optional")]
    [SerializeField] private bool randomizeColor = false;
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color flickerColor = new Color(1f, 0.3f, 0.3f);

    protected override IEnumerator RunEvent()
    {
        if (targetLight == null)
            yield break;

        float originalIntensity = targetLight.intensity;
        Color originalColor = targetLight.color;

        float timer = 0f;

        while (timer < duration)
        {
            // случайная яркость
            targetLight.intensity = Random.Range(minIntensity, maxIntensity);

            // опционально меняем цвет
            if (randomizeColor)
            {
                targetLight.color = Random.value > 0.5f ? normalColor : flickerColor;
            }

            float wait = Random.Range(flickerSpeed * 0.5f, flickerSpeed * 1.5f);
            yield return new WaitForSeconds(wait);

            timer += wait;
        }

        // возвращаем нормальное состояние
        targetLight.intensity = originalIntensity;
        targetLight.color = originalColor;
    }
}