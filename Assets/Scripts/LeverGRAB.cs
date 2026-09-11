using UnityEngine;

public class WheelController : MonoBehaviour
{
    public Transform wheel;

    public float maxAngle = 120f;
    private float currentAngle;

    void Update()
    {
        float angle = wheel.localEulerAngles.z;

        if (angle > 180f) angle -= 360f;

        angle = Mathf.Clamp(angle, -maxAngle, maxAngle);

        wheel.localRotation = Quaternion.Euler(0, 0, angle);

        currentAngle = angle;
    }

    public float GetSteering()
    {
        return currentAngle / maxAngle; // -1..1
    }
}