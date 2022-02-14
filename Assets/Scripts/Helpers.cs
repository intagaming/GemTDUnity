using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Helpers
{
  public static Vector2Int GetGridPosition(this GridImmobileEntity immobileEntity)
  {
    return new Vector2Int((int)immobileEntity.transform.position.x, (int)immobileEntity.transform.position.y);
  }
}
