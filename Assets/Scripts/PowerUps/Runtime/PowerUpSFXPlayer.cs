using UnityEngine;

public class PowerUpSFXPlayer : MonoBehaviour {
  private PowerUpManager powerUpManager;
  private SoundPlayer soundPlayer;

  private void Awake() {
    powerUpManager = GetComponent<PowerUpManager>();
    if (powerUpManager == null) {
      Debug.LogError("PowerUpSFXPlayer: PowerUpManager not found");
      enabled = false;
      return;
    }

    soundPlayer = GetComponent<SoundPlayer>();
    if (soundPlayer == null) {
      Debug.LogError("PowerUpSFXPlayer: SoundPlayer not found");
      enabled = false;
      return;
    }

    powerUpManager.OnPowerUpUsed += HandlePowerUpUsed;
  }

  private void OnDestroy() {
    powerUpManager.OnPowerUpUsed -= HandlePowerUpUsed;
  }

  private void HandlePowerUpUsed(BasePowerUp powerUp) {
    if (powerUp == null) return;
    soundPlayer.PlaySound(powerUp.Name);
  }
}
