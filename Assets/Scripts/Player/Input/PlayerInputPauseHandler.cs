using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputPauseHandler : MonoBehaviour {
  private PlayerInput playerInput;

  private void Awake() {
    playerInput = GetComponent<PlayerInput>();
    if (playerInput == null) {
      Debug.LogError("PlayerInputPauseHandler: PlayerInput not found");
      enabled = false;
      return;
    }

    LevelManager.instance.OnLevelPaused += DisableInput;
    LevelManager.instance.OnLevelResumed += EnableInput;
  }

  private void OnPause(InputValue _) {
    LevelManager.instance.TogglePause();
  }

  private void OnDestroy() {
    LevelManager.instance.OnLevelPaused -= DisableInput;
    LevelManager.instance.OnLevelResumed -= EnableInput;
  }

  private void DisableInput(BaseLevel _) {
    playerInput.DeactivateInput();
  }
  private void EnableInput(BaseLevel _) {
    playerInput.ActivateInput();
  }
}
