using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemTower : BaseTower
{
  protected ScriptableGemTower GemTowerBlueprint
  {
    get { return (ScriptableGemTower)_towerBlueprint; }
  }

  public int Level { get => GemTowerBlueprint.gemLevel; }

  protected override void Start()
  {
    base.Start();

    if (Level < 1 || Level > 6)
    {
      Debug.LogWarning("Invalid gem tower level");
    }
  }
}
