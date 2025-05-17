using UnityEngine;

public class PowerUpPickup : MonoBehaviour {
  [SerializeField] private BasePowerUp powerUpToGive;
  [SerializeField] private bool canOverwritePowerUp = true;
  [SerializeField] private ParticleSystem pickupEffect;

  private PowerUpVisuals powerUpVisuals;

  private void Awake() {
    powerUpVisuals = GetComponent<PowerUpVisuals>();
  }

  private void Start() {
    powerUpVisuals.Setup(powerUpToGive);
  }

  private void OnTriggerEnter(Collider other) {
    if (!other.CompareTag("Player")) return;

    PlayerPowerUpUser powerUpUser = other.GetComponent<PlayerPowerUpUser>();
    if (powerUpUser == null) return;

    powerUpUser.GivePowerUp(powerUpToGive, canOverwritePowerUp);

    if (pickupEffect == null) return;

    ParticleSystem effect = Instantiate(pickupEffect, transform.position, Quaternion.identity);
    effect.Play();
    Destroy(effect.gameObject, effect.main.duration);
    Destroy(gameObject);
  }
}
