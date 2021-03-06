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
  public int Health { get => _health;  }
  private bool _invisible = false;
  public bool Invisible { get => _invisible; }

  private float _aliveTime = 0f;
  
  private float _slowTime = 0f;
  private bool _isFinished = false;

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
    if (GameManager.Instance.State == GameState.GameOver) return;
    if(_aliveTime > _slowTime)
    {
      _aiPath.maxSpeed = _scriptableEnemy.movementSpeed;
    }
    _aliveTime += Time.deltaTime;

    EnemyHealthBarManager.Instance.UpdateHealthBar(this, _isFinished);

    if (Vector2.Distance(Checkpoint.position, transform.position) <= CheckpointRadius)
    {
      if (_currentCheckpointIndex >= GameManager.Instance.Checkpoints.Length - 1)
      {
        _isFinished = true;
        DefensePhaseManager.Instance.HandleEnemyReachTheEnd(this);
        EnemyHealthBarManager.Instance.UpdateHealthBar(this, _isFinished);
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
    if (GameManager.Instance.State == GameState.GameOver) return;
    DefensePhaseManager.Instance.HandleEnemyDie(this);
    EnemyHealthBarManager.Instance.UpdateHealthBar(this, _isFinished);
  }

  public virtual void Damage(BaseTower attacker, int damage)
  {
    _health -= damage;
    
    if (_health <= 0)
    {
      Destroy(gameObject);
    }
  }

  public virtual void DamageMovement(float speed)
  {
    _slowTime = _aliveTime + 1f;
    _aiPath.maxSpeed = speed;
  }

  public bool IsUnderTrueSightAura()
  {
    return GridManager.Instance.GridTowers
      .Any(tower => tower.TowerBlueprint.BaseAuras.trueSight && Vector3.Distance(transform.position, tower.transform.position) < TrueSightRange);
  }
}
