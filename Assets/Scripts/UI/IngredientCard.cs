using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IngredientCard : MonoBehaviour
{
  [SerializeField]
  public ScriptableTower Tower;
  [SerializeField]
  private TextMeshProUGUI _towerNameText;
  [SerializeField]
  private TextMeshProUGUI _towerQuantityText;
  [SerializeField]
  private Image _background;
  [SerializeField]
  private Color _availableTextColor;
  [SerializeField]
  private Color _availableBackgroundColor;
  [SerializeField]
  private Color _unavailableTextColor;
  [SerializeField]
  private Color _unavailableBackgroundColor;

  void Awake()
  {
    _towerNameText.text = Tower.name;

    GridManager.OnGridChange += HandleGridChange;
  }

  void OnDestroy()
  {
    GridManager.OnGridChange -= HandleGridChange;
  }

  public int TowerQuantity
  {
    get
    {
      return GridManager.Instance.GridTowers.Count(tower => tower.TowerBlueprint == Tower);
    }
  }

  private void HandleGridChange(Vector2 pos, GridImmobileEntity entity)
  {
    UpdateInfo();
  }

  public void UpdateInfo()
  {
    var quantity = TowerQuantity;
    _towerQuantityText.text = "You have: " + TowerQuantity;
    _background.color = quantity > 0 ? _availableBackgroundColor : _unavailableBackgroundColor;
    _towerNameText.color = quantity > 0 ? _availableTextColor : _unavailableTextColor;
    _towerQuantityText.color = quantity > 0 ? _availableTextColor : _unavailableTextColor;
  }
}
