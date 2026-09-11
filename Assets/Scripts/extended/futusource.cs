using UnityEngine;

public class CapsuleMotionSource : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private Transform referenceSpace;
    [SerializeField] private Rigidbody rb;

    [Header("Base Tilt")]
    [SerializeField] private float tiltMultiplier = 0.5f;

    [Header("Limits")]
    [SerializeField] private float maxPitch = 6f;
    [SerializeField] private float maxRoll = 6f;

    [Header("Dynamics")]
    [SerializeField] private float angularInfluence = 0.3f;
    [SerializeField] private float accelerationInfluence = 0.1f;

    [Header("Smoothing")]
    [SerializeField] private float smoothTime = 0.25f;

    [Header("Speed Limit")]
    [SerializeField] private float maxDegPerSec = 40f;

    public float Pitch { get; private set; }
    public float Roll { get; private set; }

    private float _pitchVel;
    private float _rollVel;

    private float _currentPitch;
    private float _currentRoll;

    private Vector3 _prevVelocity;

    private void Start()
    {
        if (rb != null)
            _prevVelocity = rb.linearVelocity;

        if (referenceSpace == null)
        {
            GameObject go = new GameObject("WorldReference");
            referenceSpace = go.transform;
            referenceSpace.rotation = Quaternion.identity;
        }
    }

    private void FixedUpdate()
    {
        Vector3 localUp = referenceSpace.InverseTransformDirection(transform.up);

        float targetRoll = Mathf.Asin(localUp.x) * Mathf.Rad2Deg;
        float targetPitch = -Mathf.Asin(localUp.z) * Mathf.Rad2Deg;

        targetPitch *= tiltMultiplier;
        targetRoll *= tiltMultiplier;

        // добавляем динамику
        if (rb != null)
        {
            Vector3 angVel = transform.InverseTransformDirection(rb.angularVelocity);
            targetPitch += angVel.x * Mathf.Rad2Deg * angularInfluence;
            targetRoll += -angVel.z * Mathf.Rad2Deg * angularInfluence;

            Vector3 acc = (rb.linearVelocity - _prevVelocity) / Time.fixedDeltaTime;
            Vector3 localAcc = transform.InverseTransformDirection(acc);

            targetPitch += -localAcc.z * accelerationInfluence;
            targetRoll += localAcc.x * accelerationInfluence;

            _prevVelocity = rb.linearVelocity;
        }

        // мягкие лимиты
        targetPitch = Mathf.Clamp(targetPitch, -maxPitch, maxPitch);
        targetRoll = Mathf.Clamp(targetRoll, -maxRoll, maxRoll);

        // сглаживание
        float smoothPitch = Mathf.SmoothDamp(_currentPitch, targetPitch, ref _pitchVel, smoothTime);
        float smoothRoll = Mathf.SmoothDamp(_currentRoll, targetRoll, ref _rollVel, smoothTime);

        // жесткий лимит скорости (САМОЕ ВАЖНОЕ)
        float dt = Time.fixedDeltaTime;

        _currentPitch = Mathf.MoveTowards(_currentPitch, smoothPitch, maxDegPerSec * dt);
        _currentRoll = Mathf.MoveTowards(_currentRoll, smoothRoll, maxDegPerSec * dt);

        Pitch = _currentPitch;
        Roll = _currentRoll;
    }
}   