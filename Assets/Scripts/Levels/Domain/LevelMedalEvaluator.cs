using UnityEngine;

public class LevelMedalEvaluator : MonoBehaviour {
  [SerializeField] private Sprite goldMedalTexture;
  [SerializeField] private Sprite silverMedalTexture;
  [SerializeField] private Sprite bronzeMedalTexture;
  
  private LevelResultsRepository levelRepo;

  private void Awake() {
    levelRepo = GetComponent<LevelResultsRepository>();
  }

  public Texture EvaluateMedalTexture(BaseLevel level, float time) {
    var medalTimes = level.medalThresholds;
    Sprite medalSprite = time < medalTimes.goldTime ? goldMedalTexture :
      time < medalTimes.silverTime ? silverMedalTexture :
      time < medalTimes.bronzeTime ? bronzeMedalTexture :
      null;

    return medalSprite != null ? medalSprite.texture : null;
  }

  public Texture GetMedalTexture(BaseLevel level) {
    float? bestTime = levelRepo.GetBestTimeForLevel(level.levelName);
    if (bestTime == null) return null;

    return EvaluateMedalTexture(level, bestTime.Value);
  }
}
