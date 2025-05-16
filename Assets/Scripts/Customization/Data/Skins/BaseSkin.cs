using UnityEngine;

public abstract class BaseSkin : ScriptableObject {
  public string skinName;
  [TextArea] public string description;
  public Sprite icon;
}