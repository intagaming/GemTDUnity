using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GridImmobileEntity : Entity, IPointerClickHandler
{
  public void OnPointerClick(PointerEventData eventData)
  {
    HUDManager.Instance.SelectGridPosition((int)transform.position.x, (int)transform.position.y);
  }
}
