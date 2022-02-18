using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResultCard : MonoBehaviour
{

  [SerializeField]
  public ScriptableAdvancedTower Tower;
  [SerializeField]
  private TextMeshProUGUI _towerName;
  [SerializeField]
  private Image _towerImage;
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
  [SerializeField]
  private IngredientCard _ingredientOne;
  [SerializeField]
  private IngredientCard _ingredientTwo;
  [SerializeField]
  private IngredientCard _ingredientThree;

  void Awake()
  {
    _towerImage.sprite = Tower.sprite;
    _towerName.text = Tower.name;
    _ingredientOne.Tower = Tower.recipe[0];
    _ingredientTwo.Tower = Tower.recipe[1];
    _ingredientThree.Tower = Tower.recipe[2];

    GridManager.OnGridChange += HandleGridChange;
  }

  void OnDestroy()
  {
    GridManager.OnGridChange -= HandleGridChange;
  }

  private void HandleGridChange(Vector2 pos, GridImmobileEntity entity)
  {
    UpdateInfo();
  }

  public bool IsAvailable()
  {
    return _ingredientOne.TowerQuantity > 0 && _ingredientTwo.TowerQuantity > 0 && _ingredientThree.TowerQuantity > 0;
  }

  public void UpdateInfo()
  {
    var available = IsAvailable();
    _background.color = available ? _availableBackgroundColor : _unavailableBackgroundColor;
    _towerName.color = available ? _availableTextColor : _unavailableTextColor;
  }
}
