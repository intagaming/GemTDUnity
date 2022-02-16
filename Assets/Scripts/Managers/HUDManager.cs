using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
  }

  void Start()
  {
    GameManager.OnGameStateChanged += HandleOnGameStateChanged;
    BuildPhaseManager.OnGemPlaced += UpdateBuildingPhaseObjective;
  }

  void OnDestroy()
  {
    GameManager.OnGameStateChanged -= HandleOnGameStateChanged;
    BuildPhaseManager.OnGemPlaced -= UpdateBuildingPhaseObjective;
  }

  // Select immobile entity
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

  public void HandleChooseGemClick()
  {
    var pos = _selectedImmobileEntity.GetGridPosition();
    BuildPhaseManager.Instance.ChooseGem(pos.x, pos.y);
  }

  // Objective board
  [SerializeField]
  private Canvas _objectiveCanvas;

  public void ChangeObjectiveText(string text)
  {
    _objectiveCanvas.GetComponentInChildren<TextMeshProUGUI>().text = text;
  }

  public void UpdateBuildingPhaseObjective()
  {
    var gemsLeft = BuildPhaseManager.Instance.GemsToPlace;
    string text;
    if (gemsLeft > 0)
    {
      text = $"Place {gemsLeft} more gems and select one to keep.";
    }
    else
    {
      text = "Select one of the gems to keep.";
    }
    ChangeObjectiveText(text);
  }

  // General
  private void HandleOnGameStateChanged(GameState state)
  {
    _SelectedImmobileEntity = null;

    // Objective text changing
    switch (state)
    {
      case GameState.Building:
        {
          UpdateBuildingPhaseObjective();
          break;
        }
      case GameState.Defense:
        {
          ChangeObjectiveText("Manage your towers to defeat all enemies!");
          break;
        }
    }
  }
}
