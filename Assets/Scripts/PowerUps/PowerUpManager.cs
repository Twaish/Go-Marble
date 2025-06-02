using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PowerUpManager : MonoBehaviour {
  private BasePowerUp currentPowerUp;
  private bool isCooldown;

  public event Action<BasePowerUp> OnPowerUpChanged;

  private void Update()
  {
    if (Input.GetKeyDown(KeyCode.P))
    {
      TryUsePowerUp();
    }
  }

  private void TryUsePowerUp() {
    if (TryGetComponent<PlayerController>(out var playerController)) {
      UsePowerUp(playerController);
    } else {
      Debug.LogWarning("No PlayerController found to apply PowerUp!");
    }
  }

  public void UsePowerUp(PlayerController player) {
    if (currentPowerUp == null || isCooldown || currentPowerUp.Uses <= 0)
      return;

    currentPowerUp.OnActivate(player);
    currentPowerUp.Effect(player);
    currentPowerUp.Uses--;

    if (currentPowerUp.Uses <= 0) {
      currentPowerUp = null;
    } else {
      StartCoroutine(HandleCooldown());
    }

    OnPowerUpChanged?.Invoke(currentPowerUp);
  }
  
  public void ClearPowerUp() {
    currentPowerUp = null;
    OnPowerUpChanged?.Invoke(currentPowerUp);
  }

  public bool HasPowerUp() {
    return currentPowerUp != null;
  }

  private IEnumerator HandleCooldown() {
    isCooldown = true;
    yield return new WaitForSeconds(currentPowerUp.Cooldown);
    isCooldown = false;
  }

  public void AssignPowerUp(BasePowerUp newPowerup) {
    if (newPowerup == null) return;
    currentPowerUp = Instantiate(newPowerup);
    OnPowerUpChanged?.Invoke(currentPowerUp);
  }
}