
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class ProjectileDecorator : ProjectileCore
{
  private ProjectileCore _wrappee;

  protected ProjectileDecorator(Projectile projectile, ProjectileCore wrapee) : base(projectile)
  {
    _wrappee = wrapee;
  }

  public override void HurtTargetAndTerminate()
  {
    _wrappee.HurtTargetAndTerminate();
  }
}
