using UnityEngine;

public class TrailSkinHandler : MonoBehaviour
{
  
  private GameObject currentTrail;
    
  public void Apply(TrailSkin trail)
  {
    if (trail == null)
    {
      return;
    }

    if (currentTrail != null)
    {
      Destroy(currentTrail);
    }

    currentTrail = Instantiate(trail.trailPrefab, transform);

    Debug.Log("APPLYING TRAIL: " + trail.skinName);
  }
}
