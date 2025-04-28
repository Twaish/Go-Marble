using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

[Serializable]
public class MenuAnimatorPair {
  public string menuName;
  public Animator animator;
}


public class NavigationManager : MonoBehaviour {
  [SerializeField]
  private GameObject menusContainer;

  private readonly Stack<string> menuStack = new();
  private string currentMenuName = "MainMenu";

  [SerializeField]
  private List<GameObject> menus = new();

  public void OpenMenu(string menuName) {
    OpenMenu(menuName, true);
  }

  private void OpenMenu(string newMenuName, bool addToStack = true) {
    Transform targetMenu = menusContainer.transform.Find(newMenuName);
    if (targetMenu == null) return;

    if (newMenuName == "MainMenu") 
      menuStack.Clear();
    
    if (addToStack)
      menuStack.Push(currentMenuName);

    foreach (GameObject menu in menus) {
      Animator menuAnimator = menu.GetComponent<Animator>();
      if (!menuAnimator) continue;
      UpdateAnimator(menuAnimator, menu.name, newMenuName);
    }

    // DisableAllMenus();
    targetMenu.gameObject.SetActive(true);
    currentMenuName = newMenuName;
  }

  private void UpdateAnimator(Animator animator, string menuName, string newMenuName) {
    bool shouldOpen = newMenuName == menuName;
    animator.SetBool("active", shouldOpen);
  }

  public void OpenScene(string sceneName) {
    Scene targetScene = SceneManager.GetSceneByName(sceneName);
    if (targetScene.isLoaded) return;

    SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
  }

  public void Focus(GameObject gameObject) {
    EventSystem.current.SetSelectedGameObject(gameObject);
  }
  
  public void GoBack() {
    if (menuStack.Count <= 0) return;
    OpenMenu(menuStack.Pop(), false);
  }

  public void UnloadScene(string sceneName) {
    if (SceneManager.GetSceneByName(sceneName).isLoaded) {
      SceneManager.UnloadSceneAsync(sceneName);
    }
  }
}