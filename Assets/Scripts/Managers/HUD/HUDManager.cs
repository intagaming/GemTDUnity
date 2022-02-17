using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class HUDManager : MonoBehaviour
{
  [SerializeField]
  private GameObject _selectIndicator;

  private static HUDManager _instance;
  public static HUDManager Instance { get => _instance; }

  void Awake()
  {
    _instance = this;
  }

  void Start()
  {
    GameManager.OnGameStateChanged += HandleOnGameStateChanged;
    GridManager.OnGridChange += HandleGridChange;
  }

  void OnDestroy()
  {
    GameManager.OnGameStateChanged -= HandleOnGameStateChanged;
    GridManager.OnGridChange -= HandleGridChange;
  }

  // Select immobile entity
  private GridImmobileEntity _selectedImmobileEntity;
  public static event Action<GridImmobileEntity> OnSelectImmobileEntity;
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
      }
      else
      {
        _selectIndicator.SetActive(false);
      }

      OnSelectImmobileEntity?.Invoke(value);
    }
  }
  public GridImmobileEntity SelectedImmobileEntity { get => _SelectedImmobileEntity; }


  public void SelectGridEntity(int x, int y)
  {
    var key = new Vector2(x, y);

    var entity = GridManager.Instance.GetGridImmobileEntity(x, y);
    _SelectedImmobileEntity = entity;
  }

  public void RefreshSelection()
  {
    if (_SelectedImmobileEntity == null) return;
    var pos = _SelectedImmobileEntity.GetGridPosition();
    var entity = GridManager.Instance.GetGridImmobileEntity(pos.x, pos.y);
    _SelectedImmobileEntity = entity;
  }

  // Objective board
  [SerializeField]
  private Canvas _objectiveCanvas;

  public void ChangeObjectiveText(string text)
  {
    _objectiveCanvas.GetComponentInChildren<TextMeshProUGUI>().text = text;
  }


  // General
  private void HandleOnGameStateChanged(GameState state)
  {
  }

  private void HandleGridChange(Vector2 pos, GridImmobileEntity entity)
  {
    if (_SelectedImmobileEntity != null && _SelectedImmobileEntity.GetGridPosition() == pos)
    {
      RefreshSelection();
    }
  }
}
