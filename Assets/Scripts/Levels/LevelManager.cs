using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelManager : MonoBehaviour {
  public static LevelManager instance;
  [SerializeField] private LevelDatabase levelDatabase;
  
  public event Action<BaseLevel> OnLevelStarted;
  public event Action<BaseLevel> OnLevelEnded;
  public event Action OnLevelStopped;
  public event Action<BaseLevel> OnLevelPaused;
  public event Action<BaseLevel> OnLevelResumed;
  public event Action<BaseLevel> OnLevelRestarted;

  private LevelSceneLoader levelSceneLoader;
  private LevelResultsRepository levelRepository;
  private LevelSelectUIController levelSelectUIController;
  private LevelResultUIController levelResultUIController;
  private LevelInspectorUIController levelInspectorUIController;
  
  private BaseLevel currentLevelScene;
  private BaseLevel selectedLevel;

  private bool isPaused = false;

  private void Awake() {
    levelRepository = GetComponent<LevelResultsRepository>();
    levelSceneLoader = GetComponent<LevelSceneLoader>();
    levelSelectUIController = GetComponent<LevelSelectUIController>();
    levelResultUIController = GetComponent<LevelResultUIController>();
    levelInspectorUIController = GetComponent<LevelInspectorUIController>();

    if (instance == null) {
      instance = this;
    }
    else {
      Destroy(gameObject);
    }
  }

  private void Start() {
    IReadOnlyList<BaseLevel> allLevels = levelDatabase.levels;
    var sortedLevels = allLevels.OrderBy(level => level.levelIndex).ToList();
    selectedLevel = sortedLevels.FirstOrDefault();
    
    levelSelectUIController.PopulateLevelButtons(sortedLevels, OnLevelClicked, levelInspectorUIController.UpdateUI, selectedLevel);
    levelInspectorUIController.UpdateUI(selectedLevel);
  }

  private void OnPause() {
    if (currentLevelScene != null) {
      TogglePause();
    }
  }

  private void TogglePause() {
    if (isPaused) {
      ResumeLevel();
    } else {
      PauseLevel();
    }
  }

  public void ResumeLevel() {
    if (selectedLevel != null) {
      isPaused = false;
      OnLevelResumed?.Invoke(selectedLevel);
      Time.timeScale = 1f;
    }
  }

  public void PauseLevel() {
    if (selectedLevel != null) {
      isPaused = true;
      OnLevelPaused?.Invoke(selectedLevel);
      Time.timeScale = 0f;
    }
  }

  private void OnLevelClicked(BaseLevel clickedLevel) {
    if (clickedLevel == selectedLevel) {
      LoadLevel(clickedLevel);
    }
    else {
      selectedLevel = clickedLevel;

      levelSelectUIController.HighlightSelected(clickedLevel);
      levelInspectorUIController.UpdateUI(clickedLevel);
    }
  }

  public void LoadLevel(BaseLevel levelData) {
    if (currentLevelScene != null) {
      levelSceneLoader.UnloadScene(currentLevelScene.sceneName, () => {
        levelSceneLoader.LoadScene(levelData.sceneName);
        currentLevelScene = levelData;
        OnLevelStarted?.Invoke(levelData);
      });
    } else {
      levelSceneLoader.LoadScene(levelData.sceneName);
      currentLevelScene = levelData;
      OnLevelStarted?.Invoke(levelData);
    }
  }

  public void UnloadCurrentLevel(Action onUnloaded = null) {
    if (currentLevelScene == null) {
      Debug.LogWarning("LevelManager: No level is currently loaded to unload");
      onUnloaded?.Invoke();
      return;
    }

    OnLevelEnded?.Invoke(selectedLevel);

    levelSceneLoader.UnloadScene(currentLevelScene.sceneName, () => {
      Debug.Log($"LevelManager: Level '{currentLevelScene.levelName}' unloaded.");
      onUnloaded?.Invoke();
    });
    currentLevelScene = null;
  }

  public void SubmitResult(string levelName, float time) {
    levelRepository.SubmitLevelResult(levelName, time);
    levelSelectUIController.RefreshMedals();
  }

  public void EndLevel() {
    if (currentLevelScene == null) return;
    levelSceneLoader.UnloadScene(currentLevelScene.sceneName, () => {
      OnLevelEnded?.Invoke(currentLevelScene);
      levelResultUIController.UpdateUI(currentLevelScene);
      currentLevelScene = null;
    });
  }
  
  public void StopLevel() {
    if (currentLevelScene == null) {
      Debug.LogWarning("LevelManager: No level is currently loaded to stop");
      OnLevelStopped?.Invoke();
      return;
    }

    Debug.Log($"LevelManager: Level '{currentLevelScene.levelName}' stopped by user.");

    levelSceneLoader.UnloadScene(currentLevelScene.sceneName, () => {
      currentLevelScene = null;
      OnLevelStopped?.Invoke();
    });
  }

  public void RestartLevel() {
    if (currentLevelScene == null) return;
    levelSceneLoader.UnloadScene(currentLevelScene.sceneName, () => {
      levelSceneLoader.LoadScene(currentLevelScene.sceneName, () => {
        OnLevelRestarted?.Invoke(currentLevelScene);
      });
    });
  }

  public void PlayerDeathRestartLevel(float delay) {
    StartCoroutine(RestartCoroutine(delay));
  }

  private IEnumerator RestartCoroutine(float delay) {
    yield return new WaitForSeconds(delay);
    RestartLevel();
  }
}
