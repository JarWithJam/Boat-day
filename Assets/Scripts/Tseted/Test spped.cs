using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BoatEngineTest : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Transform enginePoint;
    [SerializeField] private float engineForce = 20f;

    private void Reset()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Awake()
    {
        if (rb == null)
            rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.W) && enginePoint != null)
        {
            rb.AddForceAtPosition(
                enginePoint.forward * engineForce,
                enginePoint.position,
                ForceMode.Force
            );
        }

        Debug.Log($"W: {Input.GetKey(KeyCode.W)} | Speed: {rb.linearVelocity.magnitude:F2}");
    }
}