using UnityEngine;

public class PowerUpPickup : MonoBehaviour {
  [SerializeField] private BasePowerUp powerUpToGive;
  [SerializeField] private bool canOverwritePowerUp = true;
  [SerializeField] private ParticleSystem pickupEffect;

  private PowerUpAppearance powerUpAppearance;

  private void Awake() {
    powerUpAppearance = GetComponent<PowerUpAppearance>();
  }

  private void Start() {
    powerUpAppearance.Setup(powerUpToGive);
  }

  private void OnTriggerEnter(Collider other) {
    if (!other.CompareTag("Player")) return;
    if (!other.TryGetComponent<PowerUpManager>(out var powerUpManager)) return;
    if (!canOverwritePowerUp && powerUpManager.HasPowerUp()) return;
    powerUpManager.AssignPowerUp(powerUpToGive);
    
    ParticleSystem effect = Instantiate(pickupEffect, transform.position, Quaternion.identity);
    effect.Play();
    Destroy(effect.gameObject, effect.main.duration);
    Destroy(gameObject);
  }
}
