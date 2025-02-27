using UnityEngine;

public class CameraController : MonoBehaviour {

  [Header("Player")]
  [SerializeField, Tooltip("Player game object")]
  private GameObject player;
  [SerializeField, Tooltip("Player game object")]
  private float followPlayerSpeed = 5f;
  [SerializeField, Tooltip("Player game object")]
  private Vector3 playerOffset = new(1f, 3.5f, -15f);

  [Header("Camera Tilt")]
  [SerializeField, Tooltip("Maximum tilt angle (degrees)")] 
  private float maxTiltAngle = 5f;
  [SerializeField, Tooltip("How quickly camera tilts")] 
  private float tiltSpeed = 3f;
  [SerializeField, Tooltip("How much velocity affects tilt")] 
  private float velocityTiltFactor = 0.1f;
  
  private Rigidbody playerRigidbody;
  private CameraShaker cameraShaker;
  
  private float targetYRotation;
  [SerializeField]
  private float rotationLerpSpeed = 10f;
  void Start() {
    cameraShaker = GetComponent<CameraShaker>();
    playerRigidbody = player.GetComponent<Rigidbody>();

    targetYRotation = transform.eulerAngles.y;
  }

  void LateUpdate() {
    float rotationSpeed = 200f;

    if (Input.GetKey(KeyCode.K)) {
      targetYRotation -= rotationSpeed * Time.deltaTime; // Rotate Left
    }
    else if (Input.GetKey(KeyCode.L)) {
      targetYRotation += rotationSpeed * Time.deltaTime; // Rotate Right
    }
    
    float currentYRotation = Mathf.LerpAngle(transform.eulerAngles.y, targetYRotation, rotationLerpSpeed * Time.deltaTime);
    
    Quaternion smoothedRotation = Quaternion.Euler(
      transform.eulerAngles.x,
      currentYRotation,
      transform.eulerAngles.z
    );
    transform.rotation = smoothedRotation;

    Quaternion cameraYRotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);
    Vector3 rotatedOffset = cameraYRotation * playerOffset;
    Vector3 targetPosition = player.transform.position + rotatedOffset;
    Vector3 followPosition = Vector3.Lerp(transform.position, targetPosition, followPlayerSpeed * Time.deltaTime);

    ApplyCameraTilt();

    if (cameraShaker == null) {
      transform.position = followPosition;
    } else {
      transform.position = followPosition + cameraShaker.GetShakeOffset();
    }
  }

  void ApplyCameraTilt() {
    if (playerRigidbody == null) return;

    // Get player's velocity (ignoring vertical movement)
    Vector3 playerVelocity = new Vector3(playerRigidbody.linearVelocity.x, 0, playerRigidbody.linearVelocity.z);
    float speed = playerVelocity.magnitude;

    // Calculate target rotation
    Quaternion targetRotation;

    if (speed > 1f)
    { // Only tilt if moving faster than a minimum speed
      // Calculate side tilt (roll) based on lateral movement
      float rightDot = Vector3.Dot(playerVelocity.normalized, transform.right);
      float rollAngle = -rightDot * maxTiltAngle * Mathf.Min(1.0f, speed * velocityTiltFactor);

      // Calculate forward tilt (pitch) based on forward velocity
      float forwardDot = Vector3.Dot(playerVelocity.normalized, transform.forward);
      float pitchAngle = forwardDot * maxTiltAngle * 0.5f * Mathf.Min(1.0f, speed * velocityTiltFactor);

      // Create rotation with calculated angles
      targetRotation = Quaternion.Euler(pitchAngle, transform.eulerAngles.y, rollAngle);
    }
    else
    {
      // Return to neutral position when not moving
      targetRotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);
    }

    // Smoothly interpolate to target rotation
    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, tiltSpeed * Time.deltaTime);
  }
}
