using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDManager : MonoBehaviour
{
  [SerializeField]
  private GameObject _selectIndicator;
  [SerializeField]
  private Canvas _selectGemOnBuildCanvas;

  private GridImmobileEntity _selectedImmobileEntity;
  private GridImmobileEntity _SelectedImmobileEntity
  {
    get => _selectedImmobileEntity; set
    {
      _selectedImmobileEntity = value;

      if (value != null)
      {
        _selectIndicator.SetActive(true);
        _selectIndicator.transform.position = _selectedImmobileEntity.transform.position;

        if (GameManager.Instance.State == GameState.Building)
        {
          _selectGemOnBuildCanvas.gameObject.SetActive(true);
        }
      }
      else
      {
        _selectIndicator.SetActive(false);
        if (GameManager.Instance.State == GameState.Building)
        {
          _selectGemOnBuildCanvas.gameObject.SetActive(false);
        }
      }
    }
  }

  private static HUDManager _instance;
  public static HUDManager Instance { get => _instance; }

  void Awake()
  {
    _instance = this;
    GameManager.OnGameStateChanged += HandleOnGameStateChanged;
  }

  public void SelectTower(int x, int y)
  {
    var key = new Vector2(x, y);

    if (GameManager.Instance.State == GameState.Building)
    {
      if (BuildPhaseManager.Instance.GemsToPlace > 0 || !BuildPhaseManager.Instance.IsBuiltGem(x, y)) return;
      GemTower gem = (GemTower)GridManager.Instance.GetGridImmobileEntity(x, y);
      _SelectedImmobileEntity = gem;
    }

  }

  private void HandleOnGameStateChanged(GameState state)
  {
    if (state == GameState.Defense)
    {
      _selectGemOnBuildCanvas.gameObject.SetActive(false);
      _SelectedImmobileEntity = null;
    }
  }

  public void HandleChooseGemClick()
  {
    var pos = _selectedImmobileEntity.GetGridPosition();
    BuildPhaseManager.Instance.ChooseGem(pos.x, pos.y);
  }
}
