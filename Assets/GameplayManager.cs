using UnityEngine;

public class GameplayManager : MonoBehaviour {
  [SerializeField] private TimerManager timerManager;
  [SerializeField] private NavigationManager navigationManager;

  private void Awake() {
    LevelManager.instance.OnLevelStarted += HandleLevelStarted;
    LevelManager.instance.OnLevelEnded += HandleLevelEnded;
    LevelManager.instance.OnLevelPaused += HandleLevelPaused;
  }

  private void OnDestroy() {
    if (LevelManager.instance != null) {
      LevelManager.instance.OnLevelStarted -= HandleLevelStarted;
      LevelManager.instance.OnLevelEnded -= HandleLevelEnded;
      LevelManager.instance.OnLevelPaused -= HandleLevelPaused;
    }
  }

  private void HandleLevelStarted(BaseLevel level) {
    timerManager.ResetTimer();
    timerManager.StartTimer();
    navigationManager.OpenMenu("Gameplay");
    Debug.Log("STARTED LEVEL");
  }

  private void HandleLevelEnded(BaseLevel level) {
    Debug.Log($"Completed level {level.levelName} in {timerManager.GetTime():F2} seconds");
    timerManager.StopTimer();
    navigationManager.OpenMenu("LevelCompleteMenu");
  }

  private void HandleLevelPaused(BaseLevel level) {
    timerManager.PauseTimer();
    Debug.Log("PAUSED LEVEL");
    navigationManager.OpenMenu("PauseMenu");
  }
  
}
