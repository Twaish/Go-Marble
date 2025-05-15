using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Levels/LevelDatabase")]
public class LevelDatabase : ScriptableObject {
  public List<BaseLevel> levels;
}
