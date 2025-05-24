using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

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
      bool animatorFound = menu.TryGetComponent(out Animator menuAnimator);
      if (animatorFound) {
        UpdateAnimator(menuAnimator, menu.name, newMenuName);
      } else {
        menu.SetActive(false);
      }
    }

    // DisableAllMenus();
    targetMenu.gameObject.SetActive(true);
    currentMenuName = newMenuName;
    
    BlockNavigation(0.5f);
  }

  public void BlockNavigation(float duration) {
    StartCoroutine(TemporarilyDisableNavigation(duration));
  }

  private IEnumerator TemporarilyDisableNavigation(float duration) {
    if (EventSystem.current == null || EventSystem.current.currentInputModule == null)
      yield break;

    EventSystem.current.sendNavigationEvents = false;

    yield return new WaitForSeconds(duration);

    EventSystem.current.sendNavigationEvents = true;
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