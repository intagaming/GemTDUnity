using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class HUDManager : MonoBehaviour
{
  [SerializeField]
  private GameObject _selectIndicator;
  [SerializeField]
  private RectTransform _combineLookup;
  [SerializeField]
  private TextMeshProUGUI _waveText;

  [SerializeField]
  private Image _healthForeground;
  [SerializeField]
  private TextMeshProUGUI _healthText;
  [SerializeField]
  private Canvas _pauseMenuCanvas;
  [SerializeField]
  private TowerInfo _towerInfo;

  private static HUDManager _instance;
  public static HUDManager Instance { get => _instance; }
  private bool _isGamePaused;

  void Awake()
  {
    _instance = this;
  }

  void Start()
  {
    GameManager.OnGameStateChanged += HandleOnGameStateChanged;
    GridManager.OnGridChange += HandleGridChange;
    GameManager.OnHealthChanged += HandleHealthChanged;
    OnSelectImmobileEntity += HandleSelectImmobileEntity;
  }

  void OnDestroy()
  {
    GameManager.OnGameStateChanged -= HandleOnGameStateChanged;
    GridManager.OnGridChange -= HandleGridChange;
    GameManager.OnHealthChanged -= HandleHealthChanged;
    OnSelectImmobileEntity -= HandleSelectImmobileEntity;
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

      OnSelectImmobileEntity?.Invoke(value);
    }
  }
  public GridImmobileEntity SelectedImmobileEntity { get => _SelectedImmobileEntity; }

  public static event Action<Vector2> OnSelectPositionChanged;
  public Vector3 SelectedPosition
  {
    get => _selectIndicator.transform.position;
    set
    {
      var v2 = (Vector2)value;
      if (!GridManager.Instance.Tiles.ContainsKey(v2)) return;

      if (value == null) {
        _selectIndicator.SetActive(false);
      } else {
        _selectIndicator.SetActive(true);
        _selectIndicator.transform.position = v2;
      }

      OnSelectPositionChanged?.Invoke(v2);
    }
  }

  public void SelectGridEntity(int x, int y)
  {
    var entity = GridManager.Instance.GetGridImmobileEntity(x, y);
    _SelectedImmobileEntity = entity;
  }

  public void SelectGridPosition(int x, int y)
  {
    SelectGridPosition(new Vector2(x, y));
  }

  public void SelectGridPosition(Vector2 pos)
  {
    SelectedPosition = pos;

    SelectGridEntity((int)pos.x, (int)pos.y);
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
  private TextMeshProUGUI _objectiveText;

  public void ChangeObjectiveText(string text)
  {
    _objectiveText.text = text;
  }


  // General
  private void HandleOnGameStateChanged(GameState state)
  {
    if (state == GameState.Building)
    {
      _waveText.text = $"Wave {GameManager.Instance.Wave}";
    }
  }

  private void HandleGridChange(Vector2 pos, GridImmobileEntity entity)
  {
    if (_SelectedImmobileEntity != null && _SelectedImmobileEntity.GetGridPosition() == pos)
    {
      RefreshSelection();
    }
  }

  public void ToggleCombineLookup()
  {
    var currentActiveStatus = _combineLookup.gameObject.activeSelf;
    _combineLookup.gameObject.SetActive(!currentActiveStatus);
    if (!currentActiveStatus)
    {
      foreach (var resultCard in _combineLookup.GetComponentsInChildren<ResultCard>())
      {
        resultCard.UpdateInfo();
      }
      foreach (var ingredientCard in _combineLookup.GetComponentsInChildren<IngredientCard>())
      {
        ingredientCard.UpdateInfo();
      }
    }
  }

  public void UpdateHealthHUD()
  {
    float percentage = (float)GameManager.Instance.Health / GameManager.INITIAL_HEALTH;
    _healthForeground.fillAmount = percentage;
    _healthText.text = $"{(int)(percentage * 100)}%";
  }

  private void HandleHealthChanged(int health)
  {
    UpdateHealthHUD();
  }

  public void TogglePauseMenu()
  {
    if (_isGamePaused)
    {
      Resume();
    }
    else
    {
      Pause();
    }
    var currentActiveStatus = _pauseMenuCanvas.gameObject.activeSelf;
    _pauseMenuCanvas.gameObject.SetActive(!currentActiveStatus);
  }

  void Pause()
  {
    Time.timeScale = 0f;
    _isGamePaused = true;
  }

  void Resume()
  {
    Time.timeScale = 1f;
    _isGamePaused = false;
  }

  private void HandleSelectImmobileEntity(GridImmobileEntity entity)
  {

    if (entity != null)
    {
      _towerInfo.Entity = entity;
      _towerInfo.gameObject.SetActive(true);

    }
    else
    {
      _towerInfo.gameObject.SetActive(false);
    }
  }


  public void MoveCursor(Vector2 offset)
  {
    SelectedPosition += new Vector3(offset.x, offset.y);
  }
}
