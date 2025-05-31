using UnityEngine;

public class PowerUpPickup : MonoBehaviour {
  [SerializeField] private BasePowerUp powerUpToGive;
  [SerializeField] private bool canOverwritePowerUp = true;
  [SerializeField] private GameObject pickupEffectPrefab;
  
  private PowerUpVisuals powerUpVisuals;

  private void Awake() {
    powerUpVisuals = GetComponent<PowerUpVisuals>();
  }

  private void Start() {
    powerUpVisuals.Setup(powerUpToGive);
  }

  private void OnTriggerEnter(Collider other) {
    if (!other.CompareTag("Player")) return;

    if (!other.TryGetComponent(out PlayerPowerUpUser powerUpUser)) return;

    if (pickupEffectPrefab != null) {
      Instantiate(pickupEffectPrefab, transform.position, Quaternion.identity);
    } 

    powerUpUser.GivePowerUp(powerUpToGive, canOverwritePowerUp);
    Destroy(gameObject);
  }
}
