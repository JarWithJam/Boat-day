using System.Reflection;
using UnityEngine;

public class BoatProbesKeyboardSmoothTest : MonoBehaviour
{
    [SerializeField] private MonoBehaviour boatScript;

    [Header("Limits")]
    [SerializeField] private float maxEnginePower = 5f;
    [SerializeField] private float maxTurnPower = 0.5f;

    [Header("Response")]
    [SerializeField] private float engineLerpSpeed = 3f;
    [SerializeField] private float turnLerpSpeed = 5f;

    private FieldInfo engineField;
    private FieldInfo turnField;
    private object boatInstance;

    private float currentEngine;
    private float currentTurn;

    private void Awake()
    {
        if (boatScript == null)
        {
            Debug.LogError("BoatProbesKeyboardSmoothTest: boatScript не назначен.");
            enabled = false;
            return;
        }

        boatInstance = boatScript;
        System.Type type = boatScript.GetType();

        engineField = type.GetField("_enginePower", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                   ?? type.GetField("enginePower", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

        turnField = type.GetField("_turnPower", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                 ?? type.GetField("turnPower", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
    }

    private void Update()
    {
        float targetEngine = Input.GetKey(KeyCode.W) ? maxEnginePower : 0f;

        float targetTurn = 0f;
        if (Input.GetKey(KeyCode.A)) targetTurn = -maxTurnPower;
        if (Input.GetKey(KeyCode.D)) targetTurn = maxTurnPower;

        currentEngine = Mathf.Lerp(currentEngine, targetEngine, Time.deltaTime * engineLerpSpeed);
        currentTurn = Mathf.Lerp(currentTurn, targetTurn, Time.deltaTime * turnLerpSpeed);

        if (engineField != null)
            engineField.SetValue(boatInstance, currentEngine);

        if (turnField != null)
            turnField.SetValue(boatInstance, currentTurn);

        Debug.Log($"Engine: {currentEngine:F2} | Turn: {currentTurn:F2}");
    }
}