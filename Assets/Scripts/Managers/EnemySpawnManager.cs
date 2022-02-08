using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
  [SerializeField]
  private ScriptableEnemy[] enemies;

  private Vector3 spawnPoint = new Vector3(4f, 32f, 0);
  const float SpawnInterval = 1f;
  private int enemyLeft = 0;
  private float timer = 0f;

  void Awake()
  {
    GameManager.OnGameStateChanged += HandleOnGameStateChanged;
  }

  void OnDestroy()
  {
    GameManager.OnGameStateChanged -= HandleOnGameStateChanged;
  }

  void HandleOnGameStateChanged(GameState state)
  {
    if (state == GameState.Defense)
    {
      enemyLeft = 10;
      timer = SpawnInterval;
    }
  }

  void Update()
  {
    if (enemyLeft <= 0) return;
    timer -= Time.deltaTime;
    if (timer > 0) return;

    enemyLeft--;
    Instantiate(enemies[GameManager.Instance.Wave - 1].enemyPrefab, spawnPoint, Quaternion.identity);

    if (enemyLeft > 0)
    {
      timer = SpawnInterval;
    }
    else
    {
      timer = 0;
    }
  }
}
