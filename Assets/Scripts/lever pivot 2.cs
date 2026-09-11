using Crest;
using UnityEngine;

public class LeverThrottleByDistance : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform leverPoint;   // точка на корабле
    [SerializeField] private Transform leverHead;    // точка на рычаге

    [Header("Boat")]
    [SerializeField] private BoatProbes boat;

    [Header("Distance Settings")]
    [Tooltip("Стартовая дистанция будет считаться нулевой скоростью")]
    [SerializeField] private bool useStartDistanceAsZeroSpeed = true;

    [Tooltip("Минимальная дистанция рычага = максимальная скорость")]
    [SerializeField] private float minDistance = 0.060f;

    [Header("Throttle Settings")]
    [Tooltip("Максимальная мощность двигателя, которая будет подаваться в BoatProbes")]
    [SerializeField] private float maxEnginePower = 5f;

    [Tooltip("Мёртвая зона около стартового положения")]
    [SerializeField] private float deadZone = 0.05f;

    private float startDistance;

    private void Start()
    {
        if (leverPoint == null || leverHead == null)
        {
            Debug.LogError("LeverThrottleByDistance: leverPoint или leverHead не назначены.");
            enabled = false;
            return;
        }

        if (boat == null)
        {
            Debug.LogError("LeverThrottleByDistance: BoatProbes не назначен.");
            enabled = false;
            return;
        }

        float currentDistance = Vector3.Distance(leverPoint.position, leverHead.position);

        if (useStartDistanceAsZeroSpeed)
            startDistance = currentDistance;
        else
            startDistance = 0.185f; // можешь вручную заменить, если понадобится

        Debug.Log($"LeverThrottleByDistance INIT | StartDist: {startDistance:F3} | MinDist: {minDistance:F3}");
    }

    private void Update()
    {
        float currentDistance = Vector3.Distance(leverPoint.position, leverHead.position);

        // Не даём выйти за границы
        currentDistance = Mathf.Clamp(currentDistance, minDistance, startDistance);

        // startDistance = 0 скорость
        // minDistance = максимум
        float throttle = Mathf.InverseLerp(startDistance, minDistance, currentDistance);
        throttle = Mathf.Clamp01(throttle);

        if (throttle < deadZone)
            throttle = 0f;

        float enginePower = throttle * maxEnginePower;
        boat._enginePower = enginePower;

        Debug.Log(
            $"StartDist: {startDistance:F3} | " +
            $"CurrentDist: {currentDistance:F3} | " +
            $"MinDist: {minDistance:F3} | " +
            $"Throttle: {throttle:F2} | " +
            $"EnginePower: {enginePower:F2}"
        );
    }
}