using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

[System.Serializable]
public class MenuGroup {
  public string groupName;
  public Transform container;
  public List<GameObject> menus;

  [HideInInspector] public Stack<string> menuStack = new();
  [HideInInspector] public string currentMenuName = string.Empty;
}

public class NavigationManager : MonoBehaviour {
  [SerializeField] private List<MenuGroup> menuGroups = new();

  public void OpenMenu(string fullMenuPath) {
    // GroupName/MenuName
    var split = fullMenuPath.Split('/');
    if (split.Length != 2) {
      Debug.LogWarning($"NavigationManager: Invalid menu path '{fullMenuPath}'");
      return;
    }

    string groupName = split[0];
    string menuName = split[1];

    MenuGroup group = menuGroups.FirstOrDefault(g => g.groupName == groupName);
    if (group == null) {
      Debug.LogWarning($"NavigationManager: Menu group '{groupName}' not found");
      return;
    }

    OpenMenuInGroup(group, menuName);
  }

  private void OpenMenuInGroup(MenuGroup group, string newMenuName, bool addToStack = true) {
    Transform targetMenu = group.container.Find(newMenuName);

    Debug.Log("OPENING " + group.groupName + "/" + newMenuName);

    if (newMenuName == "MainMenu")
      group.menuStack.Clear();

    if (addToStack && !string.IsNullOrEmpty(group.currentMenuName))
      group.menuStack.Push(group.currentMenuName);

    foreach (GameObject menu in group.menus) {
      if (!menu.activeSelf) continue;
      if (menu.TryGetComponent(out Animator animator)) {
        animator.SetBool("active", menu.name == newMenuName);
      }
      else {
        menu.SetActive(menu.name == newMenuName);
      }
    }

    if (targetMenu == null) {
      group.currentMenuName = string.Empty;
      return;
    }

    group.currentMenuName = newMenuName;
    targetMenu.gameObject.SetActive(true);
    BlockNavigation(0.5f);
  }

  public void BlockNavigation(float duration) {
    StartCoroutine(TemporarilyDisableNavigation(duration));
  }

  private IEnumerator TemporarilyDisableNavigation(float duration) {
    if (EventSystem.current == null || EventSystem.current.currentInputModule == null)
      yield break;

    EventSystem.current.sendNavigationEvents = false;

    yield return new WaitForSecondsRealtime(duration);

    EventSystem.current.sendNavigationEvents = true;
  }

  public void Focus(GameObject gameObject) {
    EventSystem.current.SetSelectedGameObject(gameObject);
  }

  public void GoBack(string groupName) {
    MenuGroup group = menuGroups.FirstOrDefault(g => g.groupName == groupName);
    if (group == null || group.menuStack.Count == 0) return;

    string previousMenu = group.menuStack.Pop();
    OpenMenuInGroup(group, previousMenu, false);
  }

  public void OpenScene(string sceneName) {
    Scene targetScene = SceneManager.GetSceneByName(sceneName);
    if (targetScene.isLoaded) return;

    SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
  }

  public void UnloadScene(string sceneName) {
    if (SceneManager.GetSceneByName(sceneName).isLoaded) {
      SceneManager.UnloadSceneAsync(sceneName);
    }
  }
}