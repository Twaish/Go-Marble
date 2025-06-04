using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSceneLoader : MonoBehaviour {
  public Coroutine LoadScene(string sceneName, Action onLoaded = null) {
    return StartCoroutine(LoadRoutine());
    IEnumerator LoadRoutine() {
      AsyncOperation op = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
      yield return new WaitUntil(() => op.isDone);

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
