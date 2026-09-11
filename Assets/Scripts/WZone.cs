using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class WheelHandDriver : MonoBehaviour
{
    [Header("Wheel Setup")]
    [SerializeField] private Transform wheelVisual;
    [SerializeField] private Transform wheelCenter;
    [SerializeField] private Vector3 wheelPlaneNormalLocal = Vector3.forward;
    [SerializeField] private float minAngle = -120f;
    [SerializeField] private float maxAngle = 120f;

    [Header("Input")]
    [SerializeField] private InputActionProperty leftGrip;
    [SerializeField] private InputActionProperty rightGrip;
    [SerializeField] private float gripThreshold = 0.5f;

    [Header("Debug")]
    [SerializeField] private bool invert = false;

    private Transform hoveringHand;
    private XRBaseInteractor hoveringInteractor;

    private bool isHeld;
    private XRBaseInteractor activeInteractor;

    private float currentAngle;
    private float grabHandStartAngle;
    private float wheelStartAngle;

    private void Awake()
    {
        if (wheelCenter == null)
            wheelCenter = transform;
    }

    private void Update()
    {
        if (!isHeld)
        {
            TryBeginHold();
            return;
        }

        if (!IsGripPressed(activeInteractor))
        {
            EndHold();
            return;
        }

        UpdateWheelFromHand();
    }

    private void TryBeginHold()
    {
        if (hoveringInteractor == null || hoveringHand == null)
            return;

        if (!IsGripPressed(hoveringInteractor))
            return;

        isHeld = true;
        activeInteractor = hoveringInteractor;

        grabHandStartAngle = GetHandAngle(activeInteractor.transform);
        wheelStartAngle = currentAngle;
    }

    private void EndHold()
    {
        isHeld = false;
        activeInteractor = null;
    }

    private void UpdateWheelFromHand()
    {
        if (activeInteractor == null)
            return;

        float currentHandAngle = GetHandAngle(activeInteractor.transform);
        float delta = Mathf.DeltaAngle(grabHandStartAngle, currentHandAngle);

        if (invert)
            delta = -delta;

        currentAngle = Mathf.Clamp(wheelStartAngle + delta, minAngle, maxAngle);

        ApplyWheelAngle(currentAngle);
    }

    private float GetHandAngle(Transform handTransform)
    {
        Vector3 worldNormal = wheelCenter.TransformDirection(wheelPlaneNormalLocal.normalized);
        Vector3 center = wheelCenter.position;
        Vector3 toHand = handTransform.position - center;

        Vector3 projected = Vector3.ProjectOnPlane(toHand, worldNormal).normalized;
        if (projected.sqrMagnitude < 0.0001f)
            return 0f;

        Vector3 reference = wheelCenter.up;
        reference = Vector3.ProjectOnPlane(reference, worldNormal).normalized;

        if (reference.sqrMagnitude < 0.0001f)
            reference = wheelCenter.right;

        float angle = Vector3.SignedAngle(reference, projected, worldNormal);
        return angle;
    }

    private void ApplyWheelAngle(float angle)
    {
        if (wheelVisual == null)
            return;

        wheelVisual.localRotation = Quaternion.Euler(0f, 0f, angle);
    }

    private bool IsGripPressed(XRBaseInteractor interactor)
    {
        if (interactor == null)
            return false;

        string n = interactor.name.ToLower();

        if (n.Contains("left"))
            return leftGrip.action != null && leftGrip.action.ReadValue<float>() > gripThreshold;

        if (n.Contains("right"))
            return rightGrip.action != null && rightGrip.action.ReadValue<float>() > gripThreshold;

        return false;
    }

    public float GetSteeringNormalized()
    {
        float maxAbs = Mathf.Max(Mathf.Abs(minAngle), Mathf.Abs(maxAngle));
        if (maxAbs <= 0.001f)
            return 0f;

        return Mathf.Clamp(currentAngle / maxAbs, -1f, 1f);
    }

    public void RegisterHover(XRBaseInteractor interactor)
    {
        hoveringInteractor = interactor;
        hoveringHand = interactor != null ? interactor.transform : null;
    }

    public void UnregisterHover(XRBaseInteractor interactor)
    {
        if (hoveringInteractor == interactor)
        {
            hoveringInteractor = null;
            hoveringHand = null;
        }

        if (activeInteractor == interactor)
            EndHold();
    }
}