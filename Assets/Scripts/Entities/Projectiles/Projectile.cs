using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : Entity
{
  [SerializeField]
  protected ScriptableProjectile _blueprint;

  private BaseTower Attacker { get; set; }
  private BaseEnemy Target { get; set; }

  public void Init(BaseTower attacker, BaseEnemy target)
  {
    Attacker = attacker;
    Target = target;
  }

  protected override void Update()
  {
    if (Target == null || Target.gameObject == null)
    {
      Destroy(gameObject);
      return;
    }

    float step = _blueprint.BaseStats.speed * Time.deltaTime;
    transform.position = Vector2.MoveTowards(transform.position, Target.transform.position, step);
  }

  private void HurtTargetAndTerminate()
  {
    Target.Damage(Attacker, Attacker.TowerBlueprint.BaseStats.damage);
    Destroy(gameObject);
  }

  void OnTriggerEnter2D(Collider2D collider)
  {
    if (Target == null || Target.gameObject == null)
    {
      Destroy(gameObject);
      return;
    }

    var baseEnemy = collider.GetComponent<BaseEnemy>();
    if (baseEnemy == null || baseEnemy != Target) return;

    HurtTargetAndTerminate();
  }
}
