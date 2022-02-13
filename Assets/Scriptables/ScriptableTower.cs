using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ScriptableTower : ScriptableObject
{
  [SerializeField]
  private TowerStats stats;
  public TowerStats BaseStats => stats;
}

[Serializable]
public struct TowerStats
{
  public int damage;
  public float range;
  public float attackSpeed;
}