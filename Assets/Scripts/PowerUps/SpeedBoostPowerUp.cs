using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "SpeedBoost", menuName = "Powerups/SpeedBoost")]
public class SpeedBoostPowerUp : BasePowerUp {
  public float BurstStrength = 20f;

  public override void Effect(PlayerController player) {
    player.StartCoroutine(SpeedBoost(player));
  }

  private IEnumerator SpeedBoost(PlayerController player) {
    Vector3 direction = player.GetComponent<Rigidbody>().linearVelocity.normalized; 

    direction.y = 0;

    player.GetComponent<Rigidbody>().AddForce(direction * BurstStrength, ForceMode.VelocityChange);

    OnActivate(player);
    yield return null;
    OnExpire(player);
  }

  public override void OnActivate(PlayerController player) {
    Debug.Log($"{Name} Activated!");
  }

  public override void OnExpire(PlayerController player) {
    Debug.Log($"{Name} Expired!");
  }
}
