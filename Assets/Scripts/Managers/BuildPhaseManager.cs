using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildPhaseManager : MonoBehaviour
{
  public const int GEMS_EACH_WAVE = 5;
  int gemsToPlace = 0;

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

      // TODO: SetState to Defense should be called when the player commits to their build instead.
      Debug.Log("Hard-starting the game.");
      GameManager.Instance.SetState(GameState.Defense);
    }
  }
}
