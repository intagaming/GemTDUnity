
public class SlowProjectileDecorator : ProjectileDecorator
{
  public SlowProjectileDecorator(Projectile projectile, ProjectileCore wrapee) : base(projectile, wrapee)
  {
  }

  public override void HurtTargetAndTerminate()
  {
    Target.DamageMovement(1f);
    base.HurtTargetAndTerminate();
  }
}
