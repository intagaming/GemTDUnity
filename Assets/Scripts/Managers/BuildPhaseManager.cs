using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BuildPhaseManager : MonoBehaviour
{
  public const int GEMS_EACH_WAVE = 5;

  [SerializeField]
  private List<ScriptableTower> _towerPlaceCheat;

  [SerializeField]
  private int _gemsToPlace = 0;
  public int GemsToPlace { get => _gemsToPlace; }

  private Dictionary<Vector2, GridImmobileEntity> _currentWaveGems = new Dictionary<Vector2, GridImmobileEntity>();
  private static BuildPhaseManager _instance;
  public static BuildPhaseManager Instance { get { return _instance; } }

  public static event Action OnGemPlaced;

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

  public void PlaceGem(int x, int y)
  {
    if (_gemsToPlace <= 0 || GridManager.Instance.IsTileOccupied(x, y)) return;

    _gemsToPlace--;

    ScriptableTower blueprint;
    if (_towerPlaceCheat.Count > 0)
    {
      blueprint = _towerPlaceCheat[0];
      _towerPlaceCheat.RemoveAt(0);
    }
    else
    {
      blueprint = TowerManager.Instance.GenerateRandomGem();
    }

    BaseTower prefab;
    if (blueprint is ScriptableGemTower)
    {
      prefab = (blueprint as ScriptableGemTower).towerPrefab;
    }
    else if (blueprint is ScriptableAdvancedTower)
    {
      prefab = (blueprint as ScriptableAdvancedTower).prefab;
    }
    else return;

    var spawnedTower = GridManager.Instance.PlaceImmobileEntity(prefab, x, y);
    if (spawnedTower == null) return;

    _currentWaveGems[new Vector2(x, y)] = spawnedTower;

    OnGemPlaced?.Invoke();
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
      GridManager.Instance.PlaceStone((int)iKey.x, (int)iKey.y, true);
    });

    var chosen = _currentWaveGems[key];
    _currentWaveGems.Clear();

    // Switch state
    GameManager.Instance.SetState(GameState.Defense);

    return chosen;
  }

  public bool IsBuiltGem(int x, int y)
  {
    return _currentWaveGems.ContainsKey(new Vector2(x, y));
  }

  public void UseWaveGem(Vector2 pos)
  {
    _currentWaveGems.Remove(pos);
  }

  public void ProceedToDefense()
  {
    // Turn all wave gems into stone
    _currentWaveGems.Values.ToList().ForEach(gem =>
    {
      var pos = gem.GetGridPosition();
      GridManager.Instance.PlaceStone(pos.x, pos.y, true);
    });

    _currentWaveGems.Clear();

    // Switch state
    GameManager.Instance.SetState(GameState.Defense);
  }
}
