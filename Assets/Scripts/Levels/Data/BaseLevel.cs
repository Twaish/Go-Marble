using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Levels/Level")]
public class BaseLevel : ScriptableObject {
  public string levelName;
  public string sceneName;
  public string description;
  public int levelIndex;
  public Sprite previewImage;

  public MedalThresholds medalThresholds; 
}

[Serializable]
public class MedalThresholds {
  public float goldTime;
  public float silverTime;
  public float bronzeTime;
}