using System.Collections.Generic;
using UnityEngine;

public class PlaySoundsOnStart : MonoBehaviour {
  [SerializeField] private List<string> soundNames = new();

  private SoundPlayer soundPlayer;

  void Start() {
    soundPlayer = GetComponent<SoundPlayer>();
    if (soundPlayer == null) {
      Debug.LogWarning("PlaySoundsOnStart: SoundPlayer not found");
      return;
    }

    foreach (var soundName in soundNames) {
      if (!string.IsNullOrWhiteSpace(soundName)) {
        soundPlayer.PlaySound(soundName);
      }
    }
  }
}
