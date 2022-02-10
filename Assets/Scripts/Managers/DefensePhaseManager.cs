using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefensePhaseManager : MonoBehaviour
{
  const float SPAWN_INTERVAL = 1f;

  [SerializeField]
  private ScriptableEnemy[] enemies;
  [SerializeField]
  private Transform enemiesParent;

  private Vector3 _spawnPoint = new Vector3(4f, 32f, 0);
  private int _enemyLeft = 0;
  private float _timer = 0f;

  private HashSet<BaseEnemy> _waveEnemies = new HashSet<BaseEnemy>();

  private static DefensePhaseManager _instance;
  public static DefensePhaseManager Instance { get { return _instance; } }

  void Awake()
  {
    _instance = this;
    GameManager.OnGameStateChanged += HandleOnGameStateChanged;
  }

  void OnDestroy()
  {
    GameManager.OnGameStateChanged -= HandleOnGameStateChanged;
  }

  private void Reset()
  {
    _enemyLeft = 0;
    _timer = 0f;
  }

  void HandleOnGameStateChanged(GameState state)
  {
    if (state == GameState.Defense)
    {
      _enemyLeft = 10;
      _timer = SPAWN_INTERVAL;
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
    if (_enemyLeft <= 0) return;
    _timer -= Time.deltaTime;
    if (_timer > 0) return;

    SpawnEnemy();

    if (_enemyLeft > 0)
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
    if (_enemyLeft <= 0) return;
    _enemyLeft--;
    var enemy = Instantiate(enemies[GameManager.Instance.Wave - 1].enemyPrefab, _spawnPoint, Quaternion.identity, enemiesParent);
    _waveEnemies.Add(enemy);
  }

  public void HandleEnemyDie(BaseEnemy enemy)
  {
    if (!_waveEnemies.Contains(enemy))
    {
      Debug.LogError("Dead enemy not found in waveEnemies.");
      return;
    }

    _waveEnemies.Remove(enemy);

    if (_waveEnemies.Count == 0)
    {
      GameManager.Instance.SetState(GameState.Building);
    }
  }

  public void HandleEnemyReachTheEnd(BaseEnemy enemy)
  {
    // TODO: subtract health
    Destroy(enemy.gameObject);
  }
}
