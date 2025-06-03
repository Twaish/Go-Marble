using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class LevelSceneLoader : MonoBehaviour {
  public Coroutine LoadScene(string sceneName, Action onLoaded = null) {
    return StartCoroutine(LoadRoutine());
    IEnumerator LoadRoutine() {
      AsyncOperation op = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
      yield return new WaitUntil(() => op.isDone);

      try {
        var playerInput = FindFirstObjectByType<PlayerInput>();
        if (playerInput != null) {
          var gamepads = InputSystem.devices.Where(d => d is Gamepad).ToList();
          if (gamepads.Count > 0) {
            Debug.Log("Gamepad found");
            playerInput.SwitchCurrentControlScheme("Gamepad", gamepads.FirstOrDefault());
            playerInput.actions.Enable();
          }
        }
      } catch (Exception error) {
        Debug.LogError(error);
      }

      onLoaded?.Invoke();
    }
  }

  public Coroutine UnloadScene(string sceneName, Action onUnloaded = null) {
    return StartCoroutine(UnloadRoutine());
    IEnumerator UnloadRoutine() {
      AsyncOperation op = SceneManager.UnloadSceneAsync(sceneName);
      yield return new WaitUntil(() => op.isDone);
      onUnloaded?.Invoke();
    }
  }
}
