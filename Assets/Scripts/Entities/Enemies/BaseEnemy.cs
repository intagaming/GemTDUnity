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

  private int currentCheckpointIndex = 0;
  public Transform Checkpoint
  {
    get { return GameManager.Instance.Checkpoints[currentCheckpointIndex]; }
  }

  private AIDestinationSetter destinationSetter;
  private AIPath aiPath;

  protected override void Start()
  {
    destinationSetter = GetComponent<AIDestinationSetter>();
    destinationSetter.target = Checkpoint;

    aiPath = GetComponent<AIPath>();
    aiPath.maxSpeed = scriptableEnemy.movementSpeed;
  }

  protected override void Update()
  {
    if (Vector2.Distance(Checkpoint.position, transform.position) <= CheckpointRadius)
    {
      if (currentCheckpointIndex >= GameManager.Instance.Checkpoints.Length - 1)
      {
        Debug.Log("Reached the end");
        return;
      }
      currentCheckpointIndex++;
      destinationSetter.target = Checkpoint;
    }
  }

  void OnDestroy()
  {
    DefensePhaseManager.Instance.HandleEnemyDie(this);
  }
}
