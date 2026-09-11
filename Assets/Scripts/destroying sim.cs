/*
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Simulation;

public class DisableXRDeviceSimulator : MonoBehaviour
{
    private void Start()
    {
        XRDeviceSimulator simulator = FindFirstObjectByType<XRDeviceSimulator>();

        if (simulator != null)
        {
            Destroy(simulator.gameObject);
            Debug.Log("XR Device Simulator destroyed.");
        }
        else
        {
            Debug.Log("XR Device Simulator not found.");
        }
    }
}
*/