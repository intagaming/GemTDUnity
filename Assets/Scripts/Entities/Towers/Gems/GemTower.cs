using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemTower : BaseTower
{
  protected ScriptableGemTower GemTowerBlueprint
  {
    get { return (ScriptableGemTower)TowerBlueprint; }
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

  void OnMouseDown()
  {
    if (GameManager.Instance.State != GameState.Building) return;
    BuildPhaseManager.Instance.ChooseGem((int)transform.position.x, (int)transform.position.y);
  }
}
