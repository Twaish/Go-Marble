using UnityEngine;

public enum LocalDirection {
  Up,
  Down,
  Left,
  Right,
  Forward,
  Backward
}

// https://github.com/ThermiteFe8/Custom-Gravity-Physics-Unity/blob/main/Assets/Scripts/GravityFields/ParallelGrav.cs
public class ParallelGravity : MonoBehaviour {
  [SerializeField]
  private LocalDirection gravityDirection = LocalDirection.Down;
  [SerializeField] 
  float intensity = 30;
  [SerializeField] 
  int priority = 0;

  void Update() {
    Debug.DrawRay(transform.position, GetSelectedDirection() * intensity, Color.green);
  }

  private void OnTriggerStay(Collider other) {
    if (other.gameObject.TryGetComponent<GravityHandler>(out var gravityReceiver)) {
      Vector3 gravityDirection = GetSelectedDirection() * intensity;
      GravityDirection newGravityDirection = new(gravityDirection, priority);
      gravityReceiver.AddGravityDirection(newGravityDirection);
    }
  }

  // Translate direction to local space transform axis vector
  private Vector3 GetSelectedDirection() {
    return gravityDirection switch {
      LocalDirection.Up => transform.up,
      LocalDirection.Down => -transform.up,
      LocalDirection.Left => -transform.right,
      LocalDirection.Right => transform.right,
      LocalDirection.Forward => transform.forward,
      LocalDirection.Backward => -transform.forward,
      _ => -transform.up
    };
  }
}
