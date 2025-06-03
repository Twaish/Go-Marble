using UnityEngine;

public class TrailSkinHandler : MonoBehaviour {
  private GameObject currentTrail;
    
  public void Apply(TrailSkin trail) {
    if (currentTrail != null) {
      Destroy(currentTrail);
      currentTrail = null;
    }

    if (trail != null) {
      currentTrail = Instantiate(trail.trailPrefab, transform);
      Debug.Log("[Trail] " + trail.skinName);
    }
  }
}
