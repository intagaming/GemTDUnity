using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CombinationLookupCard : MonoBehaviour
{

  [SerializeField]
  public ScriptableAdvancedTower Tower;
  [SerializeField]
  private Image _towerImage;
  [SerializeField]
  private TextMeshProUGUI _towerName;
  [SerializeField]
  private TextMeshProUGUI _ingredientOne;
  [SerializeField]
  private TextMeshProUGUI _ingredientTwo;
  [SerializeField]
  private TextMeshProUGUI _ingredientThree;

  void Start()
  {
    _towerImage.sprite = Tower.sprite;
    _towerName.text = Tower.name;
    _ingredientOne.text = Tower.recipe[0].name;
    _ingredientTwo.text = Tower.recipe[1].name;
    _ingredientThree.text = Tower.recipe[2].name;
  }
}
