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


  public static event Action<GameState> OnGameStateChanged;

  private GameState state = GameState.Initializing;
  public GameState State
  {
    get { return state; }
  }
  public void SetState(GameState state)
  {
    if (
      state == this.state ||
      (state == GameState.Building && !(this.state == GameState.Initializing || this.state == GameState.Defense))
      )
    {
      Debug.LogError("Invalid SetState. Currently " + this.state.ToString() + ", but setting " + state.ToString());
      return;
    }

    var prevState = this.state;
    this.state = state;

    HandleStateChanged(prevState, state);

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


  private void HandleStateChanged(GameState prevState, GameState state)
  {
    if (prevState == GameState.Defense && state == GameState.Building)
    {
      wave++;
      print("Wave " + wave);
    }
  }

  void Start()
  {
    SetState(GameState.Building);
    // SetState(GameState.Defense);

    // Initialize checkpoints
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
  Initializing,
  Building,
  Defense
}
