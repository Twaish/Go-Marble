using UnityEngine;

public class MarblePreviewMovement : MonoBehaviour {
  [SerializeField]
  private float speed = 15f;
  [SerializeField]
  private float maxSpeed = 10f;
  
  private Rigidbody rb;

  void Awake() {
    rb = GetComponent<Rigidbody>();
  }

  void FixedUpdate() {
    rb.AddForce(4 * speed * Time.fixedDeltaTime * Vector3.right, ForceMode.Force);
    // if (rb.linearVelocity.magnitude > maxSpeed) {
    //   Vector3 excessVelocity = rb.linearVelocity.normalized * (rb.linearVelocity.magnitude - maxSpeed);
    //   rb.AddForce(-excessVelocity * 2, ForceMode.Force);
    // }
  }
}
