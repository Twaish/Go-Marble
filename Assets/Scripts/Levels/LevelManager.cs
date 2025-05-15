using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {
  public static LevelManager instance;

  private LevelSelectUI levelSelectUI;
  private LevelRepository levelRepository;
  private LevelSceneLoader levelSceneLoader;
  private LevelInspectorUI levelInspectorUI;
  
  private string currentLevelScene;
  private BaseLevel selectedLevel;

  private void Awake() {
    levelInspectorUI = GetComponent<LevelInspectorUI>();
    levelSceneLoader = GetComponent<LevelSceneLoader>();
    levelRepository = GetComponent<LevelRepository>();
    levelSelectUI  = GetComponent<LevelSelectUI>();

    if (instance == null) {
      instance = this;
      DontDestroyOnLoad(gameObject);
    } else {
      Destroy(gameObject);
    }
  }

  private void Start() {
    IReadOnlyList<BaseLevel> allLevels = levelRepository.GetAllLevels();
    selectedLevel = allLevels.FirstOrDefault();
    
    levelSelectUI.PopulateLevelButtons(allLevels, OnLevelClicked, selectedLevel);
    levelInspectorUI.UpdateUI(selectedLevel);
  }

  private void OnLevelClicked(BaseLevel clickedLevel) {
    if (clickedLevel == selectedLevel) {
      LoadLevel(clickedLevel);

      // Test submit time and refresh medals
      var time = 9.24f;
      levelRepository.SubmitLevelResult(clickedLevel.levelName, time);
      levelSelectUI.RefreshMedals();
    } else {
      selectedLevel = clickedLevel;
      
      levelSelectUI.HighlightSelected(clickedLevel);
      levelInspectorUI.UpdateUI(clickedLevel);
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
