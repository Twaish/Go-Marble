using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScrollViewController : MonoBehaviour {
  private enum ScrollType {
    Horizontal,
    Vertical
  }

  [SerializeField]
  private ScrollType scrollType = ScrollType.Vertical;

  [SerializeField]
  private RectTransform content;
  [SerializeField]
  private GameObject prefabListItem;

  [SerializeField]
  private SkinTileEvent eventItemOnSelect;

  [SerializeField]
  private int defaultSelectedIndex = 0;

  [SerializeField]
  private int testSkinTileCount = 1;

  private ScrollViewAutoScroll autoScroll;

  void Awake() {
    autoScroll = GetComponent<ScrollViewAutoScroll>();
  }
  void Start() {
    CreateTiles();
  }

  void CreateTiles() {
    for (int i = 0; i < testSkinTileCount; i++) {
      CreateTile("Tile_" + i);
    }
  }

  SkinTileUI CreateTile(string name) {
    GameObject skinTileGO = Instantiate(prefabListItem, Vector3.zero, Quaternion.identity);
    skinTileGO.transform.SetParent(content.transform);
    skinTileGO.transform.localScale = Vector3.one;
    skinTileGO.name = name;

    SkinTileUI skinTile = skinTileGO.GetComponent<SkinTileUI>();
    skinTile.Text = name;

    skinTile.OnSelectEvent.AddListener((tile) => HandleItemOnSelect(tile));

    return skinTile;
  }

  void HandleItemOnSelect(SkinTileUI skinTile) {
    autoScroll.HandleOnSelectChange(skinTile.gameObject);
    eventItemOnSelect.Invoke(skinTile);
  }

  void FixedUpdate() {
    ScrollToSelectedTile();
  }

  public void ScrollToSelectedTile() {
  }
}
