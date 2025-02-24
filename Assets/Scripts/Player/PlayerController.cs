using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour {
  private Rigidbody rb;
  private float movementX;
  private float movementY;

  public Camera mainCamera;
  public Transform cameraTransform;

  [Header("View")]
  [Tooltip("")]
  public float baseFOV = 60f;
  [Tooltip("")]
  public float maxFOV = 80f;

  [Header("Movement")]
  [Tooltip("The speed at which the player moves")]
  public float speed = 10f;
  [Tooltip("The scale of gravity applied to the player")]
  public float gravityScale = 2f;
  [Tooltip("The force applied to the player to make them jump")]
  public float jumpForce = 5f;
  [Tooltip("Time between jumps")]
  public float jumpCooldown = 0.5f;
  [Tooltip("How fast a player can change their direction")]
  public float turnSpeed = 5f;
  [Tooltip("Maximum speed the player can achieve")]
  public float maxSpeed = 20f;
  [Tooltip("The rate at which the player slows down when no input is applied")]
  public float decelerationRate = 2f;

  [Tooltip("How bouncy the player is when colliding with surfaces")]
  public float bounciness = .8f;

  [Tooltip("The force applied to the player's movement while in midair")]
  public float airControlForce = 5f;
  [Tooltip("How quickly the player can rotate while in midair")]
  public float airRotationSpeed = 5f; 

  [SerializeField]
  private float groundDetectionRange = .6f;

  [Header("UI")]

  public LayerMask groundMask;

  private bool isGrounded;

  private float lastJumpTime = -Mathf.Infinity;

  void Start() {
    rb = GetComponent<Rigidbody>();

    rb.linearDamping = 0;
    rb.angularDamping = 0;
  }

  void Update() {
    // Vector3 cameraForward = mainCamera.transform.forward;
    // Vector3 cameraRight = mainCamera.transform.right;

    // cameraForward.y = 0;
    // cameraRight.y = 0;
    // cameraForward.Normalize();
    // cameraRight.Normalize();

    Debug.DrawRay(transform.position, Vector3.down * groundDetectionRange, Color.green);
    isGrounded = Physics.Raycast(transform.position, Vector3.down, groundDetectionRange, groundMask);

    // Vector3 movement = (cameraForward * movementY + cameraRight * movementX).normalized;
    Vector3 movement = new Vector3(movementX, 0f, movementY).normalized;

    float targetFOV = Mathf.Lerp(baseFOV, maxFOV, movement.magnitude);
    mainCamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView, targetFOV, Time.deltaTime);

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

  private void UpdateGravity() {
    Vector3 gravityForce = Physics.gravity * rb.mass * gravityScale * Time.fixedDeltaTime;
    rb.linearVelocity += gravityForce;
  }

  // void OnCollisionEnter(Collision collision) {
  //   Vector3 normal = collision.contacts[0].normal;
  //   Vector3 currentVelocity = rb.linearVelocity;

  //   Vector3 reflectedVelocity = Vector3.Reflect(currentVelocity, normal);

  //   rb.linearVelocity = reflectedVelocity * bounciness;

  //   if (normal.y > .5f) {
  //     isGrounded = true;
  //   }
  // }

  // void OnCollisionStay(Collision collision) {
  //   isGrounded = true;
  // }

  // void OnCollisionExit(){
  //   isGrounded = false;
  // }

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
