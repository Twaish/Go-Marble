using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class NavigationManager : MonoBehaviour {
  [SerializeField]
  private GameObject menus;

  private readonly Stack<string> menuStack = new();
  private string currentMenuName = "MainMenu";

  [SerializeField]
  private Animator mainMenuAnimator;
  [SerializeField]
  private Animator levelSelectAnimator;
  [SerializeField]
  private Animator customizationMenuAnimator;


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

    UpdateAnimator(mainMenuAnimator, "MainMenu", menuName);
    UpdateAnimator(levelSelectAnimator, "LevelSelect", menuName);
    UpdateAnimator(customizationMenuAnimator, "CustomizationMenu", menuName);

    // DisableAllMenus();
    targetMenu.gameObject.SetActive(true);
    currentMenuName = menuName;
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