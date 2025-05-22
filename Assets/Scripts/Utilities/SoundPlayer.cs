using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NamedAudioClip {
  public string name;
  public AudioClip clip;
  [Range(0f, 1f)] public float defaultVolume = 1f;
  public bool isSpatial;
  [Min(0f)] public float delay = 0f;
}

public class SoundPlayer : MonoBehaviour {
  [SerializeField] private NamedAudioClip[] soundList;
  [SerializeField] private int initialPoolSize = 5;

  private readonly List<AudioSource> audioSourcePool = new();

  void Awake() {
    for (int i = 0; i < initialPoolSize; i++) {
      audioSourcePool.Add(CreateNewAudioSource());
    }
  }
  
  private AudioSource GetAvailableAudioSource() {
    foreach (var src in audioSourcePool) {
      if (!src.isPlaying)
        return src;
    }

    var newSrc = CreateNewAudioSource();
    audioSourcePool.Add(newSrc);
    return newSrc;
  }

  private AudioSource CreateNewAudioSource() {
    var src = gameObject.AddComponent<AudioSource>();
    src.playOnAwake = false;
    return src;
  }

  public void PlaySound(string soundName, float? volume = null) {
    bool found = false;

    foreach (var entry in soundList) {
      if (entry.name != soundName || entry.clip == null)
        continue;

      var src = GetAvailableAudioSource();
      src.clip = entry.clip;
      src.volume = Mathf.Clamp01(volume ?? entry.defaultVolume);
      src.spatialBlend = entry.isSpatial ? 1f : 0f;
      
      if (entry.delay > 0f)
        src.PlayDelayed(entry.delay);
      else
        src.Play();

      found = true;
    }

    if (!found) {
      Debug.LogWarning($"SoundPlayer: No clips found with name '{soundName}'");
    }
  }
}
