using System.Linq;
using UnityEngine;

public class SpreadProjectileDecorator : ProjectileDecorator
{
  public SpreadProjectileDecorator(Projectile projectile, ProjectileCore wrapee) : base(projectile, wrapee)
  {
  }

  public override void HurtTargetAndTerminate()
  {
    base.HurtTargetAndTerminate();
    var spreadingTo = DefensePhaseManager.Instance.WaveEnemies.Where(e => Vector3.Distance(e.transform.position, ProjectileGO.transform.position) < 100).ToList();
    foreach (var enemy in spreadingTo)
    {
      enemy.Damage(Attacker, Attacker.TowerBlueprint.BaseStats.damage);
    }
  }
}
