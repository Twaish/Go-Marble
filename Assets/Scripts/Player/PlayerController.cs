using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour {
  private Rigidbody rb;
  private float movementX;
  private float movementY;

  [Header("Movement")]
  public float speed = 10f;
  public float gravityScale = 2f;
  public float jumpForce = 5f;
  public float jumpCooldown = 0.5f;
  public float turnSpeed = 5f; // 
  public float maxSpeed = 20f; // Max speed player can apply
  public float decelerationRate = 2f;
  // public float groundDetectionHeight = .5f;

  public float bounciness = .8f;

  public float airControlForce = 1f;
  public float airRotationSpeed = 5f;

  [Header("UI")]
  public TextMeshProUGUI scoreText;

  private int score = 0;
  private bool isGrounded;

  private float lastJumpTime = -Mathf.Infinity;

  void Start() {
    rb = GetComponent<Rigidbody>();
    UpdateScoreText();

    rb.linearDamping = 0;
    rb.angularDamping = 0;
  }

  void FixedUpdate() {
    // isGrounded = Physics.Raycast(transform.position, Vector3.down, groundDetectionHeight);
    // Debug.DrawRay(transform.position, Vector3.down * groundDetectionHeight, Color.red); 
    // Debug.Log(isGrounded);

    Vector3 movement = new Vector3(movementX, 0.0f, movementY).normalized;

    if (isGrounded) {
      if (movement.magnitude > 0) {
        Vector3 desiredVelocity = movement * speed;

        rb.linearVelocity = new Vector3(
          Mathf.Lerp(rb.linearVelocity.x, desiredVelocity.x, turnSpeed * Time.fixedDeltaTime),
          rb.linearVelocity.y,
          Mathf.Lerp(rb.linearVelocity.z, desiredVelocity.z, turnSpeed * Time.fixedDeltaTime)
        );
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
      if (movement.magnitude > 0) {
        rb.AddForce(movement * airControlForce, ForceMode.Force);

        Vector3 rotationAxis = Vector3.Cross(Vector3.up, movement);
        rb.AddTorque(rotationAxis * airRotationSpeed, ForceMode.Force);
      }
    }

    // rb.AddForce(movement * speed);
    // rb.linearVelocity = Vector3.Lerp(
    //   rb.linearVelocity, 
    //   new Vector3(
    //     desiredVelocity.x, 
    //     rb.linearVelocity.y, 
    //     desiredVelocity.z
    //   ), 
    //   turnSpeed * Time.fixedDeltaTime
    // );

    // Apply gravity
    Vector3 gravityForce = Physics.gravity * rb.mass * gravityScale * Time.fixedDeltaTime;
    rb.linearVelocity += gravityForce;

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
    if (isGrounded && Keyboard.current.spaceKey.isPressed && Time.time - lastJumpTime > jumpCooldown) {
      Jump();
      lastJumpTime = Time.time;
      isGrounded = false;
    }
  }

  void OnCollisionEnter(Collision collision) {
    Vector3 normal = collision.contacts[0].normal;
    Vector3 currentVelocity = rb.linearVelocity;

    Vector3 reflectedVelocity = Vector3.Reflect(currentVelocity, normal);

    rb.linearVelocity = reflectedVelocity * bounciness;

    if (normal.y > .5f) {
      isGrounded = true;
    }
  }

  void OnCollisionStay(Collision collision) {
    isGrounded = true;
  }

  void OnCollisionExit(){
    isGrounded = false;
  }

  void Jump() {
    rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
  }

  void OnTriggerEnter(Collider other) {
    if (other.gameObject.CompareTag("PickUp")) {
      Destroy(other.gameObject);
      score++;
      UpdateScoreText();
    }
  }

  void OnMove(InputValue movementValue) {
    Vector2 movementVector = movementValue.Get<Vector2>();

    movementX = movementVector.x;
    movementY = movementVector.y;
  }

  void UpdateScoreText() {
    scoreText.text = "Score: " + score.ToString();
  }
}
