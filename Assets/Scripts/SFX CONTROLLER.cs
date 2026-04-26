using UnityEngine;
using System.Collections.Generic;

public class SFXController : MonoBehaviour
{
    public static SFXController Instance;

    [System.Serializable]
    public class SFXGroup
    {
        [Header("Group Settings")]
        public string groupName;

        [Header("Audio Clips")]
        public AudioClip[] clips;

        [Header("Variation")]
        public Vector2 volumeRange = new Vector2(0.9f, 1f);
        public Vector2 pitchRange = new Vector2(0.95f, 1.05f);
    }

    [Header("Main Audio Source")]
    public AudioSource sfxSource;

    [Header("Sound Groups")]
    public List<SFXGroup> soundGroups = new List<SFXGroup>();

    private Dictionary<string, SFXGroup> groupLookup;

    void Awake()
    {
        // Singleton
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        BuildDictionary();
    }

    void BuildDictionary()
    {
        groupLookup = new Dictionary<string, SFXGroup>();

        foreach (SFXGroup group in soundGroups)
        {
            if (string.IsNullOrEmpty(group.groupName))
            {
                Debug.LogWarning("SFX Group has empty name.");
                continue;
            }

            if (!groupLookup.ContainsKey(group.groupName))
            {
                groupLookup.Add(group.groupName, group);
            }
            else
            {
                Debug.LogWarning("Duplicate SFX Group name: " + group.groupName);
            }
        }
    }

    public void Play(string groupName)
    {
        if (groupLookup == null)
            BuildDictionary();

        if (!groupLookup.ContainsKey(groupName))
        {
            Debug.LogWarning("SFX Group not found: " + groupName);
            return;
        }

        SFXGroup group = groupLookup[groupName];

        if (group.clips == null || group.clips.Length == 0)
        {
            Debug.LogWarning("No clips inside group: " + groupName);
            return;
        }

        AudioClip randomClip = group.clips[Random.Range(0, group.clips.Length)];

        float randomVolume = Random.Range(group.volumeRange.x, group.volumeRange.y);
        float randomPitch = Random.Range(group.pitchRange.x, group.pitchRange.y);

        sfxSource.pitch = randomPitch;
        sfxSource.PlayOneShot(randomClip, randomVolume);
        sfxSource.pitch = 1f;
    }

    public void PlayAtPosition(string groupName, Vector3 position)
    {
        if (groupLookup == null)
            BuildDictionary();

        if (!groupLookup.ContainsKey(groupName))
        {
            Debug.LogWarning("SFX Group not found: " + groupName);
            return;
        }

        SFXGroup group = groupLookup[groupName];

        if (group.clips == null || group.clips.Length == 0)
        {
            Debug.LogWarning("No clips inside group: " + groupName);
            return;
        }

        AudioClip randomClip = group.clips[Random.Range(0, group.clips.Length)];

        GameObject tempAudio = new GameObject("TempAudio");
        tempAudio.transform.position = position;

        AudioSource source = tempAudio.AddComponent<AudioSource>();

        source.clip = randomClip;
        source.volume = Random.Range(group.volumeRange.x, group.volumeRange.y);
        source.pitch = Random.Range(group.pitchRange.x, group.pitchRange.y);
        source.spatialBlend = 1f;

        source.Play();

        Destroy(tempAudio, randomClip.length + 0.2f);
    }

    public bool GroupExists(string groupName)
    {
        return groupLookup != null && groupLookup.ContainsKey(groupName);
    }
}