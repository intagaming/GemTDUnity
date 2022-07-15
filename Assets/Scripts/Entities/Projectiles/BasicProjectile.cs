using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicProjectile : Projectile
{
  protected override void Awake()
  {
    core = new BasicProjectileCore(this);
    base.Awake();
  }
}
