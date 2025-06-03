using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Powerups/JumpBoost")]
public class JumpBoostPowerUp : BasePowerUp {
  public float BurstStrength = 20f;

  public override void Effect(PlayerController player) {
    player.StartCoroutine(JumpBoost(player));
  }

  private IEnumerator JumpBoost(PlayerController player) {
    player.GetComponent<Rigidbody>().AddForce(Vector3.up * BurstStrength, ForceMode.VelocityChange);

    OnActivate(player);
    yield return null;
    OnExpire(player);
  }

  public override void OnActivate(PlayerController player) {
  }

  public override void OnExpire(PlayerController player) {
  }
}
