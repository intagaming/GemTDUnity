using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.SceneManagement;

public class DefensePhaseManager : MonoBehaviour
{
  public const float SPAWN_INTERVAL = 1f;

  [SerializeField]
  private ScriptableEnemy[] _enemies;
  public ScriptableEnemy[] Enemies { get => _enemies; }
  public int WaveCount => Enemies.Length;
  [SerializeField]
  private Transform _enemiesParent;
  [SerializeField]
  private Transform _projectilesParent;

  private Vector3 _spawnPoint = new Vector3(4f, 32f, 0);
  private int _enemyLeftToSpawn = 0;
  public int EnemiesLeftToSpawn { get => _enemyLeftToSpawn; }
  private float _timer = 0f;

  private HashSet<BaseEnemy> _waveEnemies = new HashSet<BaseEnemy>();
  public HashSet<BaseEnemy> WaveEnemies { get => _waveEnemies; }

  private static bool _isExiting = false;

  public int EnemiesRemaining { get => _enemyLeftToSpawn + _waveEnemies.Count; }

  public static event Action<BaseEnemy> OnEnemyDie;

  private Dictionary<ScriptableProjectile, ObjectPool<Projectile>> _projectilePools = new Dictionary<ScriptableProjectile, ObjectPool<Projectile>>();
  public ObjectPool<Projectile> GetProjectilePool(ScriptableProjectile blueprint)
  {
    if (!_projectilePools.ContainsKey(blueprint))
    {
      _projectilePools.Add(blueprint, new ObjectPool<Projectile>(() =>
      {
        return Instantiate(blueprint.prefab, Vector3.zero, Quaternion.identity, _projectilesParent);
      }, prj =>
      {
        prj.gameObject.SetActive(true);
      }, prj =>
      {
        prj.gameObject.SetActive(false);
      }, prj =>
      {
        Destroy(prj.gameObject);
      }, false, 20, 10000));
    }
    return _projectilePools[blueprint];
  }

  private static DefensePhaseManager _instance;
  public static DefensePhaseManager Instance { get { return _instance; } }

  void Awake()
  {
    _instance = this;
    GameManager.OnGameStateChanged += HandleOnGameStateChanged;
#if UNITY_EDITOR
    EditorApplication.playModeStateChanged += HandlePlayModeStateChanged;
#endif
    _isExiting = false;
    GameManager.OnHealthChanged += HandleHealthChanged;
  }

  void OnDestroy()
  {
    GameManager.OnGameStateChanged -= HandleOnGameStateChanged;
#if UNITY_EDITOR
    EditorApplication.playModeStateChanged -= HandlePlayModeStateChanged;
#endif
    _waveEnemies.Clear();
    GameManager.OnHealthChanged -= HandleHealthChanged;
  }

  private void Reset()
  {
    _enemyLeftToSpawn = 0;
    _timer = 0f;
  }

  void HandleOnGameStateChanged(GameState state)
  {
    if (state == GameState.Defense)
    {
      _enemyLeftToSpawn = _enemies[GameManager.Instance.Wave - 1].waveAmount;
      _timer = SPAWN_INTERVAL;

      // Rescan the path
      AstarPath.active.Scan();
      return;
    }

    if (state == GameState.Building)
    {
      Reset();
      return;
    }
  }

  void Update()
  {
    if (_enemyLeftToSpawn <= 0) return;
    _timer -= Time.deltaTime;
    if (_timer > 0) return;

    SpawnEnemy();

    if (_enemyLeftToSpawn > 0)
    {
      _timer = SPAWN_INTERVAL;
    }
    else
    {
      _timer = 0;
    }
  }

  private void SpawnEnemy()
  {
    if (_enemyLeftToSpawn <= 0) return;
    _enemyLeftToSpawn--;
    var enemy = Instantiate(_enemies[GameManager.Instance.Wave - 1].enemyPrefab, _spawnPoint, Quaternion.identity, _enemiesParent);
    _waveEnemies.Add(enemy);
  }

  public void HandleEnemyDie(BaseEnemy enemy)
  {
    if (_isExiting) return; // If exiting, ignore all enemy die events, thus the wave will not end.

    if (GameManager.Instance.State == GameState.GameOver)
    {
      return;
    }

    if (!_waveEnemies.Contains(enemy))
    {
      Debug.LogError("Dead enemy not found in waveEnemies.");
      return;
    }

    _waveEnemies.Remove(enemy);

    OnEnemyDie?.Invoke(enemy);

    if (_waveEnemies.Count == 0)
    {
      GameManager.Instance.SetState(GameState.Building);
    }
  }

  public void HandleEnemyReachTheEnd(BaseEnemy enemy)
  {
    GameManager.Instance.DamageCastle(1); // TODO: castle damage in ScriptableEnemy
    Destroy(enemy.gameObject);
    FindObjectOfType<SoundManager>().Play("ReachTheEnd");
    OnEnemyDie?.Invoke(enemy);
  }

#if UNITY_EDITOR
  private static void HandlePlayModeStateChanged(PlayModeStateChange state)
  {
    if (state == PlayModeStateChange.ExitingPlayMode)
    {
      _isExiting = true;
    }
  }
#endif

  public void SpawnProjectile(BaseTower tower, BaseEnemy enemy)
  {
    var pool = GetProjectilePool(tower.TowerBlueprint.projectile);

    var projectile = pool.Get();
    projectile.core.Init(tower, enemy);

    // wrap core with decorators
    foreach (var d in tower.TowerBlueprint.projectile.decorators)
    {
      var newCore = projectile.core;
      switch (d.id)
      {
        case "slow":
          newCore = new SlowProjectileDecorator(projectile, newCore);
          break;
        case "spread":
          newCore = new SpreadProjectileDecorator(projectile, newCore);
          break;
      }
      newCore.Init(tower, enemy);
      projectile.core = newCore;
    }

    projectile.transform.position = tower.transform.position;
  }

  private void HandleHealthChanged(int health)
  {
    if (health == 0)
    {
      GameManager.Instance.SetState(GameState.GameOver);
      SceneManager.LoadScene("Game Over");
    }
  }
}
