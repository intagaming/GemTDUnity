using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : Entity
{
  [SerializeField]
  protected ScriptableProjectile _blueprint;

  protected BaseTower Attacker { get; set; }
  protected BaseEnemy Target { get; set; }

  public void Init(BaseTower attacker, BaseEnemy target)
  {
    Attacker = attacker;
    Target = target;
  }

  protected override void Update()
  {
    if (Target == null || Target.gameObject == null)
    {
      DefensePhaseManager.Instance.GetProjectilePool(_blueprint).Release(this);
      return;
    }

    float step = _blueprint.BaseStats.speed * Time.deltaTime;
    transform.position = Vector2.MoveTowards(transform.position, Target.transform.position, step);
  }

  protected virtual void HurtTargetAndTerminate()
  {
    Target.Damage(Attacker, Attacker.TowerBlueprint.BaseStats.damage);
    DefensePhaseManager.Instance.GetProjectilePool(_blueprint).Release(this);
  }

  void OnTriggerEnter2D(Collider2D collider)
  {
    if (Target == null || Target.gameObject == null)
    {
      DefensePhaseManager.Instance.GetProjectilePool(_blueprint).Release(this);
      return;
    }

    var baseEnemy = collider.GetComponent<BaseEnemy>();
    if (baseEnemy == null || baseEnemy != Target) return;

    HurtTargetAndTerminate();
  }
}
