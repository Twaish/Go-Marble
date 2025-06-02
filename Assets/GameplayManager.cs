using System.Collections;
using UnityEngine;

public class GameplayManager : MonoBehaviour {
  [SerializeField] private GameObject mainCamera;

  [Header("Managers")]
  [SerializeField] private TimerManager timerManager;
  [SerializeField] private PowerUpManager powerUpManager;
  [SerializeField] private NavigationManager navigationManager;

  [Header("GUI")]
  [Tooltip("What to focus on when completing a level")]
  [SerializeField] private GameObject completeLevelFocus;
  [Tooltip("What to focus on when pausing a level")]
  [SerializeField] private GameObject pausedLevelFocus;

  private void Awake() {
    LevelManager.instance.OnLevelStarted += HandleLevelStarted;
    LevelManager.instance.OnLevelEnded += HandleLevelEnded;
    LevelManager.instance.OnLevelStopped += HandleLevelStopped;
    LevelManager.instance.OnLevelPaused += HandleLevelPaused;
    LevelManager.instance.OnLevelResumed += HandleLevelResumed;
    LevelManager.instance.OnLevelRestarted += HandleLevelRestarted;
  }

  private void OnDestroy() {
    if (LevelManager.instance != null) {
      LevelManager.instance.OnLevelStarted -= HandleLevelStarted;
      LevelManager.instance.OnLevelEnded -= HandleLevelEnded;
      LevelManager.instance.OnLevelStopped -= HandleLevelStopped;
      LevelManager.instance.OnLevelPaused -= HandleLevelPaused;
      LevelManager.instance.OnLevelResumed -= HandleLevelResumed;
      LevelManager.instance.OnLevelRestarted -= HandleLevelRestarted;
    }
  }

  private void HandleLevelStarted(BaseLevel level) {
    Debug.Log("STARTED LEVEL");
    timerManager.ResetTimer();
    timerManager.StartTimer();
    navigationManager.OpenMenu("System/None");
    navigationManager.OpenMenu("Gameplay/Gameplay");
    powerUpManager.ClearPowerUp();
    mainCamera.SetActive(false);
  }

  private void HandleLevelEnded(BaseLevel level) {
    Debug.Log($"Completed level {level.levelName} in {timerManager.GetTime():F2} seconds");
    timerManager.StopTimer();
    LevelManager.instance.ResumeLevel();
    LevelManager.instance.SubmitResult(level.levelName, timerManager.GetTime());
    navigationManager.OpenMenu("System/LevelCompleteMenu");
    navigationManager.OpenMenu("Gameplay/None");
    navigationManager.Focus(completeLevelFocus);
    powerUpManager.ClearPowerUp();
    mainCamera.SetActive(true);
  }

  private void HandleLevelStopped() {
    timerManager.StopTimer();
    LevelManager.instance.ResumeLevel();
    navigationManager.OpenMenu("System/LevelSelect");
    powerUpManager.ClearPowerUp();
    mainCamera.SetActive(true);
  }

  private void HandleLevelRestarted(BaseLevel level) {
    timerManager.ResetTimer();
    LevelManager.instance.ResumeLevel();
    timerManager.StartTimer();
  }

  private void HandleLevelPaused(BaseLevel level) {
    Debug.Log("PAUSED LEVEL");
    timerManager.PauseTimer();
    navigationManager.OpenMenu("System/PauseMenu");
    StartCoroutine(DelayFocus(pausedLevelFocus));
  }

  private void HandleLevelResumed(BaseLevel level) {
    timerManager.ResumeTimer();
    navigationManager.OpenMenu("System/None");
    Debug.Log("RESUMED LEVEL");
  }


  private IEnumerator DelayFocus(GameObject gameObject) {
    yield return null;
    navigationManager.Focus(gameObject);

    Debug.Log("FOCUSING " + gameObject.name);
  }

  public void QuitGame() {
    #if UNITY_EDITOR
      UnityEditor.EditorApplication.isPlaying = false;
    #else
      Application.Quit();
    #endif
  }
}
