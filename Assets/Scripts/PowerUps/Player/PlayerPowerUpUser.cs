using UnityEngine;

public class PlayerPowerUpUser : MonoBehaviour
{
  private PowerUpManager powerUpManager;
  private PlayerController player;

  private void Awake()
  {
    player = GetComponent<PlayerController>();
    if (player == null)
    {
      Debug.LogError("PlayerPowerUpUser: PlayerController not found");
      enabled = false;
      return;
    }

    powerUpManager = FindFirstObjectByType<PowerUpManager>();
    if (powerUpManager == null)
    {
      Debug.LogError("PlayerPowerUpUser: PowerUpManager not found in scene");
      enabled = false;
    }
  }

  private void Start()
  {
  }

  private void Update()
  {
    if (Input.GetKeyDown(KeyCode.P))
    {
      if (player != null)
      {
        powerUpManager.UsePowerUp(player);
      }
    }
  }

  public void GivePowerUp(BasePowerUp powerUp, bool overwrite = true)
  {
    if (!overwrite && powerUpManager.HasPowerUp()) return;

    powerUpManager.AssignPowerUp(powerUp);
  }
}
