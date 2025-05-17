using UnityEngine;

public class AccesoryPivot : MonoBehaviour
{
  [SerializeField] private Transform target;
  [SerializeField] private Vector3 offset = new(0, 0.5f, 0);
  [SerializeField] private float rotationSmoothSpeed = 10f;
  [SerializeField] private float yawOffsetDegrees = -90f; // Y-axis rotation offset

  private Vector3 previousPosition;

  private void Start()
  {
    if (target == null)
    {
      Debug.LogError("AccessoryPivot: Target is not assigned");
      enabled = false;
      return;
    }
    previousPosition = target.position;
  }

  private void Update()
  {
    Vector3 currentPosition = target.position;
    Vector3 velocity = (currentPosition - previousPosition) / Time.deltaTime;

    Vector3 desiredPosition = currentPosition + offset;

    transform.position = desiredPosition;

    if (velocity.sqrMagnitude > 0.001f) {
      velocity.y = 0;

      Quaternion targetRotation = Quaternion.LookRotation(velocity.normalized);
      targetRotation *= Quaternion.Euler(0, yawOffsetDegrees, 0);

      transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSmoothSpeed * Time.deltaTime);
    }

    previousPosition = currentPosition;
  }
}
