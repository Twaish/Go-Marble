using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LevelResultsRepository : MonoBehaviour {
  private Dictionary<string, float> levelResults;
  private LevelResultPersistence persistence;

  private void Awake() {
    persistence = new LevelResultPersistence();
    levelResults = persistence.LoadProgress();
  }

  public float? GetBestTimeForLevel(string levelName) {
    return levelResults.TryGetValue(levelName, out var time) ? time : null;
  }

  public void SubmitLevelResult(string levelName, float time) {
    if (levelResults.TryGetValue(levelName, out var existingTime)) {
      if (time >= existingTime) return;
    }

    levelResults[levelName] = time;
    persistence.SaveProgress(levelResults);
  }
}

public class LevelResultPersistence {
  private string filePath => Path.Combine(Application.persistentDataPath, "level_results.json");

  public Dictionary<string, float> LoadProgress() {
    Debug.Log(filePath);
    if (!File.Exists(filePath)) return new();

    string json = File.ReadAllText(filePath);
    LevelResultData wrapper = JsonUtility.FromJson<LevelResultData>(json);
    Dictionary<string, float> result = new();

    if (wrapper != null && wrapper.levelResults != null) {
      foreach (var entry in wrapper.levelResults) {
        result[entry.levelName] = entry.bestTime;
      }
    }

    return result;
  }

  public void SaveProgress(Dictionary<string, float> data) {
    LevelResultData wrapper = new() { 
      levelResults = new List<LevelResultEntry>() 
    };

    foreach (var kvp in data) {
      wrapper.levelResults.Add(new LevelResultEntry {
        levelName = kvp.Key,
        bestTime = kvp.Value
      });
    }

    string json = JsonUtility.ToJson(wrapper, true);
    File.WriteAllText(filePath, json);
  }
}

[Serializable]
public class LevelResultEntry {
  public string levelName;
  public float bestTime;
}
[Serializable]
public class LevelResultData {
  public List<LevelResultEntry> levelResults = new();
}