using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NavigationManager : MonoBehaviour {
  [SerializeField]
  private GameObject menus;

  private readonly Stack<string> menuStack = new();
  private string currentMenuName = "MainMenu";

  public void OpenMenu(string menuName) {
    OpenMenu(menuName, true);
  }

  private void OpenMenu(string menuName, bool addToStack = true) {
    Transform targetMenu = menus.transform.Find(menuName);
    if (targetMenu == null) return;

    if (menuName == "MainMenu") 
      menuStack.Clear();
    
    if (addToStack)
      menuStack.Push(currentMenuName);

    DisableAllMenus();
    targetMenu.gameObject.SetActive(true);
    currentMenuName = menuName;
  }

  public void OpenScene(string sceneName) {
    Scene targetScene = SceneManager.GetSceneByName(sceneName);
    if (targetScene.isLoaded) return;

    SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
  }
  
  public void GoBack() {
    if (menuStack.Count <= 0) return;
    OpenMenu(menuStack.Pop(), false);
  }

  private void DisableAllMenus() {
    foreach (Transform child in menus.transform) {
      child.gameObject.SetActive(false);
    }
  }

  public void UnloadScene(string sceneName) {
    if (SceneManager.GetSceneByName(sceneName).isLoaded) {
      SceneManager.UnloadSceneAsync(sceneName);
    }
  }
}