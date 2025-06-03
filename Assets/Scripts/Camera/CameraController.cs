using UnityEngine;

public class CameraController : MonoBehaviour {

  [Header("Player")]
  [SerializeField, Tooltip("Player game object")]
  private GameObject player;
  [SerializeField, Tooltip("Player game object")]
  private float followPlayerSpeed = 5f;
  [SerializeField, Tooltip("Player game object")]
  private Vector3 playerOffset = new(1f, 3.5f, -15f);

  [Header("Camera")]
  [SerializeField, Tooltip("Maximum tilt angle (degrees)")] 
  private float maxTiltAngle = 5f;
  [SerializeField, Tooltip("How quickly camera tilts")] 
  private float tiltSpeed = 3f;
  [SerializeField, Tooltip("How much velocity affects tilt")] 
  private float velocityTiltFactor = 0.1f;
  [SerializeField]
  private float rotationLerpSpeed = 10f;

  private float rotateCoefficient = 0f;
  
  private Rigidbody playerRigidbody;
  private CameraShaker cameraShaker;
  private float targetYRotation;
  private PlayerControls playerControls;

  void Awake() {
    cameraShaker = GetComponent<CameraShaker>();
    playerRigidbody = player.GetComponent<Rigidbody>();
    playerControls = player.GetComponent<PlayerControls>();
    playerControls.OnLook += UpdateRotateCoefficient;
  }

  private void OnDestroy() {
    playerControls.OnLook -= UpdateRotateCoefficient;
  }

  private void Start() {
    targetYRotation = transform.eulerAngles.y;
  }

  private void UpdateRotateCoefficient(float value) {
    rotateCoefficient = value;
  }

  private void LateUpdate() {
    HandleCameraRotationInput();
    ApplyCameraTilt();
    UpdateCameraPosition();
  }

  private void HandleCameraRotationInput() {
    float rotationSpeed = 200f;
    targetYRotation += rotateCoefficient * rotationSpeed * Time.deltaTime;

    float currentYRotation = Mathf.LerpAngle(transform.eulerAngles.y, targetYRotation, rotationLerpSpeed * Time.deltaTime);
    transform.rotation = Quaternion.Euler(transform.eulerAngles.x, currentYRotation, transform.eulerAngles.z);
  }

  private void UpdateCameraPosition() {
    Vector3 rotatedOffset = transform.rotation * playerOffset;
    Vector3 targetPosition = player.transform.position + rotatedOffset;

    transform.position = Vector3.Lerp(
      transform.position,
      targetPosition,
      followPlayerSpeed * Time.deltaTime
    ) + cameraShaker.GetShakeOffset();
  }

  private void ApplyCameraTilt() {
    Vector3 playerVelocity = new(playerRigidbody.linearVelocity.x, 0, playerRigidbody.linearVelocity.z);
    float speed = playerVelocity.magnitude;

    Quaternion targetRotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);

    if (speed > 1f) { 
      float rightDot = Vector3.Dot(playerVelocity.normalized, transform.right);
      float rollAngle = -rightDot * maxTiltAngle * Mathf.Min(1.0f, speed * velocityTiltFactor);

      float forwardDot = Vector3.Dot(playerVelocity.normalized, transform.forward);
      float pitchAngle = forwardDot * maxTiltAngle * 0.5f * Mathf.Min(1.0f, speed * velocityTiltFactor);

      targetRotation = Quaternion.Euler(pitchAngle, transform.eulerAngles.y, rollAngle);
    }

    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, tiltSpeed * Time.deltaTime);
  }
}
