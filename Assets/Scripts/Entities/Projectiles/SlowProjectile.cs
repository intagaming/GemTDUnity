using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowProjectile : Projectile
{
  protected override void HurtTargetAndTerminate()
  {
    Target.DamageMovement(1f);
    base.HurtTargetAndTerminate();
  }
}
