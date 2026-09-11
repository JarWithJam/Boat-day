using UnityEngine;
using Bhaptics.SDK2;

public class BoatHapticsController : MonoBehaviour
{
    public static BoatHapticsController Instance;

    private void Awake()
    {
        Instance = this;
    }

    // 🌊 легкая качка
    public void PlayLight()
    {
        BhapticsLibrary.Play("boat_sway_light", 0, 0.5f, 1f);
    }

    // 🌊 сильная качка
    public void PlayHeavy()
    {
        BhapticsLibrary.Play("boat_sway_heavy", 0, 1f, 1f);
    }

    // 💥 событие (удар)
    public void PlayImpact()
    {
        BhapticsLibrary.Play("boat_event_impact", 0, 1.2f, 1f);
    }
}