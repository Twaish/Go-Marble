using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {
  private Dictionary<string, PlayerResult> playerResults;
  private List<LevelInfo> levels = new() {
    new(
      "Something",
      "ImageName",
      20, 25, 30
    )
  };

  void Awake() {
    
  }
}
