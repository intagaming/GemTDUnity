using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pathfinding;
using UnityEngine;

public class BaseEnemy : GridMobileEntity
{
  public const float CheckpointRadius = 0.2f;
  public const float InvisibleAfter = 1.5f;
  public const float TrueSightRange = 5f;


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
  private SpriteRenderer _spriteRenderer;

  private int _health;
  private bool _invisible = false;
  public bool Invisible { get => _invisible; }

  private float _aliveTime = 0f;

  protected override void Start()
  {
    _destinationSetter = GetComponent<AIDestinationSetter>();
    _destinationSetter.target = Checkpoint;

    _aiPath = GetComponent<AIPath>();
    _aiPath.maxSpeed = _scriptableEnemy.movementSpeed;

    _spriteRenderer = GetComponent<SpriteRenderer>();

    _health = _scriptableEnemy.hp;
  }

  protected override void Update()
  {
    _aliveTime += Time.deltaTime;

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

    if (_scriptableEnemy.invisible && _aliveTime >= InvisibleAfter)
    {
      if (IsUnderTrueSightAura())
      {
        if (_invisible)
        {
          _invisible = false;
          _spriteRenderer.enabled = true;
        }
      }
      else if (!_invisible)
      {
        _invisible = true;
        _spriteRenderer.enabled = false;
      }
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

  public bool IsUnderTrueSightAura()
  {
    return GridManager.Instance.GridTowers
      .Any(tower => tower.TowerBlueprint.BaseAuras.trueSight && Vector3.Distance(transform.position, tower.transform.position) < TrueSightRange);
  }
}
