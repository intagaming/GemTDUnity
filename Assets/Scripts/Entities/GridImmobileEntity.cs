using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridImmobileEntity : Entity
{
  void OnMouseDown()
  {
    HUDManager.Instance.SelectGridEntity((int)transform.position.x, (int)transform.position.y);
  }
}
