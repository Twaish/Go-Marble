using UnityEngine;

public class PlayerPowerUpUser : MonoBehaviour {
  private PowerUpManager powerUpManager;
  private PlayerController player;
  private PlayerControls playerControls;

  private void Awake() {
    player = GetComponent<PlayerController>();
    if (player == null) {
      Debug.LogError("PlayerPowerUpUser: PlayerController not found");
      enabled = false;
      return;
    }

    playerControls = GetComponent<PlayerControls>();
    if (playerControls == null) {
      Debug.LogError("PlayerPowerUpUser: PlayerControls not found");
      enabled = false;
      return;
    }

    powerUpManager = FindFirstObjectByType<PowerUpManager>();
    if (powerUpManager == null) {
      Debug.LogError("PlayerPowerUpUser: PowerUpManager not found in scene");
      enabled = false;
    }

    playerControls.OnUsePowerUp += UsePowerUp;
  }

  private void OnDestroy() {
    playerControls.OnUsePowerUp -= UsePowerUp;
  }

  private void UsePowerUp() {
    powerUpManager.UsePowerUp(player);
  }

  public void GivePowerUp(BasePowerUp powerUp, bool overwrite = true) {
    if (!overwrite && powerUpManager.HasPowerUp()) return;

    powerUpManager.AssignPowerUp(powerUp);
  }
}
