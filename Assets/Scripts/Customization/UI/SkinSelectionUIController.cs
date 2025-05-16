using System;
using System.Collections.Generic;
using UnityEngine;

public class SkinSelectionUIController : MonoBehaviour {
  [SerializeField] private GameObject skinTilePrefab;
  
  [SerializeField] private Transform marbleContainer;
  [SerializeField] private Transform trailContainer;
  [SerializeField] private Transform accessoryContainer;

  private Dictionary<BaseSkin, SkinTileUI> marbleTiles = new();
  private Dictionary<BaseSkin, SkinTileUI> trailTiles = new();
  private Dictionary<BaseSkin, SkinTileUI> accessoryTiles = new();

  public void PopulateSkins(SkinDatabase database, Action<BaseSkin> onSelect) {
    marbleTiles = PopulateList(database.marbles, marbleContainer, onSelect);
    trailTiles = PopulateList(database.trails, trailContainer, onSelect);
    accessoryTiles = PopulateList(database.accessories, accessoryContainer, onSelect);
  }

  private Dictionary<BaseSkin, SkinTileUI> PopulateList<T>(List<T> skins, Transform container, Action<BaseSkin> onSelect) where T : BaseSkin {
    foreach (Transform child in container) Destroy(child.gameObject);

    var dict = new Dictionary<BaseSkin, SkinTileUI>();
    foreach (var skin in skins) {
      var tile = Instantiate(skinTilePrefab, container).GetComponent<SkinTileUI>();
      tile.Setup(skin, () => onSelect(skin));
      dict[skin] = tile;
    }
    return dict;
  }
  
  public void HighlightMarble(string marbleName) {
    foreach (var kvp in marbleTiles)
      kvp.Value.SetSelected(kvp.Key.skinName == marbleName);
  }

  public void HighlightTrail(string trailName) {
    foreach (var kvp in trailTiles)
      kvp.Value.SetSelected(kvp.Key.skinName == trailName);
  }

  public void HighlightAccessories(List<string> accessoryNames) {
    foreach (var kvp in accessoryTiles)
      kvp.Value.SetSelected(accessoryNames.Contains(kvp.Key.skinName));
  }
}
