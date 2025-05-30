using UnityEngine;

public class PlayerCameraController : MonoBehaviour {
  [SerializeField] private Camera cameraComponent;
  [SerializeField] private CameraShaker cameraShaker;

  [Header("FOV")]
  [SerializeField] private float baseFOV = 60f;
  [SerializeField] private float maxFOV = 80f;

  [Header("Shake")]
  [SerializeField] private float shakeIntensity = 0.3f;
  [SerializeField] private float shakeDuration = 1f;
  [Tooltip("Minimum collision magnitude before shaking camera")]
  [SerializeField] private float shakeImpactThreshold = 35f;

  public void HandleMovementFOV(Vector3 movement) {
    float targetFOV = Mathf.Lerp(baseFOV, maxFOV, movement.magnitude);
    cameraComponent.fieldOfView = Mathf.Lerp(cameraComponent.fieldOfView, targetFOV, Time.fixedDeltaTime);
  }

  public void HandleCollisionShake(Collision collision) {
    float impactForce = collision.relativeVelocity.magnitude;
    if (impactForce > shakeImpactThreshold) {
      float intensity = Mathf.Clamp(impactForce / 40f, 0.05f, 1f) * shakeIntensity;
      float duration = Mathf.Clamp(impactForce / 50f, 0.1f, 0.5f) * shakeDuration;
      StartCoroutine(cameraShaker.Shake(duration, intensity));
    }
  }

  public Quaternion GetCameraRotation() {
    return cameraComponent.transform.rotation;
  }
}
