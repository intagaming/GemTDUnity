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
  }

  void OnDestroy()
  {
    BuildPhaseManager.OnGemPlaced -= HandleOnGemPlaced;
    GameManager.OnGameStateChanged -= HandleOnGameStateChanged;
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
}
