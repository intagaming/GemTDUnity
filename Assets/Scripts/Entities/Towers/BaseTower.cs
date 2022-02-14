using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseTower : GridImmobileEntity
{
  // This field is programmatically assigned if it's a ScriptableGemTower.
  [SerializeField]
  protected ScriptableTower TowerBlueprint;

  private float _cooldown = 0f;
  private BaseEnemy _target = null;

  protected override void Update()
  {
    base.Update();

    if (GameManager.Instance.State != GameState.Defense) return;

    _cooldown = Mathf.Max(0, _cooldown - Time.deltaTime);
    if (_cooldown <= 0f) AttemptAttack();
  }

  protected virtual void AttemptAttack()
  {
    if (_cooldown > 0f) return;

    var stats = TowerBlueprint.BaseStats;
    var enemies = DefensePhaseManager.Instance.WaveEnemies;

    // Check if target is dead or out of reach
    if (_target != null && (
      _target.gameObject == null ||
      !enemies.Contains(_target) ||
      Vector3.Distance(transform.position, _target.transform.position) > stats.range)
      )
    {
      _target = null;
    }

    if (_target != null)
    {
      Attack(_target);
      return;
    }

    // Find new target
    foreach (var enemy in enemies)
    {
      var distance = Vector3.Distance(enemy.transform.position, transform.position);
      if (distance <= stats.range)
      {
        _target = enemy;
        Attack(enemy);
        return;
      }
    }
  }

  protected virtual void Attack(BaseEnemy enemy)
  {
    if (_target != enemy) _target = enemy;

    var stats = TowerBlueprint.BaseStats;
    _cooldown = stats.attackSpeed;
    enemy.Damage(this, stats.damage);
  }

  public void SetTowerBlueprint(ScriptableTower blueprint)
  {
    TowerBlueprint = blueprint;
  }


  void OnMouseDown()
  {
    HUDManager.Instance.SelectTower((int)transform.position.x, (int)transform.position.y);
  }
}
