using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CustomizationManager : MonoBehaviour {
  [SerializeField] private SkinDatabase skinDatabase;
  
  private CustomizationRepository customizationRepository;
  private SkinSelectionUI skinSelectionUI;
  private SkinEquippedUI skinEquippedUI;

  private void Awake() {
    customizationRepository = GetComponent<CustomizationRepository>();
    skinSelectionUI = GetComponent<SkinSelectionUI>();
    skinEquippedUI = GetComponent<SkinEquippedUI>();
  }

  private void Start() {
    skinSelectionUI.PopulateSkins(skinDatabase, onSkinSelected);
    skinSelectionUI.HighlightMarble(customizationRepository.GetSelectedMarble());
    skinSelectionUI.HighlightTrail(customizationRepository.GetSelectedTrail());
    skinSelectionUI.HighlightAccessories(customizationRepository.GetSelectedAccessories().ToList());

    skinEquippedUI.UpdateMarbleDetails(
      skinDatabase.marbles.FirstOrDefault(m => m.skinName == customizationRepository.GetSelectedMarble())
    );

    skinEquippedUI.UpdateTrailDetails(
      skinDatabase.trails.FirstOrDefault(t => t.skinName == customizationRepository.GetSelectedTrail())
    );

    List<BaseSkin> accessories = customizationRepository.GetSelectedAccessories()
      .Select(name => skinDatabase.accessories.FirstOrDefault(a => a.skinName == name))
      .Where(skin => skin != null)
      .ToList<BaseSkin>();

    skinEquippedUI.UpdateAccessoryDetails(accessories);
  }

  private void onSkinSelected(BaseSkin selectedSkin) {
    switch (selectedSkin) {
      case MarbleSkin:
        customizationRepository.SetSelectedMarble(selectedSkin.skinName);
        string marbleName = customizationRepository.GetSelectedMarble();
        skinSelectionUI.HighlightMarble(marbleName);
        
        var equippedMarble = skinDatabase.marbles.FirstOrDefault(m => m.skinName == marbleName);
        skinEquippedUI.UpdateMarbleDetails(equippedMarble);
        break;

      case TrailSkin:
        customizationRepository.SetSelectedTrail(selectedSkin.skinName);
        string trailName = customizationRepository.GetSelectedTrail();
        skinSelectionUI.HighlightTrail(trailName);
        
        var equippedTrail = skinDatabase.trails.FirstOrDefault(t => t.skinName == trailName);
        skinEquippedUI.UpdateTrailDetails(equippedTrail);
        break;

      case AccessorySkin:
        customizationRepository.ToggleAccessory(selectedSkin.skinName);
        skinSelectionUI.HighlightAccessories(customizationRepository.GetSelectedAccessories().ToList());
        List<BaseSkin> updatedAccessories = customizationRepository.GetSelectedAccessories()
          .Select(name => skinDatabase.accessories.FirstOrDefault(a => a.skinName == name))
          .Where(skin => skin != null)
          .ToList<BaseSkin>();

        skinEquippedUI.UpdateAccessoryDetails(updatedAccessories);
        break;
    }
  }
}