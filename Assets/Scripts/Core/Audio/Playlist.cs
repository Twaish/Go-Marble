using System.Collections.Generic;
using UnityEngine;

public class Playlist : MonoBehaviour {
  [Tooltip("Ordered list of sound names matching those in SoundPlayer")]
  [SerializeField] private List<string> trackNames = new();

  [SerializeField] private bool loop = true;
  [SerializeField] private bool shuffle = true;

  private int currentIndex = 0;
  private List<string> shuffledTracks;

  private SoundPlayer soundPlayer;

  private void Awake() {
    soundPlayer = GetComponent<SoundPlayer>();
    if (soundPlayer == null) {
      Debug.LogError("Playlist: SoundPlayer not found");
      enabled = false;
      return;
    }
  }

  private void Start() {
    if (shuffle) {
      shuffledTracks = new List<string>(trackNames);
      ShuffleList(shuffledTracks);
    }
    PlayCurrent();
  }

  public void PlayCurrent() {
    string track = GetCurrentTrackName();
    if (!string.IsNullOrEmpty(track)) {
      soundPlayer.PlaySoundWithCallback(track, PlayNext);
    }
  }

  public void PlayNext() {
    if (shuffle) {
      currentIndex = (currentIndex + 1) % shuffledTracks.Count;
      soundPlayer.PlaySoundWithCallback(shuffledTracks[currentIndex], PlayNext);
    } 
    else {
      currentIndex++;
      if (currentIndex >= trackNames.Count) {
        if (!loop) return;
        currentIndex = 0;
      }
      soundPlayer.PlaySoundWithCallback(trackNames[currentIndex], PlayNext);
    }
  }

  public string GetCurrentTrackName() {
    if (shuffle && shuffledTracks != null && shuffledTracks.Count > 0)
      return shuffledTracks[currentIndex];

    if (trackNames.Count == 0) return null;
    return trackNames[Mathf.Clamp(currentIndex, 0, trackNames.Count - 1)];
  }

  private void ShuffleList(List<string> list) {
    for (int i = 0; i < list.Count; i++) {
      int randomIndex = Random.Range(i, list.Count);
      (list[i], list[randomIndex]) = (list[randomIndex], list[i]);
    }
  }
}
