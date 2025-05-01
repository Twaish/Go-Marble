using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class PowerUpManager : MonoBehaviour {
  private BasePowerUp currentPowerUp;
  private bool isCooldown;

  [SerializeField]
  private Image powerupIconImage;
  [SerializeField]
  private TextMeshProUGUI usesText;
  [SerializeField]
  private TextMeshProUGUI nameText;

  private void Update() {
    if (Input.GetKeyDown(KeyCode.P)) {
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
      ClearPowerUp();
    } else {
      StartCoroutine(HandleCooldown());
    }
    UpdateGUI();
  }

  public bool HasPowerUp() {
    return currentPowerUp != null;
  }

  private void UpdateGUI() {
    if (currentPowerUp) {
      powerupIconImage.sprite = currentPowerUp.Icon;
      powerupIconImage.enabled = true;
      nameText.text = currentPowerUp.Name;
      usesText.text = $"x{currentPowerUp.Uses}";
    } else {
      powerupIconImage.sprite = null;
      powerupIconImage.enabled = false;
      nameText.text = "";
      usesText.text = "";
    }
  }

  private IEnumerator HandleCooldown() {
    isCooldown = true;
    yield return new WaitForSeconds(currentPowerUp.Cooldown);
    isCooldown = false;
  }

  public void AssignPowerUp(BasePowerUp newPowerup) {
    if (!newPowerup) return;
    currentPowerUp = Instantiate(newPowerup);
    UpdateGUI();
  }
  private void ClearPowerUp() {
    currentPowerUp = null;
  }
}