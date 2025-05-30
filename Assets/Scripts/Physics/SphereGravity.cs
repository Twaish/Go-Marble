using UnityEngine;

// https://github.com/ThermiteFe8/Custom-Gravity-Physics-Unity/blob/main/Assets/Scripts/GravityFields/SphereGrav.cs
public class SphereGravity : MonoBehaviour {
  [SerializeField] Vector3 centerPosition = Vector3.zero;
  [SerializeField] float intensity = 30;
  [SerializeField] int priority = 0;
  private void OnTriggerStay(Collider other) {
    if (other.gameObject.TryGetComponent<GravityHandler>(out var gravityReceiver)) {
      Vector3 pointAtCenter = -1 * intensity * (other.transform.position - (centerPosition + transform.position)).normalized;

      GravityDirection newGravityDirection = new(pointAtCenter, priority);
      gravityReceiver.AddGravityDirection(newGravityDirection);
    }
  }
}
