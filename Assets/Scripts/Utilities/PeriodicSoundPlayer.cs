using UnityEngine;

public class PeriodicSoundPlayer : MonoBehaviour {
  [SerializeField] private string soundName;
  [SerializeField] private float intervalSeconds = 5f;

  private SoundPlayer soundPlayer;
  private float timer;

  void Start() {
    soundPlayer = GetComponent<SoundPlayer>();
    if (soundPlayer == null) {
      Debug.LogError("PeriodicSoundPlayer: SoundPlayer not found");
      enabled = false;
      return;
    }

    if (string.IsNullOrEmpty(soundName)) {
      Debug.LogWarning("PeriodicSoundPlayer: No sound name provided");
      enabled = false;
      return;
    }

    timer = intervalSeconds;
  }

  void Update() {
    timer -= Time.deltaTime;
    if (timer <= 0f) {
      soundPlayer.PlaySound(soundName);
      timer = intervalSeconds;
    }
  }

  public void SetInterval(float newInterval) {
    intervalSeconds = Mathf.Max(0.1f, newInterval);
    timer = intervalSeconds;
  }
}
