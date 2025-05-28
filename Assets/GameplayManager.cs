using System.Collections;
using UnityEngine;

public class GameplayManager : MonoBehaviour {
  [SerializeField] private TimerManager timerManager;
  [SerializeField] private NavigationManager navigationManager;
  [SerializeField] private GameObject completeLevelFocus;
  [SerializeField] private GameObject pausedLevelFocus;

  private void Awake() {
    LevelManager.instance.OnLevelStarted += HandleLevelStarted;
    LevelManager.instance.OnLevelEnded += HandleLevelEnded;
    LevelManager.instance.OnLevelPaused += HandleLevelPaused;
    LevelManager.instance.OnLevelResumed += HandleLevelResumed;
  }

  private void OnDestroy() {
    if (LevelManager.instance != null) {
      LevelManager.instance.OnLevelStarted -= HandleLevelStarted;
      LevelManager.instance.OnLevelEnded -= HandleLevelEnded;
      LevelManager.instance.OnLevelPaused -= HandleLevelPaused;
      LevelManager.instance.OnLevelResumed -= HandleLevelResumed;
    }
  }

  private void HandleLevelStarted(BaseLevel level) {
    Debug.Log("STARTED LEVEL");
    timerManager.ResetTimer();
    timerManager.StartTimer();
    navigationManager.OpenMenu("System/None");
    navigationManager.OpenMenu("Gameplay/Gameplay");
  }

  private void HandleLevelEnded(BaseLevel level) {
    Debug.Log($"Completed level {level.levelName} in {timerManager.GetTime():F2} seconds");
    timerManager.StopTimer();
    LevelManager.instance.ResumeLevel();
    navigationManager.OpenMenu("System/LevelCompleteMenu");
    navigationManager.OpenMenu("Gameplay/None");
    navigationManager.Focus(completeLevelFocus);
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
  
}
