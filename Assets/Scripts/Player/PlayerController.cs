using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour {
  private Rigidbody rb;
  private float movementX;
  private float movementY;

  [Header("View")]
  [SerializeField, Tooltip("")]
  private Camera mainCamera;
  [SerializeField, Tooltip("")]
  private Transform cameraTransform;
  [SerializeField, Tooltip("")]
  private float baseFOV = 60f;
  [SerializeField, Tooltip("")]
  private float maxFOV = 80f;

  [Header("Movement")]
  [SerializeField, Tooltip("The speed at which the player moves")]
  private float speed = 10f;
  [SerializeField, Tooltip("The scale of gravity applied to the player")]
  private float gravityScale = 2f;
  [SerializeField, Tooltip("The force applied to the player to make them jump")]
  private float jumpForce = 5f;
  [SerializeField, Tooltip("Time between jumps")]
  private float jumpCooldown = 0.5f;
  [SerializeField, Tooltip("How fast a player can change their direction")]
  private float turnSpeed = 5f;
  [SerializeField, Tooltip("Maximum speed the player can achieve")]
  private float maxSpeed = 20f;
  [SerializeField, Tooltip("The rate at which the player slows down when no input is applied")]
  private float decelerationRate = 2f;

  [SerializeField, Tooltip("How bouncy the player is when colliding with surfaces")]
  private float bounciness = .8f;

  [SerializeField, Tooltip("The force applied to the player's movement while in midair")]
  private float airControlForce = 5f;
  [SerializeField, Tooltip("How quickly the player can rotate while in midair")]
  private float airRotationSpeed = 5f; 

  [SerializeField] 
  private float groundDetectionRange = .6f;
  
  [SerializeField] 
  private LayerMask groundMask;
  
  private bool isGrounded;
  private float lastJumpTime = -Mathf.Infinity;

  void Start() {
    rb = GetComponent<Rigidbody>();

    rb.linearDamping = 0;
    rb.angularDamping = 0;
  }

  void Update() {
    Debug.DrawRay(transform.position, Vector3.down * groundDetectionRange, Color.green);
    isGrounded = Physics.Raycast(transform.position, Vector3.down, groundDetectionRange, groundMask);

    Vector3 movement = new Vector3(movementX, 0f, movementY).normalized;

    UpdateFOV(movement);

    if (isGrounded) {
      // Lerp velocity 
      if (movement.magnitude > 0) {
        Vector3 desiredVelocity = movement * speed;

        rb.linearVelocity = new Vector3(
          Mathf.Lerp(rb.linearVelocity.x, desiredVelocity.x, turnSpeed * Time.fixedDeltaTime),
          rb.linearVelocity.y,
          Mathf.Lerp(rb.linearVelocity.z, desiredVelocity.z, turnSpeed * Time.fixedDeltaTime)
        );

      // Gradually decrease velocity when player is applying movement 
      } else {
        Vector3 horizontalVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        horizontalVelocity = Vector3.Lerp(horizontalVelocity, Vector3.zero, decelerationRate * Time.fixedDeltaTime);
        
        rb.linearVelocity = new Vector3(
          horizontalVelocity.x,
          rb.linearVelocity.y,
          horizontalVelocity.z
        );
      }
    } else {
      // Add rotation and decrease velocity control in midair
      if (movement.magnitude > 0) {
        rb.AddForce(movement * airControlForce, ForceMode.Force);

        Vector3 rotationAxis = Vector3.Cross(Vector3.up, movement);
        rb.AddTorque(rotationAxis * airRotationSpeed, ForceMode.Force);
      }
    }

    float shakeIntensity = .1f;
    float speedShakeThreshold = 15f;
    if (movement.magnitude > speedShakeThreshold) {
      float shakeAmount = (speed - speedShakeThreshold) * shakeIntensity;
      Vector3 shakeOffset = new Vector3(
        Mathf.PerlinNoise(Time.time * 10f, 0) - 0.5f,
        Mathf.PerlinNoise(0, Time.time * 10f) - 0.5f,
        0
      ) * shakeAmount;

      cameraTransform.localPosition += shakeOffset;
    }

    UpdateGravity();

    // Speed limit
    float currentSpeed = new Vector2(rb.linearVelocity.x, rb.linearVelocity.z).magnitude;
    if (currentSpeed > maxSpeed) {
      Vector3 velocityDirection = rb.linearVelocity.normalized;
      rb.linearVelocity = new Vector3(
        velocityDirection.x * maxSpeed,
        rb.linearVelocity.y,
        velocityDirection.z * maxSpeed
      );
    }

    // Jump when space pressed
    if (
      isGrounded 
      && Keyboard.current.spaceKey.isPressed 
      && Time.time - lastJumpTime > jumpCooldown
    ) {
      Jump();
      lastJumpTime = Time.time;
      isGrounded = false;
    }
  }

  // Interpolates the main camera FOV based on the player movement speed
  private void UpdateFOV(Vector3 movement) {
    float targetFOV = Mathf.Lerp(baseFOV, maxFOV, movement.magnitude);
    mainCamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView, targetFOV, Time.deltaTime);
  }

  private void UpdateGravity() {
    Vector3 gravityForce = Physics.gravity * rb.mass * gravityScale * Time.fixedDeltaTime;
    rb.linearVelocity += gravityForce;
  }

  void Jump() {
    rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
    
    rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
  }

  void OnMove(InputValue movementValue) {
    Vector2 movementVector = movementValue.Get<Vector2>();

    movementX = movementVector.x;
    movementY = movementVector.y;
  }
}
