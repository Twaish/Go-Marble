using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

  public static LevelManager instance; // Singleton for easy access
  private Dictionary<string, PlayerResult> playerResults;
  private List<LevelInfo> levels = new() {
    new(
      "Something",
      "ImageName",
      20, 25, 30
    )
  };

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Optional: Persist between scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

      public void PlayerDeathRestartLevel(float delay)
    {
        StartCoroutine(RestartCoroutine(delay));
    }

    private System.Collections.IEnumerator RestartCoroutine(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
