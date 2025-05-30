using UnityEngine;

public class PlayerMovement : MonoBehaviour {
  [Header("Environment Control")]
  [Tooltip("The force applied to the player's movement while in midair")]
  [SerializeField] private float airControlForce = 15f;
  [Tooltip("How quickly the player can rotate while in midair")]
  [SerializeField] private float airRotationSpeed = 15f;

  [Header("Movement")]
  [Tooltip("The speed at which the player moves")]
  [SerializeField] private float speed = 45f;
  [Tooltip("How fast a player can change their direction")]
  [SerializeField] private float turnSpeed = 5f;
  [Tooltip("Maximum speed the player can achieve")]
  [SerializeField] private float maxSpeed = 200f;
  [Tooltip("The rate at which the player slows down when no input is applied")]
  [SerializeField] private float decelerationRate = 1f;

  private Rigidbody rb;
  private Transform playerTransform;
  private SphereCollider sphereCollider;

  private void Awake() {
    rb = GetComponent<Rigidbody>();
    sphereCollider = GetComponent<SphereCollider>();
    playerTransform = transform;
  }

  public void HandleGroundMovement(Vector3 movement) {
    Vector3 movementForce = movement * speed;
    rb.AddForce(movementForce, ForceMode.Force);

    Quaternion targetRotation = Quaternion.LookRotation(movement);
    playerTransform.rotation = Quaternion.Slerp(playerTransform.rotation, targetRotation, turnSpeed * Time.fixedDeltaTime);
  }

  public void HandleIdleMovement() {
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

  public void HandleAirMovement(Vector3 movement) {
    // Add rotation and reduced velocity in midair
    rb.AddForce(movement * airControlForce, ForceMode.Force);

    Vector3 rotationAxis = Vector3.Cross(Vector3.up, movement);
    rb.AddTorque(rotationAxis * airRotationSpeed, ForceMode.Force);
  }

  public void EnforceSpeedLimit() {
    Vector3 horizontalVelocity = new(rb.linearVelocity.x, 0, rb.linearVelocity.z);
    float currentSpeed = horizontalVelocity.magnitude;

    // Add excess force against player
    if (currentSpeed > maxSpeed) {
      Vector3 excessVelocity = horizontalVelocity.normalized * (currentSpeed - maxSpeed);
      rb.AddForce(-excessVelocity * rb.mass, ForceMode.Force);
    }
  }

  public void ApplySurfaceCondition(SurfaceCondition condition) {
    speed = condition.speed;
    turnSpeed = condition.turnSpeed;
    decelerationRate = condition.decelerationRate;
    sphereCollider.sharedMaterial.bounciness = condition.bounciness;
  }
}
