using UnityEngine;

public class PlayerController : MonoBehaviour {
  private PlayerControls playerControls;
  private PlayerMovement playerMovement;
  private PlayerCameraController playerCameraController;

  private SurfaceConditionHandler surfaceConditionHandler;
  private GroundChecker groundChecker;

  private void Awake() {
    playerControls = GetComponent<PlayerControls>();
    playerMovement = GetComponent<PlayerMovement>();
    playerCameraController = GetComponent<PlayerCameraController>();

    surfaceConditionHandler = GetComponent<SurfaceConditionHandler>();
    groundChecker = GetComponent<GroundChecker>();
  }

  private void FixedUpdate() {
    Vector3 movement = GetCameraRelativeMovement();
    playerCameraController.HandleMovementFOV(movement);

    groundChecker.CheckGroundStatus();

    if (groundChecker.IsGrounded) {
      if (movement.magnitude > 0) {
        playerMovement.HandleGroundMovement(movement);
      } else {
        playerMovement.HandleIdleMovement();
      }
    } else if (movement.magnitude > 0) {
      playerMovement.HandleAirMovement(movement);
    }

    playerMovement.EnforceSpeedLimit();
  }

  // Rotate movement vector inline with camera rotation
  private Vector3 GetCameraRelativeMovement() {
    Vector3 inputDirection = playerControls.GetMovementVector();

    if (inputDirection.magnitude <= 0) {
      return Vector3.zero;
    }

    float cameraYRotation = playerCameraController.GetCameraRotation().eulerAngles.y;
    Quaternion cameraRotation = Quaternion.Euler(0f, cameraYRotation, 0f);

    Vector3 cameraRelativeMovement = cameraRotation * inputDirection;

    return cameraRelativeMovement.normalized;
  }

  private void OnCollisionEnter(Collision collision) {
    ApplyMaterialConditions(collision.gameObject);
    playerCameraController.HandleCollisionShake(collision);
  }


  private void ApplyMaterialConditions(GameObject gameObject) {
    SurfaceCondition condition = surfaceConditionHandler.GetCondition(gameObject);
    playerMovement.ApplySurfaceCondition(condition);
  }
}
