using Crest;
using UnityEngine;

public class WheelSteeringCrest : MonoBehaviour
{
    [Header("Wheel")]
    [SerializeField] private Transform wheel; // сам руль

    [Header("Boat")]
    [SerializeField] private MonoBehaviour boatScript; // BoatProbes

    [Header("Angles")]
    [SerializeField] private float minAngle = -90f; // влево
    [SerializeField] private float maxAngle = 90f;  // вправо

    [Header("Settings")]
    [SerializeField] private float deadZone = 0.05f;
    [SerializeField] private float maxTurnPower = 1f;

    private BoatProbes boat;

    private void Awake()
    {
        boat = boatScript as BoatProbes;
    }

    private void Update()
    {
        if (boat == null || wheel == null)
            return;

        // берём локальный угол по Z (синяя ось)
        float angle = wheel.localEulerAngles.z;

        // переводим из 0–360 в -180..180
        if (angle > 180f)
            angle -= 360f;

        // нормализация в -1..1
        float t = Mathf.InverseLerp(minAngle, maxAngle, angle) * 2f - 1f;
        t = Mathf.Clamp(t, -1f, 1f);

        if (Mathf.Abs(t) < deadZone)
            t = 0f;

        // применяем к лодке
        boat._turnPower = t * maxTurnPower;

        Debug.Log($"Angle: {angle:F1} | Steering: {t:F2}");
    }
}