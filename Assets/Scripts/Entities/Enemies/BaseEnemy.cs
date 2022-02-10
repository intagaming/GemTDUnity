using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class BaseEnemy : GridMobileEntity
{
  const float CheckpointRadius = 0.2f;
  [SerializeField]
  private ScriptableEnemy scriptableEnemy;
  public ScriptableEnemy ScriptableEnemy
  {
    get { return scriptableEnemy; }
  }

  private int _currentCheckpointIndex = 0;
  public Transform Checkpoint
  {
    get { return GameManager.Instance.Checkpoints[_currentCheckpointIndex]; }
  }

  private AIDestinationSetter _destinationSetter;
  private AIPath _aiPath;

  protected override void Start()
  {
    _destinationSetter = GetComponent<AIDestinationSetter>();
    _destinationSetter.target = Checkpoint;

    _aiPath = GetComponent<AIPath>();
    _aiPath.maxSpeed = scriptableEnemy.movementSpeed;
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
}
