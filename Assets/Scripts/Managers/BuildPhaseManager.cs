using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildPhaseManager : MonoBehaviour
{
  public const int GEMS_EACH_WAVE = 5;
  private int gemsToPlace = 0;

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
    if (state == GameState.Building)
    {
      gemsToPlace = GEMS_EACH_WAVE;

      // FIXME: Test place gem
      for (int x = 16; x <= 20; x++)
      {
        PlaceGem(x, 18);
      }

      // FIXME: SetState to Defense should be called when the player commits to their build instead.
      Debug.Log("Hard-starting the game.");
      GameManager.Instance.SetState(GameState.Defense);
    }
  }

  public GemTower PlaceGem(int x, int y)
  {
    gemsToPlace--;

    ScriptableGemTower gemBlueprint = TowerManager.Instance.GenerateRandomGem();
    var gemTower = GridManager.Instance.Place(gemBlueprint.towerPrefab, x, y);
    if (gemTower == null) return null;

    gemTower.SetTowerBlueprint(gemBlueprint);
    return gemTower;
  }
}
