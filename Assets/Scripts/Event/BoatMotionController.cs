using System.Collections;
using UnityEngine;

public class BoatMotionController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody targetRigidbody;

    public Rigidbody TargetRigidbody => targetRigidbody;

    public void AddImpulse(Vector3 worldForce)
    {
        if (targetRigidbody == null)
            return;

        targetRigidbody.AddForce(worldForce, ForceMode.Impulse);
    }

    public void AddTorque(Vector3 worldTorque)
    {
        if (targetRigidbody == null)
            return;

        targetRigidbody.AddTorque(worldTorque, ForceMode.Impulse);
    }

    public IEnumerator RockBoat(float duration, float torqueStrength, float interval)
    {
        if (targetRigidbody == null)
            yield break;

        float timer = 0f;

        while (timer < duration)
        {
            float dir = Random.value > 0.5f ? 1f : -1f;

            // качка по крену
            Vector3 torque = transform.forward * dir * torqueStrength;
            targetRigidbody.AddTorque(torque, ForceMode.Impulse);

            yield return new WaitForSeconds(interval);
            timer += interval;
        }
    }
}