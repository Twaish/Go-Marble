using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LevelRepository : MonoBehaviour {
  [SerializeField] private List<BaseLevel> levels;

  public IReadOnlyList<BaseLevel> GetAllLevels() => levels;
  
  private Dictionary<string, float> levelTimes;
  private LevelResultPersistence persistence;

  private void Awake() {
    persistence = new LevelResultPersistence();
    levelTimes = persistence.LoadProgress();
  }

  public float? GetBestTimeForLevel(string levelName) {
    return levelTimes.TryGetValue(levelName, out var time) ? time : null;
  }

  public void SubmitLevelResult(string levelName, float time) {
    if (levelTimes.TryGetValue(levelName, out var existingTime)) {
      if (time >= existingTime) return;
    }

    levelTimes[levelName] = time;
    persistence.SaveProgress(levelTimes);
  }
}

public class LevelResultPersistence {
  private string filePath => Path.Combine(Application.persistentDataPath, "level_times.json");

  public Dictionary<string, float> LoadProgress() {
    Debug.Log(filePath);
    if (!File.Exists(filePath)) return new();

    string json = File.ReadAllText(filePath);
    LevelResultData wrapper = JsonUtility.FromJson<LevelResultData>(json);
    Dictionary<string, float> result = new();

    if (wrapper != null && wrapper.levelTimes != null) {
      foreach (var entry in wrapper.levelTimes) {
        result[entry.levelName] = entry.bestTime;
      }
    }

    return result;
  }

  public void SaveProgress(Dictionary<string, float> data) {
    LevelResultData wrapper = new() { 
      levelTimes = new List<LevelResultEntry>() 
    };

    foreach (var kvp in data) {
      wrapper.levelTimes.Add(new LevelResultEntry {
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
  public List<LevelResultEntry> levelTimes = new();
}