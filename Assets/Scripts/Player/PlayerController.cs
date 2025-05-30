using UnityEngine;

public class PlayerController : MonoBehaviour {
  [Header("Environment Control")]
  [SerializeField, Tooltip("The force applied to the player's movement while in midair")]
  private float airControlForce = 15f;
  [SerializeField, Tooltip("How quickly the player can rotate while in midair")]
  private float airRotationSpeed = 15f;

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

  private Rigidbody rb;
  private SphereCollider sphereCollider;
  private float lastJumpTime = -Mathf.Infinity;

  private PlayerControls playerControls;
  private SurfaceConditionHandler surfaceConditionHandler;
  private PlayerCameraController playerCameraController;
  private GroundChecker groundChecker;
  private GravityHandler gravityHandler;

  void Awake() {
    playerControls = GetComponent<PlayerControls>();
    playerControls.OnJump += HandleJump;

    surfaceConditionHandler = GetComponent<SurfaceConditionHandler>();
    playerCameraController = GetComponent<PlayerCameraController>();
    groundChecker = GetComponent<GroundChecker>();
    gravityHandler = GetComponent<GravityHandler>();

    sphereCollider = GetComponent<SphereCollider>();
    rb = GetComponent<Rigidbody>();
  }

  void FixedUpdate() {
    Vector3 movement = GetCameraRelativeMovement();
    playerCameraController.HandleMovementFOV(movement);

    groundChecker.CheckGroundStatus();

    if (groundChecker.IsGrounded) {
      if (movement.magnitude > 0) {
        HandleGroundMovement(movement);
      }
      else {
        HandleIdleMovement();
      }
    }
    else if (movement.magnitude > 0) {
      HandleAirMovement(movement);
    }

    gravityHandler.UpdateGravity();
    EnforceSpeedLimit();
  }

  void HandleGroundMovement(Vector3 movement) {
    Vector3 movementForce = movement * speed;
    rb.AddForce(movementForce, ForceMode.Force);

    Quaternion targetRotation = Quaternion.LookRotation(movement);
    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.fixedDeltaTime);
  }

  void HandleIdleMovement() {
    // Decrease velocity when no input is applied
    Vector3 horizontalVelocity = new(rb.linearVelocity.x, 0, rb.linearVelocity.z);
    horizontalVelocity = Vector3.Lerp(horizontalVelocity, Vector3.zero, decelerationRate * Time.fixedDeltaTime);

    rb.linearVelocity = new Vector3(
      horizontalVelocity.x,
      rb.linearVelocity.y,
      horizontalVelocity.z
    );

    // Decrease angular velocity when not moving
    rb.angularVelocity = Vector3.Lerp(rb.angularVelocity, Vector3.zero, decelerationRate * Time.fixedDeltaTime);
  }

  void HandleAirMovement(Vector3 movement) {
    // Add rotation and reduced velocity in midair
    rb.AddForce(movement * airControlForce, ForceMode.Force);

    Vector3 rotationAxis = Vector3.Cross(Vector3.up, movement);
    rb.AddTorque(rotationAxis * airRotationSpeed, ForceMode.Force);
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

  // Rotate movement vector inline with camera rotation
  Vector3 GetCameraRelativeMovement() {
    Vector3 inputDirection = playerControls.GetMovementVector();

    if (inputDirection.magnitude <= 0) {
      return Vector3.zero;
    }

    float cameraYRotation = playerCameraController.GetCameraRotation().eulerAngles.y;
    Quaternion cameraRotation = Quaternion.Euler(0f, cameraYRotation, 0f);

    Vector3 cameraRelativeMovement = cameraRotation * inputDirection;

    return cameraRelativeMovement.normalized;
  }

  void HandleJump() {
    if (
      groundChecker.IsGrounded &&
      Time.time - lastJumpTime > jumpCooldown
    ) {
      rb.linearVelocity = Vector3.ProjectOnPlane(rb.linearVelocity, groundChecker.GroundNormal); // remove velocity into surface
      rb.AddForce(groundChecker.GroundNormal * jumpForce, ForceMode.Impulse);

      lastJumpTime = Time.time;
    }
  }

  private void OnCollisionEnter(Collision collision) {
    ApplyMaterialConditions(collision.gameObject);
    playerCameraController.HandleCollisionShake(collision);
  }


  void ApplyMaterialConditions(GameObject gameObject) {
    SurfaceCondition condition = surfaceConditionHandler.GetCondition(gameObject);
    
    speed = condition.speed;
    turnSpeed = condition.turnSpeed;
    decelerationRate = condition.decelerationRate;
    sphereCollider.sharedMaterial.bounciness = condition.bounciness;
  }
}
