using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PowerUpUIController : MonoBehaviour
{
  [SerializeField] private Image powerupIconImage;
  [SerializeField] private TextMeshProUGUI usesText;
  [SerializeField] private TextMeshProUGUI nameText;

  private PowerUpManager powerUpManager;

  private void Awake()
  {
    powerUpManager = GetComponent<PowerUpManager>();
    if (powerUpManager == null)
    {
      Debug.LogError("PowerUpUIController: PowerUpManager not found");
      enabled = false;
      return;
    }
    powerUpManager.OnPowerUpChanged += UpdateUI;
  }

  private void Oestroy()
  {
    powerUpManager.OnPowerUpChanged -= UpdateUI;    
  }

  private void UpdateUI(BasePowerUp currentPowerUp) {
    if (currentPowerUp != null) {
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
}
