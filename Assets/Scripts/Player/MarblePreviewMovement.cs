using UnityEngine;

public class MarblePreviewMovement : MonoBehaviour {
  [SerializeField]
  private float speed = 50f;
  [SerializeField]
  private float maxSpeed = 10f;
  [SerializeField]
  private float maxDistance = 400f;
  
  private Rigidbody rb;
  private Vector3 startPosition;
  private float rightBoundary;
  private float leftBoundary;
  private bool movingRight = true;

  void Awake() {
    rb = GetComponent<Rigidbody>();
    startPosition = transform.position;
    rightBoundary = startPosition.x + maxDistance;
    leftBoundary = startPosition.x - maxDistance;
  }

  void FixedUpdate() {
    int direction = movingRight ? 1 : -1;
    Vector3 movementForce = direction * 4 * speed * Time.fixedDeltaTime * Vector3.right;
    rb.AddForce(movementForce, ForceMode.Force);

    if (rb.linearVelocity.magnitude > maxSpeed) {
      rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
    }

    if (
      (movingRight && transform.position.x >= rightBoundary) ||
      (!movingRight && transform.position.x <= leftBoundary)
    ) {
      movingRight = !movingRight;
    }
  }
}
