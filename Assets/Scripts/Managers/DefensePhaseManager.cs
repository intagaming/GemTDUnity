using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefensePhaseManager : MonoBehaviour
{
  [SerializeField]
  private ScriptableEnemy[] enemies;
  [SerializeField]
  private Transform enemiesParent;

  private Vector3 spawnPoint = new Vector3(4f, 32f, 0);
  const float SPAWN_INTERVAL = 1f;
  private int enemyLeft = 0;
  private float timer = 0f;

  private HashSet<BaseEnemy> waveEnemies = new HashSet<BaseEnemy>();

  private static DefensePhaseManager instance;
  public static DefensePhaseManager Instance { get { return instance; } }

  void Awake()
  {
    instance = this;
    GameManager.OnGameStateChanged += HandleOnGameStateChanged;
  }

  void OnDestroy()
  {
    GameManager.OnGameStateChanged -= HandleOnGameStateChanged;
  }

  private void Reset()
  {
    enemyLeft = 0;
    timer = 0f;
  }

  void HandleOnGameStateChanged(GameState state)
  {
    if (state == GameState.Defense)
    {
      enemyLeft = 10;
      timer = SPAWN_INTERVAL;
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
    if (enemyLeft <= 0) return;
    timer -= Time.deltaTime;
    if (timer > 0) return;

    SpawnEnemy();

    if (enemyLeft > 0)
    {
      timer = SPAWN_INTERVAL;
    }
    else
    {
      timer = 0;
    }
  }

  private void SpawnEnemy()
  {
    if (enemyLeft <= 0) return;
    enemyLeft--;
    var enemy = Instantiate(enemies[GameManager.Instance.Wave - 1].enemyPrefab, spawnPoint, Quaternion.identity, enemiesParent);
    waveEnemies.Add(enemy);
  }

  public void HandleEnemyDie(BaseEnemy enemy)
  {
    if (!waveEnemies.Contains(enemy))
    {
      Debug.LogError("Dead enemy not found in waveEnemies.");
      return;
    }

    waveEnemies.Remove(enemy);

    if (waveEnemies.Count == 0)
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
