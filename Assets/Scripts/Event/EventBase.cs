using System.Collections;
using UnityEngine;

public abstract class BaseEvent : MonoBehaviour
{
    [Header("Base Event Settings")]
    public string eventId = "event";
    public float weight = 1f;
    public float cooldown = 20f;

    protected float lastTriggerTime = -999f;

    public bool IsRunning { get; private set; }

    public virtual bool CanStart()
    {
        if (IsRunning)
            return false;

        if (Time.time < lastTriggerTime + cooldown)
            return false;

        return true;
    }

    public IEnumerator ExecuteEvent()
    {
        if (!CanStart())
            yield break;

        lastTriggerTime = Time.time;
        IsRunning = true;

        yield return RunEvent();

        IsRunning = false;
    }

    protected abstract IEnumerator RunEvent();
}