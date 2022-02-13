using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BuildPhaseManager : MonoBehaviour
{
  public const int GEMS_EACH_WAVE = 5;
  private int _gemsToPlace = 0;

  private Dictionary<Vector2, GridImmobileEntity> _currentWaveGems = new Dictionary<Vector2, GridImmobileEntity>();
  private static BuildPhaseManager _instance;
  public static BuildPhaseManager Instance { get { return _instance; } }

  void Awake()
  {
    _instance = this;
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
      _gemsToPlace = GEMS_EACH_WAVE;

      /*if (GameManager.Instance.Wave == 1)
      {
        // FIXME: Test place gem
        for (int x = 16; x <= 20; x++)
        {
          PlaceGem(x, 18);
        }

        // FIXME: Test choose gem
        ChooseGem(16, 18);
      }*/
    }
  }

  public GemTower PlaceGem(int x, int y)
  {
    if (_gemsToPlace <= 0) return null;

    _gemsToPlace--;

    ScriptableGemTower gemBlueprint = TowerManager.Instance.GenerateRandomGem();
    var gemTower = GridManager.Instance.PlaceImmobileEntity(gemBlueprint.towerPrefab, x, y);
    if (gemTower == null) return null;

    gemTower.SetTowerBlueprint(gemBlueprint);

    _currentWaveGems[new Vector2(x, y)] = gemTower;

    return gemTower;
  }

  public GridImmobileEntity ChooseGem(int x, int y)
  {
    if (_gemsToPlace > 0) return null;

    var key = new Vector2(x, y);
    // Find all other gems and turn into stone
    _currentWaveGems.Keys.ToList().ForEach(iKey =>
    {
      if (key == iKey) return;

      // Turns into stone
      var gem = _currentWaveGems[iKey];
      GridManager.Instance.DestroyEntity((int)iKey.x, (int)iKey.y);
      GridManager.Instance.PlaceStone((int)iKey.x, (int)iKey.y);
    });

    var chosen = _currentWaveGems[key];
    _currentWaveGems.Clear();

    // Rescan the path
    AstarPath.active.Scan();

    // Switch state
    GameManager.Instance.SetState(GameState.Defense);

    return chosen;
  }
}
