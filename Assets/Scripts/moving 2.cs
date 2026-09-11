using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BoatMovement : MonoBehaviour
{
    [Header("Forward Movement")]
    [SerializeField] private float accelerationForce = 80f;
    [SerializeField] private float maxForwardSpeed = 8f;
    [SerializeField] private float waterDragMultiplier = 0.2f;

    [Header("Steering")]
    [SerializeField] private float turnTorque = 25f;
    [SerializeField] private float minSpeedForSteering = 0.2f;

    private Rigidbody rb;

    private float throttle;
    private float steering;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void SetThrottle(float value)
    {
        throttle = Mathf.Clamp01(value);
    }

    public void SetSteering(float value)
    {
        steering = Mathf.Clamp(value, -1f, 1f);
    }

    private void FixedUpdate()
    {
        Vector3 velocity = rb.linearVelocity;
        Vector3 flatVelocity = Vector3.ProjectOnPlane(velocity, Vector3.up);

        float forwardSpeed = Vector3.Dot(flatVelocity, transform.forward);
        float sideSpeed = Vector3.Dot(flatVelocity, transform.right);

        if (forwardSpeed < maxForwardSpeed)
        {
            rb.AddForce(transform.forward * throttle * accelerationForce, ForceMode.Acceleration);
        }

        if (Mathf.Abs(sideSpeed) > 0.01f)
        {
            Vector3 sideDragForce = -transform.right * sideSpeed * waterDragMultiplier;
            rb.AddForce(sideDragForce, ForceMode.Acceleration);
        }

        if (Mathf.Abs(forwardSpeed) > minSpeedForSteering)
        {
            rb.AddTorque(Vector3.up * steering * turnTorque, ForceMode.Acceleration);
        }

        Debug.Log(
            $"Throttle: {throttle:F2} | Steering: {steering:F2} | ForwardSpeed: {forwardSpeed:F2} | SideSpeed: {sideSpeed:F2}"
        );
    }
}
