using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour {
  private float Horizontal;
  private float Vertical;

  public event Action OnJump;

  private void OnMove(InputValue movementValue) {
    Vector2 moveVector = movementValue.Get<Vector2>();
    Horizontal = moveVector.x;
    Vertical = moveVector.y;
  }

  private void FixedUpdate() {
    ReadActionInput();
  }

  private void ReadActionInput() {
    if (Keyboard.current.spaceKey.isPressed) {
      OnJump();
    }
  }

  public Vector3 GetMovementVector() {
    Vector3 rawInput = new(Horizontal, 0f, Vertical);
    return rawInput.normalized;
  }
}
