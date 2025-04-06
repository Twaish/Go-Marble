using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PanelManager : MonoBehaviour {
  private class NavigationEntry {
    public string MenuName;
    public GameObject FocusedObject;
    public NavigationEntry(string menuName, GameObject focusedObject) {
      MenuName = menuName;
      FocusedObject = focusedObject;
    }
  }

  private readonly Stack<NavigationEntry> navigationStack = new();
  
  [SerializeField]
  private GameObject menus;
  [SerializeField]
  private string defaultMenu = "MainMenu";

  private string currentMenu;

  void Awake() {
    currentMenu = defaultMenu;
  }

  public void OpenMenu(string menuName) {
    OpenMenu(menuName, true);
  }

  private void OpenMenu(string menuName, bool useHistory = true) {
    Transform targetMenu = menus.transform.Find(menuName);
    if (targetMenu == null) return;

    if (menuName == "MainMenu") 
      navigationStack.Clear();
    
    if (useHistory)
      navigationStack.Push(new(currentMenu, EventSystem.current.currentSelectedGameObject));

    DisableAllMenus();
    targetMenu.gameObject.SetActive(true);
    currentMenu = menuName;
  }

  public void GoBack() {
    if (navigationStack.Count <= 0) return;
    NavigationEntry entry = navigationStack.Pop();
    OpenMenu(entry.MenuName, false);
    EventSystem.current.SetSelectedGameObject(entry.FocusedObject);
  }

  
  public void Focus(GameObject gameObject) {
    EventSystem.current.SetSelectedGameObject(gameObject);
  }

  
  private void DisableAllMenus() {
    foreach (Transform child in menus.transform) {
      child.gameObject.SetActive(false);
    }
  }
}
