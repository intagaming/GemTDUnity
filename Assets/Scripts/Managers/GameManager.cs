using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
  [SerializeField]
  private GameObject checkpointPrefab;

  private static GameManager instance;

  public static GameManager Instance
  {
    get { return instance; }
  }

  void Awake()
  {
    instance = this;
  }


  public static Action<GameState> OnGameStateChanged;

  private GameState state;
  public GameState State
  {
    get { return state; }
  }
  private void SetState(GameState state)
  {
    this.state = state;
    OnGameStateChanged?.Invoke(state);
  }

  private int wave = 1;
  public int Wave
  {
    get { return wave; }
  }

  private Transform[] checkpoints;
  public Transform[] Checkpoints
  {
    get { return checkpoints; }
  }

  void Start()
  {
    // SetState(GameState.Building);
    SetState(GameState.Defense);

    checkpoints = new Transform[6] {
      Instantiate(checkpointPrefab, new Vector2(4f, 18f), Quaternion.identity).transform,
      Instantiate(checkpointPrefab, new Vector2(32f, 18f), Quaternion.identity).transform,
      Instantiate(checkpointPrefab, new Vector2(32f, 32f), Quaternion.identity).transform,
      Instantiate(checkpointPrefab, new Vector2(18f, 32f), Quaternion.identity).transform,
      Instantiate(checkpointPrefab, new Vector2(18f, 4f), Quaternion.identity).transform,
      Instantiate(checkpointPrefab, new Vector2(32f, 4f), Quaternion.identity).transform,
    };

  }
}

public enum GameState
{
  Building,
  Defense
}
