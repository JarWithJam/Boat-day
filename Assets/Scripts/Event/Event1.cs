using System.Collections;
using UnityEngine;

public class SideImpactEvent : BaseEvent
{
    [Header("References")]
    [SerializeField] private BoatMotionController boatMotion;
    // [SerializeField] private VestController vestController;

    [Header("Impact Settings")]
    [SerializeField] private float sideForce = 2.5f;
    [SerializeField] private float rollTorque = 1.5f;
    [SerializeField] private float eventDuration = 0.5f;

    [Header("Audio")]
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioClip impactSfx;

    protected override IEnumerator RunEvent()
    {
        if (boatMotion == null || boatMotion.TargetRigidbody == null)
            yield break;

        if (sfxSource != null && impactSfx != null)
            sfxSource.PlayOneShot(impactSfx);

        Transform boatTransform = boatMotion.TargetRigidbody.transform;
        float direction = Random.value > 0.5f ? 1f : -1f;

        Vector3 sideImpulse = boatTransform.right * direction * sideForce;
        Vector3 roll = boatTransform.forward * -direction * rollTorque;

        BoatHapticsController.Instance.PlayImpact();
        boatMotion.AddImpulse(sideImpulse);
        boatMotion.AddTorque(roll);

        // if (vestController != null)
        //     yield return StartCoroutine(vestController.DoublePulse(0.8f, 0.12f, 0.15f));

        yield return new WaitForSeconds(eventDuration);
    }
}