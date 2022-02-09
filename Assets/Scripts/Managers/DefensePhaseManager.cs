using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefensePhaseManager : MonoBehaviour
{
  [SerializeField]
  private ScriptableEnemy[] enemies;

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

      // FIXME: Hard-ending wave
      IEnumerator ForceEndWaveTask()
      {
        yield return new WaitForSeconds(11f);
        Debug.Log("TESTING: Force ending wave...");
        foreach (var enemy in waveEnemies)
        {
          Destroy(enemy.gameObject);
        }
      }
      StartCoroutine(ForceEndWaveTask());
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

    enemyLeft--;
    var enemy = Instantiate(enemies[GameManager.Instance.Wave - 1].enemyPrefab, spawnPoint, Quaternion.identity);
    waveEnemies.Add(enemy);

    if (enemyLeft > 0)
    {
      timer = SPAWN_INTERVAL;
    }
    else
    {
      timer = 0;
    }
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
}
