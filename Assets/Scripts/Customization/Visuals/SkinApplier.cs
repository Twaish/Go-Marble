using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkinApplier : MonoBehaviour {
  private MarbleSkinHandler marbleHandler;
  private TrailSkinHandler trailHandler;
  private AccessorySkinHandler accessoryHandler;
  
  private CustomizationRepository repo;
  private SkinDatabase skinDatabase;

  private Action<CustomizationData> onCustomizationChanged;

  private void Awake()
  {
    marbleHandler = GetComponent<MarbleSkinHandler>();
    trailHandler = GetComponent<TrailSkinHandler>();
    accessoryHandler = GetComponent<AccessorySkinHandler>();
  }

  private void Start()
  {
    var customizationManager = FindFirstObjectByType<CustomizationManager>();
    if (customizationManager == null)
    {
      Debug.LogError("SkinApplier: CustomizationManager not found in scene");
      enabled = false;
      return;
    }

    repo = customizationManager.GetComponent<CustomizationRepository>();
    if (repo == null)
    {
      Debug.LogError("SkinApplier: CustomizationRepository not found on CustomizationManager");
      enabled = false;
      return;
    }

    skinDatabase = customizationManager.GetSkinDatabase();

    repo.OnCustomizationChanged += ApplyCustomization;

    ApplyCustomization(repo.GetCustomizationData());
  }
  
  private void OnDestroy() {
    if (repo != null && onCustomizationChanged != null) {
      repo.OnCustomizationChanged -= ApplyCustomization;
    }
  }

  public void ApplyCustomization(CustomizationData data)
  {
    var marble = skinDatabase.marbles.FirstOrDefault(m => m.skinName == data.selectedMarble);
    var trail = skinDatabase.trails.FirstOrDefault(t => t.skinName == data.selectedTrail);
    var accessories = data.selectedAccessories
      .Select(name => skinDatabase.accessories.FirstOrDefault(a => a.skinName == name))
      .Where(a => a != null)
      .ToList();

    marbleHandler.Apply(marble);
    trailHandler.Apply(trail);
    accessoryHandler.Apply(accessories);
  }
}