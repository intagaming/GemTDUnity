using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class BaseEnemy : GridMobileEntity
{
  const float CheckpointRadius = 0.2f;
  [SerializeField]
  private ScriptableEnemy _scriptableEnemy;
  public ScriptableEnemy ScriptableEnemy
  {
    get { return _scriptableEnemy; }
  }

  private int _currentCheckpointIndex = 0;
  public Transform Checkpoint
  {
    get { return GameManager.Instance.Checkpoints[_currentCheckpointIndex]; }
  }

  private AIDestinationSetter _destinationSetter;
  private AIPath _aiPath;

  private int _health;

  protected override void Start()
  {
    _destinationSetter = GetComponent<AIDestinationSetter>();
    _destinationSetter.target = Checkpoint;

    _aiPath = GetComponent<AIPath>();
    _aiPath.maxSpeed = _scriptableEnemy.movementSpeed;

    _health = _scriptableEnemy.hp;
  }

  protected override void Update()
  {
    if (Vector2.Distance(Checkpoint.position, transform.position) <= CheckpointRadius)
    {
      if (_currentCheckpointIndex >= GameManager.Instance.Checkpoints.Length - 1)
      {
        DefensePhaseManager.Instance.HandleEnemyReachTheEnd(this);
        return;
      }
      _currentCheckpointIndex++;
      _destinationSetter.target = Checkpoint;
    }
  }

  void OnDestroy()
  {
    DefensePhaseManager.Instance.HandleEnemyDie(this);
  }

  public virtual void Damage(BaseTower attacker, int damage)
  {
    _health -= damage;
    if (_health <= 0)
    {
      Destroy(gameObject);
    }
  }
}
