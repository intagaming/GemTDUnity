using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDManager : MonoBehaviour
{
  [SerializeField]
  private GameObject _selectIndicator;
  [SerializeField]
  private Canvas _selectGemOnBuildCanvas;

  private static HUDManager _instance;
  public static HUDManager Instance { get => _instance; }

  void Awake()
  {
    _instance = this;
    GameManager.OnGameStateChanged += HandleOnGameStateChanged;
  }

  private GridImmobileEntity _selectedImmobileEntity;
  private GridImmobileEntity _SelectedImmobileEntity
  {
    get => _selectedImmobileEntity;
    set
    {
      _selectedImmobileEntity = value;

      if (value != null)
      {
        _selectIndicator.SetActive(true);
        _selectIndicator.transform.position = _selectedImmobileEntity.transform.position;

        if (GameManager.Instance.State == GameState.Building)
        {
          var pos = value.GetGridPosition();
          _selectGemOnBuildCanvas.gameObject.SetActive(BuildPhaseManager.Instance.IsBuiltGem(pos.x, pos.y));
        }
      }
      else
      {
        _selectIndicator.SetActive(false);
        _selectGemOnBuildCanvas.gameObject.SetActive(false);
      }
    }
  }


  public void SelectGridEntity(int x, int y)
  {
    var key = new Vector2(x, y);

    var entity = GridManager.Instance.GetGridImmobileEntity(x, y);
    _SelectedImmobileEntity = entity;
  }

  private void HandleOnGameStateChanged(GameState state)
  {
    _SelectedImmobileEntity = null;
  }

  public void HandleChooseGemClick()
  {
    var pos = _selectedImmobileEntity.GetGridPosition();
    BuildPhaseManager.Instance.ChooseGem(pos.x, pos.y);
  }
}
