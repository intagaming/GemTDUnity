using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DefensePhaseHUDManager : MonoBehaviour
{
  [SerializeField]
  private RectTransform _enemiesRemainingBar;
  [SerializeField]
  private Image _enemiesRemainingForeground;
  [SerializeField]
  private TextMeshProUGUI _enemiesRemainingText;

  private static DefensePhaseHUDManager _instance;
  public static DefensePhaseHUDManager Instance { get => _instance; }

  void Awake()
  {
    _instance = this;
  }

  void Start()
  {
    GameManager.OnGameStateChanged += HandleOnGameStateChanged;
    DefensePhaseManager.OnEnemyDie += HandleEnemyDie;
  }

  void OnDestroy()
  {
    GameManager.OnGameStateChanged -= HandleOnGameStateChanged;
    DefensePhaseManager.OnEnemyDie -= HandleEnemyDie;
  }

  private void HandleOnGameStateChanged(GameState state)
  {
    switch (state)
    {
      case GameState.Defense:
        {
          HUDManager.Instance.ChangeObjectiveText("Manage your towers to defeat all enemies!");
          _enemiesRemainingBar.gameObject.SetActive(true);
          UpdateEnemiesRemaining();
          break;
        }
      case GameState.Building:
        {
          _enemiesRemainingBar.gameObject.SetActive(false);
          break;
        }
    }
  }

  private void UpdateEnemiesRemaining()
  {
    var remaining = DefensePhaseManager.Instance.EnemiesRemaining;
    _enemiesRemainingForeground.fillAmount = (float)remaining / DefensePhaseManager.ENEMIES_TO_SPAWN;
    _enemiesRemainingText.text = $"Enemies remaining: {remaining}";
  }

  private void HandleEnemyDie(BaseEnemy enemy)
  {
    UpdateEnemiesRemaining();
  }
}
