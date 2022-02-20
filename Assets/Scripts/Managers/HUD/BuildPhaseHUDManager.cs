using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildPhaseHUDManager : MonoBehaviour
{
  [SerializeField]
  private Button _chooseGemButton;

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
  }

  void OnDestroy()
  {
    BuildPhaseManager.OnGemPlaced -= HandleOnGemPlaced;
    GameManager.OnGameStateChanged -= HandleOnGameStateChanged;
    HUDManager.OnSelectImmobileEntity -= HandleSelectImmobileEntity;
  }

  private void HandleOnGameStateChanged(GameState state)
  {
    switch (state)
    {
      case GameState.Building:
        {
          HandleOnGemPlaced();
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
    var gemsLeft = BuildPhaseManager.Instance.GemsToPlace;

    HUDManager.Instance.ChangeObjectiveText(gemsLeft > 0 ?
      $"Place {gemsLeft} more gems and select one to keep." :
      "Select one of the gems to keep.");

    if (gemsLeft == 0)
    {
      _chooseGemButton.interactable = true;
    }
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

}
