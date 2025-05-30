using UnityEngine;

public class PlayerJump : MonoBehaviour {
  [Tooltip("The force applied to the player to make them jump")]
  [SerializeField] private float jumpForce = 15f;
  [Tooltip("Time between jumps")]
  [SerializeField] private float jumpCooldown = 0.5f;

  private float lastJumpTime = -Mathf.Infinity;
  private Rigidbody rb;
  private GroundChecker groundChecker;
  private PlayerControls playerControls;

  void Awake() {
    rb = GetComponent<Rigidbody>();
    groundChecker = GetComponent<GroundChecker>();
    playerControls = GetComponent<PlayerControls>();

    playerControls.OnJump += HandleJump;
  }

  private void HandleJump() {
    if (groundChecker.IsGrounded && Time.time - lastJumpTime > jumpCooldown) {
      // Jump upwards from the nearest ground normal
      rb.linearVelocity = Vector3.ProjectOnPlane(rb.linearVelocity, groundChecker.GroundNormal);
      rb.AddForce(groundChecker.GroundNormal * jumpForce, ForceMode.Impulse);

      lastJumpTime = Time.time;
    }
  }
}
