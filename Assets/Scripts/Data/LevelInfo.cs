using System.Collections.Generic;
using UnityEngine;

public enum Medal {
  None,
  GoldMedal,
  SilverMedal,
  BronzeMedal
}

public class PlayerLevelInfo {
  public string LevelName;
  public int PlayerTime;
  public Medal Medal;
  public PlayerLevelInfo(string levelName, int playerTime, Medal medal) {
    PlayerTime = playerTime;
    Medal = medal;
  }
}

public class PlayerResult {
  public string PlayerName;
  public float Time;
  public Medal Medal;
  public PlayerResult(string playerName, float time, Medal medal) {
    PlayerName = playerName;
    Time = time;
    Medal = medal;
  }
}

public class LevelInfo {
  string Name;
  string PreviewImage;
  float GoldRequirement;
  float SilverRequirement;
  float BronzeRequirement;
  public LevelInfo(
    string name,
    string previewImage, 
    float goldRequirement,
    float silverRequirement,
    float bronzeRequirement
  ) {
    Name = name;
    PreviewImage = previewImage;
    GoldRequirement = goldRequirement;
    SilverRequirement = silverRequirement;
    BronzeRequirement = bronzeRequirement;
  }
}
