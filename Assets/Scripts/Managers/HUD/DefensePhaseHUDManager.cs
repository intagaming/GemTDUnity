using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefensePhaseHUDManager : MonoBehaviour
{
  private static DefensePhaseHUDManager _instance;
  public static DefensePhaseHUDManager Instance { get => _instance; }

  void Awake()
  {
    _instance = this;
  }

  void Start()
  {
    GameManager.OnGameStateChanged += HandleOnGameStateChanged;
  }

  void OnDestroy()
  {
    GameManager.OnGameStateChanged -= HandleOnGameStateChanged;
  }

  private void HandleOnGameStateChanged(GameState state)
  {
    switch (state)
    {
      case GameState.Defense:
        {
          HUDManager.Instance.ChangeObjectiveText("Manage your towers to defeat all enemies!");
          break;
        }
    }
  }

}
