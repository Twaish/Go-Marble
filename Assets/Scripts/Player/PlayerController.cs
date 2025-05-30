using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[System.Serializable]
public class SurfaceCondition {
  public string surfaceTag;

  [Header("Movement Modifiers")]
  public float speed;
  public float turnSpeed;
  public float decelerationRate;

  [Header("Physics Modifiers")]
  public float torque;
  public float bounciness;
}

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
  private float gravityScale = 2f;
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
  private float shakeImpactThreshold = 35f;

  [Header("Surface Conditions")]
  [SerializeField]
  private SurfaceCondition defaultCondition;

  [SerializeField]
  private List<SurfaceCondition> surfaceConditions;

  [Header("Development")]
  [SerializeField]
  private bool debugRays;

  private Rigidbody rb;
  private SphereCollider sphereCollider;
  private CameraShaker cameraShaker;
  private Camera cameraComponent;
  private Vector3 gravityDirection;
  private List<GravityDirection> gravityDirections;
  private float movementX;
  private float movementY;
  private float lastJumpTime = -Mathf.Infinity;
  private bool isGrounded;
  private Vector3 lastGroundNormal = Vector3.up;

  void Awake() {
    cameraComponent = mainCamera.GetComponent<Camera>();
    cameraShaker = mainCamera.GetComponent<CameraShaker>();
    sphereCollider = GetComponent<SphereCollider>();
    rb = GetComponent<Rigidbody>();
  }

  void Start() {
    gravityDirections = new();
    rb.angularDamping = 0;
    rb.linearDamping = 0;
    sphereCollider.sharedMaterial.bounciness = bounciness;
  }

  void FixedUpdate() {
    Vector3 movement = GetCameraRelativeMovement();
    UpdateFOV(movement);

    // HandleSurfaceAlignment();

    CheckGroundStatus();

    if (isGrounded) {
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

    UpdateGravityDirection();
    UpdateGravity();
    EnforceSpeedLimit();
    HandleJump();
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

  void CheckGroundStatus() {
    isGrounded = false;
    lastGroundNormal = Vector3.zero;

    int hitCount = 0;
    Vector3[] directions = {
      transform.up,
      -transform.up,
      transform.right,
      -transform.right,
      transform.forward,
      -transform.forward
    };

    foreach (Vector3 dir in directions) {
      if (Physics.Raycast(transform.position, dir, out RaycastHit hit, groundDetectionRange, groundMask)) {
        isGrounded = true;
        lastGroundNormal += hit.normal;
        hitCount++;

        if (debugRays) {
          Debug.DrawRay(transform.position, dir * groundDetectionRange, Color.cyan);
          Debug.DrawRay(hit.point, hit.normal, Color.green);
        }
      }
    }

    if (isGrounded && hitCount > 0) {
      lastGroundNormal.Normalize();
    }
  }



  void UpdateFOV(Vector3 movement) {
    float targetFOV = Mathf.Lerp(baseFOV, maxFOV, movement.magnitude);
    cameraComponent.fieldOfView = Mathf.Lerp(cameraComponent.fieldOfView, targetFOV, Time.fixedDeltaTime);
  }

  void UpdateGravityDirection() {
    gravityDirection = Physics.gravity;

    // https://github.com/ThermiteFe8/Custom-Gravity-Physics-Unity/blob/main/Assets/Scripts/GravityUtils/CustomGravityAffected.cs
    if (gravityDirections.Count > 0) {
      int highestPriority = -100;
      Vector3 gravity = Vector3.zero;
      for (int i = 0; i < gravityDirections.Count; i++) {
        GravityDirection fallingRequest = gravityDirections[i];
        if (fallingRequest.priority > highestPriority) {
          gravity = fallingRequest.gravity;
          highestPriority = fallingRequest.priority;
        }
      }
      gravityDirection = gravity;
    }
    gravityDirections.Clear();
  }

  void UpdateGravity() {
    rb.linearVelocity += gravityScale * rb.mass * Time.fixedDeltaTime * gravityDirection;
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
    if (isGrounded && Keyboard.current.spaceKey.isPressed && Time.time - lastJumpTime > jumpCooldown) {
      rb.linearVelocity = Vector3.ProjectOnPlane(rb.linearVelocity, lastGroundNormal); // remove velocity into surface
      rb.AddForce(lastGroundNormal * jumpForce, ForceMode.Impulse);

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

    ApplyMaterialConditions(collision.gameObject);

    if (impactForce > shakeImpactThreshold) {
      float intensity = Mathf.Clamp(impactForce / 40f, 0.05f, 1f) * shakeIntensity;
      float duration = Mathf.Clamp(impactForce / 50f, 0.1f, 0.5f) * shakeDuration;

      StartCoroutine(cameraShaker.Shake(duration, intensity));
    }
  }

  void ApplyMaterialConditions(GameObject gameObject) {
    SurfaceCondition match = surfaceConditions.Find(cond => gameObject.CompareTag(cond.surfaceTag));

    SurfaceCondition condition = match ?? defaultCondition;

    speed = condition.speed;
    turnSpeed = condition.turnSpeed;
    decelerationRate = condition.decelerationRate;
    sphereCollider.sharedMaterial.bounciness = condition.bounciness;
  }

  // void HandleSurfaceAlignment() {
  //   if (Physics.Raycast(transform.position, gravityDirection, out RaycastHit hit, groundMask) && hit.collider.gameObject.layer == LayerMask.NameToLayer("AlignableSurface")) {
  //     gravityDirection = -hit.normal * 9.82f;      

  //     Quaternion rotation = Quaternion.FromToRotation(Vector3.up, hit.normal) * transform.rotation;
  //     transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 5f * Time.fixedDeltaTime);
  //   }
  // }

  public void AddGravityDirection(GravityDirection gravityDirection) {
    gravityDirections.Add(gravityDirection);
  }
}
