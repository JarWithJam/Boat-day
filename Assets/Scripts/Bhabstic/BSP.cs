using UnityEngine;

public class BoatStateProvider : MonoBehaviour
{
    [Header("Source")]
    [SerializeField] private Transform tiltSource;
    [SerializeField] private Rigidbody boatRigidbody;

    [Header("Read only")]
    [SerializeField] private float currentPitch;
    [SerializeField] private float currentRoll;
    [SerializeField] private float currentTiltAmount;
    [SerializeField] private float currentSpeed;

    public float CurrentPitch => currentPitch;
    public float CurrentRoll => currentRoll;
    public float CurrentTiltAmount => currentTiltAmount;
    public float CurrentSpeed => currentSpeed;

    private void Reset()
    {
        tiltSource = transform;
        boatRigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (tiltSource != null)
        {
            currentPitch = NormalizeAngle(tiltSource.eulerAngles.x);
            currentRoll = NormalizeAngle(tiltSource.eulerAngles.z);
            currentTiltAmount = Mathf.Max(Mathf.Abs(currentPitch), Mathf.Abs(currentRoll));
        }

        if (boatRigidbody != null)
        {
            currentSpeed = boatRigidbody.linearVelocity.magnitude;
        }
    }

    private float NormalizeAngle(float angle)
    {
        while (angle > 180f) angle -= 360f;
        while (angle < -180f) angle += 360f;
        return angle;
    }
}