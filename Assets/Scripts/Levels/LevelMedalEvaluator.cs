using UnityEngine;

public class LevelMedalEvaluator : MonoBehaviour {
  [SerializeField] private Sprite goldMedalTexture;
  [SerializeField] private Sprite silverMedalTexture;
  [SerializeField] private Sprite bronzeMedalTexture;
  
  private LevelRepository levelRepo;

  private void Awake() {
    levelRepo = GetComponent<LevelRepository>();
  }

  public Texture GetMedalTexture(BaseLevel level) {
    float? bestTime = levelRepo.GetBestTimeForLevel(level.levelName);
    if (bestTime == null) return null;

    var medalTimes = level.medalThresholds;
    Sprite medalSprite = bestTime < medalTimes.goldTime ? goldMedalTexture : 
      bestTime < medalTimes.silverTime ? silverMedalTexture : 
      bestTime < medalTimes.bronzeTime ? bronzeMedalTexture : 
      null;
    
    return medalSprite != null ? medalSprite.texture : null;
  }
}
