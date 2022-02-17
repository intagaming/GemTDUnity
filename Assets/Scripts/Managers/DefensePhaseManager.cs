using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DefensePhaseManager : MonoBehaviour
{
  const float SPAWN_INTERVAL = 1f;

  [SerializeField]
  private ScriptableEnemy[] _enemies;
  [SerializeField]
  private Transform _enemiesParent;

  private Vector3 _spawnPoint = new Vector3(4f, 32f, 0);
  private int _enemyLeft = 0;
  private float _timer = 0f;

  private HashSet<BaseEnemy> _waveEnemies = new HashSet<BaseEnemy>();
  public HashSet<BaseEnemy> WaveEnemies { get => _waveEnemies; }

  private static bool _isExiting = false;

  private static DefensePhaseManager _instance;
  public static DefensePhaseManager Instance { get { return _instance; } }

  void Awake()
  {
    _instance = this;
    GameManager.OnGameStateChanged += HandleOnGameStateChanged;
    EditorApplication.playModeStateChanged += HandlePlayModeStateChanged;
    _isExiting = false;
  }

  void OnDestroy()
  {
    GameManager.OnGameStateChanged -= HandleOnGameStateChanged;
    EditorApplication.playModeStateChanged -= HandlePlayModeStateChanged;
    _waveEnemies.Clear();
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
    var enemy = Instantiate(_enemies[GameManager.Instance.Wave - 1].enemyPrefab, _spawnPoint, Quaternion.identity, _enemiesParent);
    _waveEnemies.Add(enemy);
  }

  public void HandleEnemyDie(BaseEnemy enemy)
  {
    if (_isExiting) return; // If exiting, ignore all enemy die events, thus the wave will not end.

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

  private static void HandlePlayModeStateChanged(PlayModeStateChange state)
  {
    if (state == PlayModeStateChange.ExitingPlayMode)
    {
      _isExiting = true;
    }
  }
}
