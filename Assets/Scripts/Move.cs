using UnityEngine;
/*
public class BoatMovement : MonoBehaviour
{
    [SerializeField] private float maxSpeed = 8f;
    [SerializeField] private float acceleration = 2f;
    [SerializeField] private float deceleration = 3f;
    [SerializeField] private float maxTurnRate = 35f;
    [SerializeField] private float steeringSmoothness = 2f;
    [SerializeField] private float minSpeedForTurning = 0.2f;

    private float throttle;
    private float currentSpeed;
    private float targetSteering;
    private float currentSteering;

    public void SetThrottle(float value)
    {
        throttle = Mathf.Clamp01(value);
    }

    public void SetSteering(float value)
    {
        targetSteering = Mathf.Clamp(value, -1f, 1f);
    }

    private void Update()
    {
        float targetSpeed = throttle * maxSpeed;
        float speedChangeRate = throttle > 0f ? acceleration : deceleration;

        currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, speedChangeRate * Time.deltaTime);
        currentSteering = Mathf.Lerp(currentSteering, targetSteering, steeringSmoothness * Time.deltaTime);

        if (currentSpeed > minSpeedForTurning)
        {
            float speedFactor = currentSpeed / maxSpeed;
            float turnAmount = currentSteering * maxTurnRate * speedFactor * Time.deltaTime;
            transform.Rotate(0f, turnAmount, 0f);
        }

        transform.position += transform.forward * currentSpeed * Time.deltaTime;
    }
}
*/