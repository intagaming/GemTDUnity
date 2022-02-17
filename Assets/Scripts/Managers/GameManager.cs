using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
  public const int INITIAL_HEALTH = 50;

  [SerializeField]
  private GameObject _checkpointPrefab;
  [SerializeField]
  private Transform _checkpointsParent;

  private static GameManager _instance;

  public static GameManager Instance
  {
    get { return _instance; }
  }

  void Awake()
  {
    _instance = this;
  }


  public static event Action<GameState> OnGameStateChanged;

  private GameState _state = GameState.Initializing;
  public GameState State
  {
    get { return _state; }
  }
  public void SetState(GameState state)
  {
    if (
      state == this._state ||
      (state == GameState.Building && !(this._state == GameState.Initializing || this._state == GameState.Defense))
      )
    {
      Debug.LogError("Invalid SetState. Currently " + this._state.ToString() + ", but setting " + state.ToString());
      return;
    }

    var prevState = this._state;
    this._state = state;

    HandleStateChanged(prevState, state);

    OnGameStateChanged?.Invoke(state);
  }

  private int _wave = 1;
  public int Wave
  {
    get { return _wave; }
  }

  private int _health;
  public int Health { get => _health; }
  public static event Action<int> OnHealthChanged;

  private Transform[] _checkpoints;
  public Transform[] Checkpoints
  {
    get { return _checkpoints; }
  }


  private void HandleStateChanged(GameState prevState, GameState state)
  {
    if (prevState == GameState.Defense && state == GameState.Building)
    {
      _wave++;
      print("Wave " + _wave);
    }
  }

  void Start()
  {
    SetState(GameState.Building);
    // SetState(GameState.Defense);

    // Initialize checkpoints
    _checkpoints = new Transform[6] {
      Instantiate(_checkpointPrefab, new Vector2(4f, 18f), Quaternion.identity, _checkpointsParent).transform,
      Instantiate(_checkpointPrefab, new Vector2(32f, 18f), Quaternion.identity, _checkpointsParent).transform,
      Instantiate(_checkpointPrefab, new Vector2(32f, 32f), Quaternion.identity, _checkpointsParent).transform,
      Instantiate(_checkpointPrefab, new Vector2(18f, 32f), Quaternion.identity, _checkpointsParent).transform,
      Instantiate(_checkpointPrefab, new Vector2(18f, 4f), Quaternion.identity, _checkpointsParent).transform,
      Instantiate(_checkpointPrefab, new Vector2(32f, 4f), Quaternion.identity, _checkpointsParent).transform,
    };

    _health = INITIAL_HEALTH;
  }

  public void DamageCastle(int damage)
  {
    _health = Math.Max(0, _health - damage);
    OnHealthChanged?.Invoke(_health);
    Debug.Log($"Health: {_health}");
  }
}

public enum GameState
{
  Initializing,
  Building,
  Defense
}
