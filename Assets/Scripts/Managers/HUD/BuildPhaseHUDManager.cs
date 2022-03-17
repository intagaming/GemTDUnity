using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildPhaseHUDManager : MonoBehaviour
{
  [SerializeField]
  private Button _chooseGemButton;
  [SerializeField]
  private GameObject _placeGemButtons;

  private static BuildPhaseHUDManager _instance;
  public static BuildPhaseHUDManager Instance { get => _instance; }

  void Awake()
  {
    _instance = this;
  }

  void Start()
  {
    BuildPhaseManager.OnGemPlaced += HandleOnGemPlaced;
    GameManager.OnGameStateChanged += HandleOnGameStateChanged;
    HUDManager.OnSelectImmobileEntity += HandleSelectImmobileEntity;
    HUDManager.OnSelectPositionChanged += HandleSelectPositionChanged;
  }

  void OnDestroy()
  {
    BuildPhaseManager.OnGemPlaced -= HandleOnGemPlaced;
    GameManager.OnGameStateChanged -= HandleOnGameStateChanged;
    HUDManager.OnSelectImmobileEntity -= HandleSelectImmobileEntity;
    HUDManager.OnSelectPositionChanged -= HandleSelectPositionChanged;
  }

  private void HandleOnGameStateChanged(GameState state)
  {
    switch (state)
    {
      case GameState.Building:
        {
          UpdateHUD();
          break;
        }
      case GameState.Defense:
        {
          _chooseGemButton.gameObject.SetActive(false);
          _chooseGemButton.interactable = false;
          break;
        }
    }
  }

  public void HandleOnGemPlaced()
  {
    UpdateHUD();
  }

  public void UpdateHUD()
  {
    var gemsLeft = BuildPhaseManager.Instance.GemsToPlace;

    HUDManager.Instance.ChangeObjectiveText(gemsLeft > 0 ?
      $"Place {gemsLeft} more gems and select one to keep." :
      "Select one of the gems to keep.");

    if (gemsLeft == 0)
    {
      _chooseGemButton.interactable = true;
    }

    UpdatePlaceGemButtonsHUD();
  }

  private void HandleSelectImmobileEntity(GridImmobileEntity entity)
  {
    if (GameManager.Instance.State != GameState.Building) return;

    if (entity != null)
    {
      var pos = entity.GetGridPosition();
      _chooseGemButton.gameObject.SetActive(BuildPhaseManager.Instance.IsBuiltGem(pos.x, pos.y));
    }
    else
    {
      _chooseGemButton.gameObject.SetActive(false);
    }
  }

  public void HandleChooseGemClick()
  {
    var pos = HUDManager.Instance.SelectedImmobileEntity.GetGridPosition();
    BuildPhaseManager.Instance.ChooseGem(pos.x, pos.y);
  }

  public void HandlePlaceButton(int buttonId)
  {
    Vector2 offset;

    switch (buttonId)
    {
      case 0:
        {
          offset = new Vector2(-1, 1);
          break;
        }
      case 1:
        {
          offset = new Vector2(0, 1);
          break;
        }
      case 2:
        {
          offset = new Vector2(1, 1);
          break;
        }
      case 3:
        {
          offset = new Vector2(-1, 0);
          break;
        }
      case 5:
        {
          offset = new Vector2(1, 0);
          break;
        }
      case 6:
        {
          offset = new Vector2(-1, -1);
          break;
        }
      case 7:
        {
          offset = new Vector2(0, -1);
          break;
        }
      case 8:
        {
          offset = new Vector2(1, -1);
          break;
        }
      case 4:
      default:
        {
          offset = new Vector2(0, 0);
          break;
        }
    }

    // Place
    BuildPhaseManager.Instance.PlaceGem(HUDManager.Instance.SelectedPosition);

    // Move cursor
    HUDManager.Instance.MoveCursor(offset);
    UpdatePlaceGemButtonsHUD();
  }

  public void HandleSelectPositionChanged(Vector2 pos)
  {
    UpdatePlaceGemButtonsHUD();
  }

  public void UpdatePlaceGemButtonsHUD() {
    var gemsLeft = BuildPhaseManager.Instance.GemsToPlace;
    var pos = HUDManager.Instance.SelectedPosition;
    // TODO: if HUDManager._selectIndicator not active, then always SetActive false
    _placeGemButtons.SetActive(gemsLeft > 0 && pos != null && !GridManager.Instance.IsTileOccupied(pos));
  }
}

