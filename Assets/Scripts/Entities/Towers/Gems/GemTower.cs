using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemTower : BaseTower
{
  // The level needs to be initialized on creation.
  private int level = 0;

  public int Level { get => level; set => level = Math.Max(1, Math.Min(6, value)); }

  protected override void Awake()
  {
    base.Awake();

    if (Level == 0)
    {
      Debug.LogWarning("Invalid gem tower level");
    }
  }
}
