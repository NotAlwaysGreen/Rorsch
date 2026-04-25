using UnityEngine;

[System.Serializable]
public class MusicLayer
{
    public AudioSource source;

    [Range(0f, 1f)]
    public float startIntensity = 0f;

    [Range(0f, 1f)]
    public float fullIntensity = 1f;
}

public class MusicLayerController : MonoBehaviour
{
    [Header("Insanity Source (0 - 1)")]
    public float insanityValue;

    [Header("Music Layers")]
    public MusicLayer[] layers;

    [Header("Smoothing")]
    public float fadeSpeed = 2f;

    void Start()
    {
        // Start all layers playing immediately
        foreach (var layer in layers)
        {
            if (layer.source != null)
            {
                layer.source.loop = true;

                if (!layer.source.isPlaying)
                    layer.source.Play();

                layer.source.volume = 0f;
            }
        }
    }

    void Update()
    {
        UpdateMusicLayers();
    }

    void UpdateMusicLayers()
    {
        foreach (var layer in layers)
        {
            if (layer.source == null) continue;

            float targetVolume = CalculateLayerVolume(layer);

            // Smooth fade
            layer.source.volume = Mathf.Lerp(
                layer.source.volume,
                targetVolume,
                Time.deltaTime * fadeSpeed
            );
        }
    }

    float CalculateLayerVolume(MusicLayer layer)
    {
        float i = insanityValue;

        // before start threshold
        if (i <= layer.startIntensity)
            return 0f;

        // after full threshold
        if (i >= layer.fullIntensity)
            return 1f;

        // normalized fade between thresholds
        float t = Mathf.InverseLerp(layer.startIntensity, layer.fullIntensity, i);
        return t;
    }

    // Call this from your insanity system
    public void SetInsanity(float value)
    {
        insanityValue = Mathf.Clamp01(value);
    }
}