using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class HandHoverZone : MonoBehaviour
{
    [SerializeField] private WheelHandDriver wheelDriver;
    [SerializeField] private LeverHandDriver leverDriver;

    private void OnTriggerEnter(Collider other)
    {
        XRBaseInteractor interactor = other.GetComponentInParent<XRBaseInteractor>();
        if (interactor == null)
            return;

        if (wheelDriver != null)
            wheelDriver.RegisterHover(interactor);

        if (leverDriver != null)
            leverDriver.RegisterHover(interactor);
    }

    private void OnTriggerExit(Collider other)
    {
        XRBaseInteractor interactor = other.GetComponentInParent<XRBaseInteractor>();
        if (interactor == null)
            return;

        if (wheelDriver != null)
            wheelDriver.UnregisterHover(interactor);

        if (leverDriver != null)
            leverDriver.UnregisterHover(interactor);
    }
}