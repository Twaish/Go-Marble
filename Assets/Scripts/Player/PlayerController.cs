using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour {

  [Header("View")]
  [SerializeField, Tooltip("The main camera")]
  private GameObject mainCamera;
  [SerializeField, Tooltip("")]
  private float baseFOV = 60f;
  [SerializeField, Tooltip("")]
  private float maxFOV = 80f;

  [Header("Environment Control")]
  [SerializeField, Tooltip("The scale of gravity applied to the player")]
  private float gravityScale = 1f;
  [SerializeField, Tooltip("The force applied to the player's movement while in midair")]
  private float airControlForce = 15f;
  [SerializeField, Tooltip("How quickly the player can rotate while in midair")]
  private float airRotationSpeed = 15f; 
  [SerializeField] 
  private float groundDetectionRange = .6f;
  [SerializeField] 
  private LayerMask groundMask;

  [Header("Movement")]
  [SerializeField, Tooltip("The speed at which the player moves")]
  private float speed = 45f;
  [SerializeField, Tooltip("The force applied to the player to make them jump")]
  private float jumpForce = 15f;
  [SerializeField, Tooltip("Time between jumps")]
  private float jumpCooldown = 0.5f;
  [SerializeField, Tooltip("How fast a player can change their direction")]
  private float turnSpeed = 5f;
  [SerializeField, Tooltip("Maximum speed the player can achieve")]
  private float maxSpeed = 200f;
  [SerializeField, Tooltip("The rate at which the player slows down when no input is applied")]
  private float decelerationRate = 1f;

  [SerializeField, Tooltip("How bouncy the player is when colliding with surfaces")]
  private float bounciness = .8f;

  [SerializeField]
  private float shakeIntensity = .3f;
  [SerializeField]
  private float shakeDuration = 1f;
  [SerializeField, Tooltip("Minimum collision magnitude before shaking camera")]
  private float shakeImpactThreshold = 20f;
  
  private Rigidbody rb;
  private float movementX;
  private float movementY;
  private CameraShaker cameraShaker;
  private Camera cameraComponent;
  private bool isGrounded;
  private float lastJumpTime = -Mathf.Infinity;
  
  void Start() {
    cameraComponent = mainCamera.GetComponent<Camera>();
    cameraShaker = mainCamera.GetComponent<CameraShaker>();
    rb = GetComponent<Rigidbody>();

    rb.linearDamping = 0;
    rb.angularDamping = 0;
  }

  void FixedUpdate() {
    Vector3 movement = GetCameraRelativeMovement();
    UpdateFOV(movement);

    CheckGroundStatus();

    if (isGrounded) {
      HandleGroundMovement(movement);
    } else {
      HandleAirMovement(movement);
    }

    UpdateGravity();
    EnforceSpeedLimit();
    HandleJump();
  }

  void HandleGroundMovement(Vector3 movement) {
    if (movement.magnitude > 0) {
      Vector3 movementForce = movement * speed;
      rb.AddForce(movementForce, ForceMode.Force);

      Quaternion targetRotation = Quaternion.LookRotation(movement);
      transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
    }

    // Decrease velocity when no input is applied
    if (movement.magnitude <= 0) {
      Vector3 horizontalVelocity = new(rb.linearVelocity.x, 0, rb.linearVelocity.z);
      horizontalVelocity = Vector3.Lerp(horizontalVelocity, Vector3.zero, decelerationRate * Time.fixedDeltaTime);
      
      rb.linearVelocity = new Vector3(
        horizontalVelocity.x,
        rb.linearVelocity.y,
        horizontalVelocity.z
      );
    }
    
    // Decrease angular velocity when not moving
    if (movement.magnitude <= 0) {
      rb.angularVelocity = Vector3.Lerp(rb.angularVelocity, Vector3.zero, decelerationRate * Time.fixedDeltaTime);
    }
  }

  void HandleAirMovement(Vector3 movement) {
    // Add rotation and reduced velocity in midair
    if (movement.magnitude > 0) {
      rb.AddForce(movement * airControlForce, ForceMode.Force);

      Vector3 rotationAxis = Vector3.Cross(Vector3.up, movement);
      rb.AddTorque(rotationAxis * airRotationSpeed, ForceMode.Force);
    }
  }
  
  void EnforceSpeedLimit() {
    Vector3 horizontalVelocity = new(rb.linearVelocity.x, 0, rb.linearVelocity.z);
    float currentSpeed = horizontalVelocity.magnitude;

    // Add excess force against player
    if (currentSpeed > maxSpeed) {
      Vector3 excessVelocity = horizontalVelocity.normalized * (currentSpeed - maxSpeed);
      
      rb.AddForce(-excessVelocity * rb.mass, ForceMode.Force);
    }
  }

  void CheckGroundStatus() {
    Debug.DrawRay(transform.position, Vector3.down * groundDetectionRange, Color.green);
    isGrounded = Physics.Raycast(transform.position, Vector3.down, groundDetectionRange, groundMask);
  }

  void UpdateFOV(Vector3 movement) {
    float targetFOV = Mathf.Lerp(baseFOV, maxFOV, movement.magnitude);
    cameraComponent.fieldOfView = Mathf.Lerp(cameraComponent.fieldOfView, targetFOV, Time.deltaTime);
  }

  void UpdateGravity() {
    rb.linearVelocity += gravityScale * rb.mass * Time.fixedDeltaTime * Physics.gravity;
  }
  
  // Rotate movement vector inline with camera rotation
  Vector3 GetCameraRelativeMovement() {
    Vector3 inputDirection = new Vector3(movementX, 0f, movementY).normalized;
    
    if (inputDirection.magnitude <= 0) {
      return Vector3.zero;
    }
    
    float cameraYRotation = cameraComponent.transform.rotation.eulerAngles.y;
    Quaternion cameraRotation = Quaternion.Euler(0f, cameraYRotation, 0f);
    
    Vector3 cameraRelativeMovement = cameraRotation * inputDirection;
    
    return cameraRelativeMovement.normalized;
  }

  void HandleJump() {
    if (
      isGrounded 
      && Keyboard.current.spaceKey.isPressed 
      && Time.time - lastJumpTime > jumpCooldown
    ) {
      rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
      
      rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

      lastJumpTime = Time.time;
      isGrounded = false;
    }
  }

  void OnMove(InputValue movementValue) {
    Vector2 movementVector = movementValue.Get<Vector2>();

    movementX = movementVector.x;
    movementY = movementVector.y;
  }

  void OnCollisionEnter(Collision collision) {
    float impactForce = collision.relativeVelocity.magnitude;
    
    if (impactForce > shakeImpactThreshold) {
      float intensity = Mathf.Clamp(impactForce / 40f, 0.05f, 1f) * shakeIntensity;
      float duration = Mathf.Clamp(impactForce / 50f, 0.1f, 0.5f) * shakeDuration;
      
      StartCoroutine(cameraShaker.Shake(duration, intensity));
    }
  }
}
