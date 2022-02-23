using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

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
    GameManager.OnHealthChanged += HandleHealthChanged;
  }

  void OnDestroy()
  {
    GameManager.OnGameStateChanged -= HandleOnGameStateChanged;
    GridManager.OnGridChange -= HandleGridChange;
    GameManager.OnHealthChanged -= HandleHealthChanged;
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
    var currentActiveStatus = _pauseMenuCanvas.gameObject.activeSelf;
    _pauseMenuCanvas.gameObject.SetActive(!currentActiveStatus);
  }
}
