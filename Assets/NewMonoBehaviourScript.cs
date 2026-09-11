using UnityEngine;

public class ForceDarkURPLit_AllChildren : MonoBehaviour
{
    [Range(0f, 1f)] public float smoothness = 0.1f;
    [Range(0f, 1f)] public float metallic = 0.0f;

    void Awake()
    {
        foreach (var r in GetComponentsInChildren<Renderer>(true))
        {
            var m = r.material; // создаёт инстанс
            if (!m) continue;

            if (m.HasProperty("_Smoothness")) m.SetFloat("_Smoothness", smoothness);
            if (m.HasProperty("_Metallic")) m.SetFloat("_Metallic", metallic);

            if (m.HasProperty("_SpecularHighlights")) m.SetFloat("_SpecularHighlights", 0f);
            if (m.HasProperty("_EnvironmentReflections")) m.SetFloat("_EnvironmentReflections", 0f);
        }
    }
}
