using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CustomizationManager : MonoBehaviour {
  [SerializeField] private SkinDatabase skinDatabase;
  
  private CustomizationRepository customizationRepository;
  private SkinSelectionUIController skinSelectionUIController;
  private SkinEquippedUIController skinEquippedUIController;

  public SkinDatabase GetSkinDatabase() => skinDatabase;

  private void Awake() {
    customizationRepository = GetComponent<CustomizationRepository>();
    skinSelectionUIController = GetComponent<SkinSelectionUIController>();
    skinEquippedUIController = GetComponent<SkinEquippedUIController>();
  }

  private void Start() {
    skinSelectionUIController.PopulateSkins(skinDatabase, onSkinSelected);
    skinSelectionUIController.HighlightMarble(customizationRepository.GetSelectedMarble());
    skinSelectionUIController.HighlightTrail(customizationRepository.GetSelectedTrail());
    skinSelectionUIController.HighlightAccessories(customizationRepository.GetSelectedAccessories().ToList());

    skinEquippedUIController.UpdateMarbleDetails(
      skinDatabase.marbles.FirstOrDefault(m => m.skinName == customizationRepository.GetSelectedMarble())
    );

    skinEquippedUIController.UpdateTrailDetails(
      skinDatabase.trails.FirstOrDefault(t => t.skinName == customizationRepository.GetSelectedTrail())
    );

    List<BaseSkin> accessories = customizationRepository.GetSelectedAccessories()
      .Select(name => skinDatabase.accessories.FirstOrDefault(a => a.skinName == name))
      .Where(skin => skin != null)
      .ToList<BaseSkin>();

    skinEquippedUIController.UpdateAccessoryDetails(accessories);
  }

  private void onSkinSelected(BaseSkin selectedSkin)
  {
    switch (selectedSkin)
    {
      case MarbleSkin:
        customizationRepository.SetSelectedMarble(selectedSkin.skinName);
        string marbleName = customizationRepository.GetSelectedMarble();
        skinSelectionUIController.HighlightMarble(marbleName);

        var equippedMarble = skinDatabase.marbles.FirstOrDefault(m => m.skinName == marbleName);
        skinEquippedUIController.UpdateMarbleDetails(equippedMarble);
        break;

      case TrailSkin:
        customizationRepository.SetSelectedTrail(selectedSkin.skinName);
        string trailName = customizationRepository.GetSelectedTrail();
        skinSelectionUIController.HighlightTrail(trailName);

        var equippedTrail = skinDatabase.trails.FirstOrDefault(t => t.skinName == trailName);
        skinEquippedUIController.UpdateTrailDetails(equippedTrail);
        break;

      case AccessorySkin:
        customizationRepository.ToggleAccessory(selectedSkin.skinName);
        skinSelectionUIController.HighlightAccessories(customizationRepository.GetSelectedAccessories().ToList());
        List<BaseSkin> updatedAccessories = customizationRepository.GetSelectedAccessories()
          .Select(name => skinDatabase.accessories.FirstOrDefault(a => a.skinName == name))
          .Where(skin => skin != null)
          .ToList<BaseSkin>();

        skinEquippedUIController.UpdateAccessoryDetails(updatedAccessories);
        break;
    }
  }
}