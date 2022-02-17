using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ScriptableTower : ScriptableObject
{
  [SerializeField]
  private TowerStats stats;
  public TowerStats BaseStats => stats;
  [SerializeField]
  private TowerAuras auras;
  public TowerAuras BaseAuras => auras;
  public ScriptableProjectile projectile;
  public Sprite sprite;
}

[Serializable]
public struct TowerStats
{
  public int damage;
  public float range;
  public float attackSpeed;
}

[Serializable]
public struct TowerAuras
{
  public bool trueSight;
}