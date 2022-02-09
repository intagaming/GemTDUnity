using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BuildPhaseManager : MonoBehaviour
{
  public const int GEMS_EACH_WAVE = 5;
  private int gemsToPlace = 0;

  private Dictionary<Vector2, GridImmobileEntity> currentWaveGems = new Dictionary<Vector2, GridImmobileEntity>();

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

      // FIXME: Test choose gem
      ChooseGem(16, 18);
    }
  }

  public GemTower PlaceGem(int x, int y)
  {
    if (gemsToPlace <= 0) return null;

    gemsToPlace--;

    ScriptableGemTower gemBlueprint = TowerManager.Instance.GenerateRandomGem();
    var gemTower = GridManager.Instance.Place(gemBlueprint.towerPrefab, x, y);
    if (gemTower == null) return null;

    gemTower.SetTowerBlueprint(gemBlueprint);

    currentWaveGems[new Vector2(x, y)] = gemTower;

    return gemTower;
  }

  public GridImmobileEntity ChooseGem(int x, int y)
  {
    if (gemsToPlace > 0) return null;

    var key = new Vector2(x, y);
    // Find all other gems and turn into stone
    currentWaveGems.Keys.ToList().ForEach(iKey =>
    {
      if (key == iKey) return;

      // Turns into stone
      var gem = currentWaveGems[iKey];
      GridManager.Instance.DestroyEntity((int)iKey.x, (int)iKey.y);
      GridManager.Instance.PlaceStone((int)iKey.x, (int)iKey.y);
    });

    var chosen = currentWaveGems[key];
    currentWaveGems.Clear();

    // Switch state
    GameManager.Instance.SetState(GameState.Defense);

    return chosen;
  }
}
