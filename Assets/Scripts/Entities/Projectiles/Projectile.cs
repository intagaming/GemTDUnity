using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Projectile : Entity
{
  [SerializeField]
  protected ScriptableProjectile _blueprint;
  public ScriptableProjectile Blueprint { get => _blueprint; }

  public ProjectileCore core;

  protected override void Update()
  {
    core.Update();
  }

  public void OnTriggerEnter2D(Collider2D collider)
  {
    core.OnTriggerEnter2D(collider);
  }
}
