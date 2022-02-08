using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseTower : GridImmobileEntity
{
  // This field is programmatically assigned if it's a ScriptableGemTower.
  [SerializeField]
  protected ScriptableTower TowerBlueprint;

  public void SetTowerBlueprint(ScriptableTower blueprint)
  {
    TowerBlueprint = blueprint;
  }
}
