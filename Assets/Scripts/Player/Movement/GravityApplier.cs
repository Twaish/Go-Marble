using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class GravityApplier : MonoBehaviour {
  [Tooltip("The scale of gravity applied")]
  [SerializeField] private float gravityScale = 2f;

  private Rigidbody rb;
  private Vector3 gravityDirection;
  private readonly List<GravityDirection> gravityRequests = new();

  private void Awake() {
    rb = GetComponent<Rigidbody>();
  }

  private void FixedUpdate() {
    UpdateGravityDirection();
    ApplyGravity();
  }

  private void UpdateGravityDirection() {
    gravityDirection = Physics.gravity;

    // https://github.com/ThermiteFe8/Custom-Gravity-Physics-Unity/blob/main/Assets/Scripts/GravityUtils/CustomGravityAffected.cs
    if (gravityRequests.Count > 0) {
      int highestPriority = int.MinValue;
      foreach (var request in gravityRequests) {
        if (request.priority > highestPriority) {
          gravityDirection = request.gravity;
          highestPriority = request.priority;
        }
      }
    }

    gravityRequests.Clear();
  }

  private void ApplyGravity() {
    rb.linearVelocity += gravityScale * rb.mass * Time.fixedDeltaTime * gravityDirection;
  }

  public void AddGravityDirection(GravityDirection direction) {
    gravityRequests.Add(direction);
  }
}
