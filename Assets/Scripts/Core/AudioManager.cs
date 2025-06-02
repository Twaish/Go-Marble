using UnityEngine;

public class AudioManager {
  [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
  static void Init() {
    AudioListener.volume = SettingsManager.Instance.Settings.masterVolume;

    SettingsManager.Instance.OnSettingsChanged += ApplyVolume;
  }

  private static void ApplyVolume(GameSettings settings) {
    AudioListener.volume = Mathf.Clamp01(settings.masterVolume);
  }
}
