using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class EventAmbientPlayer : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float targetVolume = 0.4f;
    [SerializeField] private float fadeInDuration = 1.5f;
    [SerializeField] private float fadeOutDuration = 1.0f;

    private AudioSource audioSource;
    private Coroutine fadeRoutine;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    public void PlayAmbient()
    {
        if (audioSource.clip == null)
        {
            Debug.LogWarning($"[{name}] Нет AudioClip у EventAmbientPlayer");
            return;
        }

        if (fadeRoutine != null)
            StopCoroutine(fadeRoutine);

        fadeRoutine = StartCoroutine(FadeIn());
    }

    public void StopAmbient()
    {
        if (fadeRoutine != null)
            StopCoroutine(fadeRoutine);

        fadeRoutine = StartCoroutine(FadeOut());
    }

    public void StopAmbientImmediate()
    {
        if (fadeRoutine != null)
            StopCoroutine(fadeRoutine);

        audioSource.volume = 0f;
        audioSource.Stop();
    }

    private IEnumerator FadeIn()
    {
        if (!audioSource.isPlaying)
            audioSource.Play();

        float startVolume = audioSource.volume;
        float time = 0f;

        while (time < fadeInDuration)
        {
            time += Time.deltaTime;
            float t = fadeInDuration > 0f ? time / fadeInDuration : 1f;
            audioSource.volume = Mathf.Lerp(startVolume, targetVolume, t);
            yield return null;
        }

        audioSource.volume = targetVolume;
    }

    private IEnumerator FadeOut()
    {
        float startVolume = audioSource.volume;
        float time = 0f;

        while (time < fadeOutDuration)
        {
            time += Time.deltaTime;
            float t = fadeOutDuration > 0f ? time / fadeOutDuration : 1f;
            audioSource.volume = Mathf.Lerp(startVolume, 0f, t);
            yield return null;
        }

        audioSource.volume = 0f;
        audioSource.Stop();
    }
}