using UnityEngine;
using Futurift;
using Futurift.DataSenders;
using Futurift.Options;

/// Безопасный вывод в реальную капсулу Futurift
/// Принимает УЖЕ рассчитанные углы (pitch/roll) и делает:
/// - двойной clamp
/// - ограничение скорости
/// - fail-safe
public class FuturiftMotionOutput : MonoBehaviour
{
    [Header("Connection")]
    [SerializeField] private string ip = "127.0.0.1";
    [SerializeField] private int port = 6065;
    [SerializeField] private int interval = 50;

    [Header("Hard Limits (DEVICE)")]
    [SerializeField] private float pitchMin = -15f;
    [SerializeField] private float pitchMax = 21f;
    [SerializeField] private float rollMin = -18f;
    [SerializeField] private float rollMax = 18f;

    [Header("Soft Limits (SAFE ZONE)")]
    [SerializeField] private float pitchSoftLimit = 8f;
    [SerializeField] private float rollSoftLimit = 8f;

    [Header("Rate Limit (CRITICAL)")]
    [Tooltip("Максимальная скорость изменения (град/сек)")]
    [SerializeField] private float maxDegPerSecPitch = 40f;
    [SerializeField] private float maxDegPerSecRoll = 40f;

    [Header("Deadzone (фильтр мелкой тряски)")]
    [SerializeField] private float deadzone = 0.1f;

    [Header("Input")]
    public float targetPitchDeg;
    public float targetRollDeg;

    private FutuRiftController controller;

    // текущее безопасное состояние
    private float outPitch;
    private float outRoll;

    private bool initialized;

    private void Awake()
    {
        try
        {
            var udpOptions = new UdpOptions
            {
                ip = ip,
                port = port
            };

            var futuOptions = new FutuRiftOptions
            {
                interval = interval
            };

            controller = new FutuRiftController(
                new UdpPortSender(udpOptions),
                futuOptions
            );

            initialized = true;
        }
        catch (System.Exception e)
        {
            Debug.LogError("Futurift init failed: " + e.Message);
            initialized = false;
        }
    }

    private void OnEnable()
    {
        if (initialized)
            controller.Start();
    }

    private void FixedUpdate()
    {
        if (!initialized || controller == null)
            return;

        // =========================
        // 1. DEADZONE (убираем дрожь)
        // =========================
        float tp = Mathf.Abs(targetPitchDeg) < deadzone ? 0f : targetPitchDeg;
        float tr = Mathf.Abs(targetRollDeg) < deadzone ? 0f : targetRollDeg;

        // =========================
        // 2. SOFT LIMIT (бережём механику)
        // =========================
        tp = Mathf.Clamp(tp, -pitchSoftLimit, pitchSoftLimit);
        tr = Mathf.Clamp(tr, -rollSoftLimit, rollSoftLimit);

        // =========================
        // 3. RATE LIMIT (самое важное)
        // =========================
        float dt = Time.fixedDeltaTime;

        outPitch = Mathf.MoveTowards(outPitch, tp, maxDegPerSecPitch * dt);
        outRoll = Mathf.MoveTowards(outRoll, tr, maxDegPerSecRoll * dt);

        // =========================
        // 4. HARD LIMIT (железо)
        // =========================
        float finalPitch = Mathf.Clamp(outPitch, pitchMin, pitchMax);
        float finalRoll = Mathf.Clamp(outRoll, rollMin, rollMax);

        // =========================
        // 5. SEND
        // =========================
        controller.Pitch = finalPitch;
        controller.Roll = finalRoll;
    }

    private void OnDisable()
    {
        SafeStop();
    }

    private void OnApplicationQuit()
    {
        SafeStop();
    }

    private void SafeStop()
    {
        if (controller == null) return;

        // мягкий возврат в ноль
        controller.Pitch = 0f;
        controller.Roll = 0f;

        controller.Stop();
    }
}