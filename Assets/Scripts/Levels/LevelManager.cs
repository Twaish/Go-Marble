using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {
  public static LevelManager instance;
  
  [SerializeField] private LevelDatabase levelDatabase;

  private LevelSceneLoader levelSceneLoader;
  private LevelResultsRepository levelRepository;
  private LevelSelectUIController levelSelectUIController;
  private LevelInspectorUIController levelInspectorUIController;
  
  private string currentLevelScene;
  private BaseLevel selectedLevel;

  private void Awake() {
    levelRepository = GetComponent<LevelResultsRepository>();
    levelSceneLoader = GetComponent<LevelSceneLoader>();
    levelSelectUIController = GetComponent<LevelSelectUIController>();
    levelInspectorUIController = GetComponent<LevelInspectorUIController>();

    if (instance == null) {
      instance = this;
      DontDestroyOnLoad(gameObject);
    } else {
      Destroy(gameObject);
    }
  }

  private void Start() {
    IReadOnlyList<BaseLevel> allLevels = levelDatabase.levels;
    selectedLevel = allLevels.FirstOrDefault();
    
    levelSelectUIController.PopulateLevelButtons(allLevels, OnLevelClicked, levelInspectorUIController.UpdateUI, selectedLevel);
    levelInspectorUIController.UpdateUI(selectedLevel);
  }

  private void OnLevelClicked(BaseLevel clickedLevel) {
    if (clickedLevel == selectedLevel) {
      LoadLevel(clickedLevel);

      // Test submit time and refresh medals
      var time = 9.24f;
      levelRepository.SubmitLevelResult(clickedLevel.levelName, time);
      levelSelectUIController.RefreshMedals();
    } else {
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
      });
    } else {
      levelSceneLoader.LoadScene(levelData.sceneName);
      currentLevelScene = levelData.sceneName;
    }
  }

  // TO BE USED
  public void RestartLevel() {
    if (!string.IsNullOrEmpty(currentLevelScene)) {
      levelSceneLoader.UnloadScene(currentLevelScene, () => {
        levelSceneLoader.LoadScene(currentLevelScene);
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
