using UnityEngine;

public class CameraController : MonoBehaviour {
  public GameObject player;
  // public float rotationSpeed = 5f;
  // public float distanceFromPlayer = 10f;
  // public float height = 5f;

  // private Rigidbody playerRb;

  private Vector3 offset;
  
  void Start() {
    // playerRb = player.GetComponent<Rigidbody>();
    offset = transform.position - player.transform.position;
  }

  void LateUpdate() {
    transform.position = player.transform.position + offset;
    // Vector3 playerVelocity = new Vector3(
    //   playerRb.linearVelocity.x,
    //   0f,
    //   playerRb.linearVelocity.z
    // );

    // if (playerVelocity.magnitude > .1f) {
    //   Quaternion targetRotation = Quaternion.LookRotation(playerVelocity);

    //   transform.rotation = Quaternion.Slerp(
    //     transform.rotation,
    //     targetRotation,
    //     rotationSpeed * Time.deltaTime
    //   );
    // }

    // Vector3 targetPosition = player.transform.position - transform.forward * distanceFromPlayer + Vector3.up * height;

    // transform.position = targetPosition;
  }

}
