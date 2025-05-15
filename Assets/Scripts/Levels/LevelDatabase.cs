using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Levels/Level Database")]
public class LevelDatabase : ScriptableObject {
  public List<BaseLevel> levels;
}
