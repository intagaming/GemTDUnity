using UnityEngine;

public abstract class ProjectileCore
{
  public Projectile ProjectileGO;
  public ProjectileCore(Projectile projectile)
  {
    ProjectileGO = projectile;
  }

  protected BaseTower Attacker { get; set; }
  protected BaseEnemy Target { get; set; }

  public void Init(BaseTower attacker, BaseEnemy target)
  {
    Attacker = attacker;
    Target = target;
  }


  public virtual void HurtTargetAndTerminate()
  {
    Target.Damage(Attacker, Attacker.TowerBlueprint.BaseStats.damage);
    DefensePhaseManager.Instance.GetProjectilePool(ProjectileGO.Blueprint).Release(ProjectileGO);
  }

  public void Update()
  {
    if (Target == null || Target.gameObject == null)
    {
      DefensePhaseManager.Instance.GetProjectilePool(ProjectileGO.Blueprint).Release(ProjectileGO);
      return;
    }

    float step = ProjectileGO.Blueprint.BaseStats.speed * Time.deltaTime;
    ProjectileGO.transform.position = Vector2.MoveTowards(ProjectileGO.transform.position, Target.transform.position, step);
  }

  public void OnTriggerEnter2D(Collider2D collider)
  {
    if (Target == null || Target.gameObject == null)
    {
      DefensePhaseManager.Instance.GetProjectilePool(ProjectileGO.Blueprint).Release(ProjectileGO);
      return;
    }

    var baseEnemy = collider.GetComponent<BaseEnemy>();
    if (baseEnemy == null || baseEnemy != Target) return;

    HurtTargetAndTerminate();
  }
}
