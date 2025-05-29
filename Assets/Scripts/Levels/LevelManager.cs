using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {
  public static LevelManager instance;
  
  public event Action<BaseLevel> OnLevelStarted;
  public event Action<BaseLevel> OnLevelEnded;
  public event Action OnLevelStopped;
  public event Action<BaseLevel> OnLevelPaused;
  public event Action<BaseLevel> OnLevelResumed;
  public event Action<BaseLevel> OnLevelRestarted;
  
  
  [SerializeField] private LevelDatabase levelDatabase;

  private LevelSceneLoader levelSceneLoader;
  private LevelResultsRepository levelRepository;
  private LevelSelectUIController levelSelectUIController;
  private LevelInspectorUIController levelInspectorUIController;
  
  private string currentLevelScene;
  private BaseLevel selectedLevel;

  private bool isPaused = false;

  private void Awake() {
    levelRepository = GetComponent<LevelResultsRepository>();
    levelSceneLoader = GetComponent<LevelSceneLoader>();
    levelSelectUIController = GetComponent<LevelSelectUIController>();
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
    selectedLevel = allLevels.FirstOrDefault();
    
    levelSelectUIController.PopulateLevelButtons(allLevels, OnLevelClicked, levelInspectorUIController.UpdateUI, selectedLevel);
    levelInspectorUIController.UpdateUI(selectedLevel);
  }
  
  private void Update() {
    if (!string.IsNullOrEmpty(currentLevelScene) && Input.GetKeyDown(KeyCode.Escape)) {
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
    if (!string.IsNullOrEmpty(currentLevelScene)) {
      levelSceneLoader.UnloadScene(currentLevelScene, () => {
        levelSceneLoader.LoadScene(levelData.sceneName);
        currentLevelScene = levelData.sceneName;
        OnLevelStarted?.Invoke(levelData);
      });
    } else {
      levelSceneLoader.LoadScene(levelData.sceneName);
      currentLevelScene = levelData.sceneName;
      OnLevelStarted?.Invoke(levelData);
    }
  }

  public void UnloadCurrentLevel(Action onUnloaded = null) {
    if (string.IsNullOrEmpty(currentLevelScene)) {
      Debug.LogWarning("LevelManager: No level is currently loaded to unload");
      onUnloaded?.Invoke();
      return;
    }

    OnLevelEnded?.Invoke(selectedLevel);

    levelSceneLoader.UnloadScene(currentLevelScene, () => {
      Debug.Log($"LevelManager: Level '{currentLevelScene}' unloaded.");
      onUnloaded?.Invoke();
    });
    currentLevelScene = null;
  }

  public void SubmitResult(string levelName, float time) {
    levelRepository.SubmitLevelResult(levelName, time);
    levelSelectUIController.RefreshMedals();
  }

  public void EndLevel() {
    if (selectedLevel != null) {
      OnLevelEnded?.Invoke(selectedLevel);
    }
  }
  
  public void StopLevel() {
    if (string.IsNullOrEmpty(currentLevelScene)) {
      Debug.LogWarning("LevelManager: No level is currently loaded to stop");
      OnLevelStopped?.Invoke();
      return;
    }

    Debug.Log($"LevelManager: Level '{currentLevelScene}' stopped by user.");

    levelSceneLoader.UnloadScene(currentLevelScene, () => {
      currentLevelScene = null;
      OnLevelStopped?.Invoke();
    });
  }

  public void RestartLevel() {
    if (!string.IsNullOrEmpty(currentLevelScene)) {
      levelSceneLoader.UnloadScene(currentLevelScene, () => {
        levelSceneLoader.LoadScene(currentLevelScene, () => {
          OnLevelRestarted?.Invoke(selectedLevel);
        });
      });
    }
  }

  public void PlayerDeathRestartLevel(float delay) {
    StartCoroutine(RestartCoroutine(delay));
  }

  private IEnumerator RestartCoroutine(float delay) {
    yield return new WaitForSeconds(delay);
    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
  }
}
