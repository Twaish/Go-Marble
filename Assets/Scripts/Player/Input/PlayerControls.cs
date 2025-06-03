using System;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour {
  private float Horizontal;
  private float Vertical;
  private bool ShouldJump;

  public event Action OnJump;
  public event Action<float> OnLook;
  public event Action OnUsePowerUp;

  private void OnMove(InputValue movementValue) {
    Vector2 moveVector = movementValue.Get<Vector2>();
    Horizontal = moveVector.x;
    Vertical = moveVector.y;
  }

  private void OnRotateCamera(InputValue inputValue) {
    OnLook?.Invoke(inputValue.Get<float>());
  }

  private void OnCustomJump(InputValue inputValue) {
    ShouldJump = inputValue.Get<float>() > 0f;
  }

  private void OnPowerUp(InputValue _) {
    OnUsePowerUp?.Invoke();
  }

  private void FixedUpdate() {
    ReadActionInput();
  }

  private void ReadActionInput() {
    if (ShouldJump) {
      OnJump();
    }
  }

  public Vector3 GetMovementVector() {
    Vector3 rawInput = new(Horizontal, 0f, Vertical);
    return rawInput.normalized;
  }
}
