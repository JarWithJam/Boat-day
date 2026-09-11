using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class LeverHandDriver : MonoBehaviour
{
    [Header("Lever Setup")]
    [SerializeField] private Transform leverVisual;
    [SerializeField] private Transform leverPivot;
    [SerializeField] private Vector3 leverAxisLocal = Vector3.right;
    [SerializeField] private float minAngle = 0f;
    [SerializeField] private float maxAngle = 60f;

    [Header("Input")]
    [SerializeField] private InputActionProperty leftGrip;
    [SerializeField] private InputActionProperty rightGrip;
    [SerializeField] private float gripThreshold = 0.5f;

    [Header("Tuning")]
    [SerializeField] private float handToAngleScale = 120f;
    [SerializeField] private bool invert = false;

    private XRBaseInteractor hoveringInteractor;
    private bool isHeld;
    private XRBaseInteractor activeInteractor;

    private Vector3 localAxisNormalized;
    private float currentAngle;
    private float startHandSignedDistance;
    private float startLeverAngle;

    private void Awake()
    {
        if (leverPivot == null)
            leverPivot = transform;

        localAxisNormalized = leverAxisLocal.normalized;
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

        UpdateLeverFromHand();
    }

    private void TryBeginHold()
    {
        if (hoveringInteractor == null)
            return;

        if (!IsGripPressed(hoveringInteractor))
            return;

        isHeld = true;
        activeInteractor = hoveringInteractor;

        startHandSignedDistance = GetHandSignedDistance(activeInteractor.transform);
        startLeverAngle = currentAngle;
    }

    private void EndHold()
    {
        isHeld = false;
        activeInteractor = null;
    }

    private void UpdateLeverFromHand()
    {
        if (activeInteractor == null)
            return;

        float currentDistance = GetHandSignedDistance(activeInteractor.transform);
        float deltaDistance = currentDistance - startHandSignedDistance;

        float deltaAngle = deltaDistance * handToAngleScale;
        if (invert)
            deltaAngle = -deltaAngle;

        currentAngle = Mathf.Clamp(startLeverAngle + deltaAngle, minAngle, maxAngle);
        ApplyLeverAngle(currentAngle);
    }

    private float GetHandSignedDistance(Transform hand)
    {
        Vector3 worldAxis = leverPivot.TransformDirection(localAxisNormalized);
        Vector3 toHand = hand.position - leverPivot.position;
        return Vector3.Dot(toHand, worldAxis);
    }

    private void ApplyLeverAngle(float angle)
    {
        if (leverVisual == null)
            return;

        leverVisual.localRotation = Quaternion.Euler(angle, 0f, 0f);
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

    public float GetThrottle01()
    {
        return Mathf.InverseLerp(minAngle, maxAngle, currentAngle);
    }

    public void RegisterHover(XRBaseInteractor interactor)
    {
        hoveringInteractor = interactor;
    }

    public void UnregisterHover(XRBaseInteractor interactor)
    {
        if (hoveringInteractor == interactor)
            hoveringInteractor = null;

        if (activeInteractor == interactor)
            EndHold();
    }
}