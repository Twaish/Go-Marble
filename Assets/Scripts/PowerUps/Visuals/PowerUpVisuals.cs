using UnityEngine;
using UnityEngine.UI;

public class PowerUpVisuals : MonoBehaviour {
  [SerializeField] private Image icon;
  [SerializeField] private Light pointLight;

  public void Setup(BasePowerUp powerUp) {
    icon.sprite = powerUp.Icon;
    icon.color = powerUp.PowerUpColor;
    pointLight.color = powerUp.PowerUpColor;
  } 
}
