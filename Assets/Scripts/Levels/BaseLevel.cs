using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MedalThresholds {
  public float goldTime;
  public float silverTime;
  public float bronzeTime;
}

[Serializable]
public class LevelProgress {
  public string levelName;
  public string playerName;
  public Medal bestMedal;
  public float bestTime;
}

[Serializable]
public class PlayerProgressData {
  public List<LevelProgress> levels = new();
}

[CreateAssetMenu(fileName = "BaseLevel", menuName = "Game/Level Data")]
public class BaseLevel : ScriptableObject {
  public string levelName;
  public string sceneName;
  public string description;
  public int levelIndex;
  public Sprite previewImage;

  public MedalThresholds medalThresholds; 
}