using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Projectile", menuName = "Scriptable/Projectile")]
public class ScriptableProjectile : ScriptableObject
{
  public Projectile prefab;
  [SerializeField]
  private ProjectileStats _stats;
  public ProjectileStats BaseStats { get => _stats; }

  public ScriptableProjectileDecorator[] decorators;
}

[Serializable]
public struct ProjectileStats
{
  public float speed;
}
