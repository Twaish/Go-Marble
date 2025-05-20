using System.Collections.Generic;
using UnityEngine;

public class AutoScrollManager : MonoBehaviour {
  [SerializeField] private SkinDatabase skinDatabase;
  [SerializeField] private GameObject VerticalSection;
  [SerializeField] private GameObject HorizontalSection;
  [SerializeField] private GameObject GridSection;

  private void Start() {
    var vertical = VerticalSection.GetComponent<SkinTileGroupSpawner>();
    vertical.SpawnTiles(skinDatabase.marbles);

    var horizontal = HorizontalSection.GetComponent<SkinTileGroupSpawner>();
    horizontal.SpawnTiles(skinDatabase.marbles);

    var grid = GridSection.GetComponent<SkinTileGroupSpawner>();
    grid.SpawnTiles(skinDatabase.marbles);
  }
}
