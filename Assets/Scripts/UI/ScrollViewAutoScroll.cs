using System;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.Burst.Intrinsics;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;

// https://youtu.be/l2_rHUffkJw
public class ScrollViewAutoScroll : MonoBehaviour {
  [SerializeField]
  private RectTransform viewportRectTransform;
  [SerializeField]
  private RectTransform content;
  [SerializeField]
  private float transitionDuration = 0.2f;

  private ScrollRect scrollRect;

  void Awake() {
    scrollRect = GetComponent<ScrollRect>();
  }

  public void HandleOnSelectChange(GameObject GO) {
    if (GetVerticalLayoutGroup(out VerticalLayoutGroup verticalLayoutGroup)) {
      float viewportTopBorderY = GetBorderTopYLocal(viewportRectTransform.gameObject);
      float viewportBottomBorderY = GetBorderBottomYLocal(viewportRectTransform.gameObject);
      
      // Top
      float targetTopYWithViewportOffset = GetBorderTopYRelative(GO) + viewportTopBorderY;
      float topDifference = targetTopYWithViewportOffset - viewportTopBorderY;
      if (topDifference > 0) {
        Debug.Log("Moving TOP by " + topDifference);
        MoveContentObjectYByAmount(topDifference + verticalLayoutGroup.padding.top);
      }

      // Bottom
      float targetBottomYWithViewportOffset = GetBorderBottomYRelative(GO) + viewportTopBorderY;
      float bottomDifference = targetBottomYWithViewportOffset - viewportBottomBorderY;
      if (bottomDifference < 0f) {
        Debug.Log("Moving BOTTOM by " + bottomDifference);
        MoveContentObjectYByAmount(bottomDifference - verticalLayoutGroup.padding.bottom);
      }
    }

    if (GetHorizontalLayoutGroup(out HorizontalLayoutGroup horizontalLayoutGroup)) {
      float viewportLeftBorderX = GetBorderLeftXLocal(viewportRectTransform.gameObject);
      float viewportRightBorderX = GetBorderRightXLocal(viewportRectTransform.gameObject);
      
      // Left
      float targetLeftXWithViewportOffset = GetBorderLeftXRelative(GO) + viewportLeftBorderX;
      float leftDifference = targetLeftXWithViewportOffset - viewportLeftBorderX;
      if (leftDifference > 0 ) {
        Debug.Log("Moving LEFT by " + leftDifference);
        MoveContentObjectXByAmount(leftDifference + horizontalLayoutGroup.padding.left);
      }

      // Right 
      float targetRightXWithViewportOffset = GetBorderRightXRelative(GO) - viewportLeftBorderX;
      float rightDifference = targetRightXWithViewportOffset - viewportRightBorderX;
      if (rightDifference < 0f) {
        Debug.Log("Moving RIGHT by " + rightDifference);
        MoveContentObjectXByAmount(rightDifference - horizontalLayoutGroup.padding.right);
      }
    }
  }

  private float GetBorderTopYLocal(GameObject GO) {
    return GO.transform.localPosition.y;
  }
  private float GetBorderBottomYLocal(GameObject GO) {
    Vector2 rectSize = GO.GetComponent<RectTransform>().rect.size;
    Vector3 position = GO.transform.localPosition;
    position.y -= rectSize.y;
    return position.y;
  }


  /*
  
  
  
  FIIXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
  
  
  
  */









  private float GetBorderLeftXLocal(GameObject GO) {
    return GO.transform.localPosition.x;
  }
  private float GetBorderRightXLocal(GameObject GO) {
    Vector2 rectSize = GO.GetComponent<RectTransform>().rect.size;
    Vector3 position = GO.transform.localPosition;
    position.x -= rectSize.x;
    return position.x;
  }
  private float GetBorderTopYRelative(GameObject GO) {
    return content.transform.localPosition.y - GetBorderTopYLocal(GO);
  }
  private float GetBorderBottomYRelative(GameObject GO) {
    return content.transform.localPosition.y + GetBorderBottomYLocal(GO);
  }
  private float GetBorderLeftXRelative(GameObject GO) {
    return content.transform.localPosition.x + GetBorderLeftXLocal(GO);
  }
  private float GetBorderRightXRelative(GameObject GO) {
    return content.transform.localPosition.x - GetBorderRightXLocal(GO);
  }
  
  private void MoveContentObjectYByAmount(float amount) {
    Vector2 positionScrollFrom = content.transform.localPosition;
    Vector2 positionScrollTo = positionScrollFrom;
    positionScrollTo.y -= amount;

    content.transform.localPosition = positionScrollTo;
  }

  private void MoveContentObjectXByAmount(float amount) {
    Vector2 positionScrollFrom = content.transform.localPosition;
    Vector2 positionScrollTo = positionScrollFrom;
    positionScrollTo.x -= amount;

    content.transform.localPosition = positionScrollTo;
  }

  private bool GetVerticalLayoutGroup(out VerticalLayoutGroup layoutGroup) {
    layoutGroup = content.GetComponent<VerticalLayoutGroup>();
    return layoutGroup ;
  }

  private bool GetHorizontalLayoutGroup(out HorizontalLayoutGroup layoutGroup) {
    layoutGroup = content.GetComponent<HorizontalLayoutGroup>();
    return layoutGroup;
  }
}
