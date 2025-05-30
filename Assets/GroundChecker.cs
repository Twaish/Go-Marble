using UnityEngine;

public class GroundChecker : MonoBehaviour {
  [SerializeField] private float detectionRange = 0.6f;
  [SerializeField] private LayerMask groundMask;

  [HideInInspector] public bool IsGrounded;
  [HideInInspector] public Vector3 GroundNormal;

  public void CheckGroundStatus() {
    IsGrounded = false;
    GroundNormal = Vector3.zero;

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
      if (Physics.Raycast(transform.position, dir, out RaycastHit hit, detectionRange, groundMask)) {
        IsGrounded = true;
        GroundNormal += hit.normal;
        hitCount++;
      }
    }

    if (IsGrounded && hitCount > 0) {
      GroundNormal.Normalize();
    }
  }
}
