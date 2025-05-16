using System.Collections.Generic;
using UnityEngine;

public class SkinEquippedUIController : MonoBehaviour {
  [SerializeField] private SkinDetailsUI marbleDetails;
  [SerializeField] private SkinDetailsUI trailDetails;
  [SerializeField] private Transform accessoryDetailsParent;
  [SerializeField] private GameObject accessoryDetailPrefab;
  
  private readonly List<SkinDetailsUI> accessoryDetailInstances = new();

  public void UpdateMarbleDetails(BaseSkin marbleSkin) {
    marbleDetails.SetSkin(marbleSkin);
  }
  public void UpdateTrailDetails(BaseSkin trailSkin) {
    trailDetails.SetSkin(trailSkin);
  }
  public void UpdateAccessoryDetails(List<BaseSkin> accessorySkins) {
    foreach (var instance in accessoryDetailInstances) {
      Destroy(instance.gameObject);
    }
    accessoryDetailInstances.Clear();

    // Add an empty accessory detail placeholder
    if (accessorySkins.Count == 0) { 
      accessorySkins.Add(null);
    }

    foreach (var accessory in accessorySkins) {
      GameObject go = Instantiate(accessoryDetailPrefab, accessoryDetailsParent);
      SkinDetailsUI detail = go.GetComponent<SkinDetailsUI>();
      detail.SetSkin(accessory);
      accessoryDetailInstances.Add(detail);
    }
  }
}
