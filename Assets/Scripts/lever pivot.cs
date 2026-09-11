/*
using UnityEngine;

public class LeverThrottle : MonoBehaviour
{
    [Header("Lever")]
    [SerializeField] private Transform leverPivot;
    [Header("Angles")]
    [SerializeField] private float minAngle = -74.676f; 
    [SerializeField] private float maxAngle = -23.93f;  
    [Header("Throttle")]
    [SerializeField] private float deadZone = 0.05f; 
    [SerializeField] private BoatMovement boatMovement; private void Update()
    {
        float angle = NormalizeAngle(leverPivot.localEulerAngles.x);
        angle = Mathf.Clamp(angle, minAngle, maxAngle);

        float throttle = Mathf.InverseLerp(minAngle, maxAngle, angle);

        if (throttle < deadZone)
            throttle = 0f;

        if (boatMovement != null)
            boatMovement.SetThrottle(throttle);
    } private float NormalizeAngle(float angle)
    {
        if (angle > 180f)
            angle -= 360f;

        return angle;
    }
}
*/