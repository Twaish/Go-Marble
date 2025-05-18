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

  private void Awake() {
    marbleHandler = GetComponent<MarbleSkinHandler>();
    trailHandler = GetComponent<TrailSkinHandler>();
    accessoryHandler = GetComponent<AccessorySkinHandler>();
  }

  private void Start() {
    var customizationManager = FindFirstObjectByType<CustomizationManager>();
    if (customizationManager == null) {
      Debug.LogError("SkinApplier: CustomizationManager not found in scene");
      enabled = false;
      return;
    }

    repo = customizationManager.GetComponent<CustomizationRepository>();
    if (repo == null) {
      Debug.LogError("SkinApplier: CustomizationRepository not found on CustomizationManager");
      enabled = false;
      return;
    }

    skinDatabase = customizationManager.GetSkinDatabase();

    repo.OnMarbleChanged += ApplyMarble;
    repo.OnTrailChanged += ApplyTrail;
    repo.OnAccessoriesChanged += ApplyAccessories;

    CustomizationData customizationData = repo.GetCustomizationData();
    ApplyMarble(customizationData.selectedMarble);
    ApplyTrail(customizationData.selectedTrail);
    ApplyAccessories(customizationData.selectedAccessories);
  }
  
  private void OnDestroy() {
    if (repo == null) return; 
    repo.OnMarbleChanged -= ApplyMarble;
    repo.OnTrailChanged -= ApplyTrail;
    repo.OnAccessoriesChanged -= ApplyAccessories;
  }

  private void ApplyMarble(string marbleName)
  {
    MarbleSkin marble = skinDatabase.marbles.FirstOrDefault(m => m.skinName == marbleName);
    marbleHandler.Apply(marble);
  }

  private void ApplyTrail(string trailName)
  {
    TrailSkin trail = skinDatabase.trails.FirstOrDefault(t => t.skinName == trailName);
    trailHandler.Apply(trail);
  }

  private void ApplyAccessories(List<string> accessoryNames)
  {
    List<AccessorySkin> accessories = accessoryNames
      .Select(name => skinDatabase.accessories.FirstOrDefault(a => a.skinName == name))
      .Where(a => a != null)
      .ToList();

    accessoryHandler.Apply(accessories);
  }
}