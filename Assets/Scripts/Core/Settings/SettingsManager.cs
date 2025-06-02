using System;
using System.IO;
using UnityEngine;

[Serializable]
public class GameSettings {
  public float masterVolume = 1f;
  public bool enablePostProcessing = true;
}

public class SettingsManager {
  private readonly string savePath;

  private static SettingsManager _instance;
  public static SettingsManager Instance => _instance ??= new();

  public GameSettings Settings { get; private set; }
  public event Action<GameSettings> OnSettingsChanged;

  private SettingsManager() {
    savePath = Path.Combine(Application.persistentDataPath, "settings.json");
    LoadSettings();
  }

  public void LoadSettings() {
    if (File.Exists(savePath)) {
      string json = File.ReadAllText(savePath);
      Settings = JsonUtility.FromJson<GameSettings>(json);
    } else {
      Settings = new GameSettings();
    }

    OnSettingsChanged?.Invoke(Settings);
  }

  public void SaveSettings() {
    string json = JsonUtility.ToJson(Settings, true);
    File.WriteAllText(savePath, json);
    OnSettingsChanged?.Invoke(Settings);
  }

  public void UpdateSetting(Action<GameSettings> updateAction) {
    updateAction?.Invoke(Settings);
    SaveSettings();
  }

  public void ResetToDefaults() {
    Settings = new GameSettings();
    SaveSettings();
  }
}
