using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CombineManager : MonoBehaviour
{
  [SerializeField]
  private GridLayoutGroup _lookupGrid;
  [SerializeField]
  private GameObject _combinationCardPrefab;

  private static CombineManager _instance;

  public static CombineManager Instance
  {
    get { return _instance; }
  }

  private ScriptableAdvancedTower[] _advancedTowers;
  private Dictionary<ScriptableTower, HashSet<ScriptableAdvancedTower>> _ingredientToAdvancedDict;

  void Awake()
  {
    _instance = this;

    _advancedTowers = Resources.LoadAll<ScriptableAdvancedTower>("");

    _ingredientToAdvancedDict = new Dictionary<ScriptableTower, HashSet<ScriptableAdvancedTower>>();
    foreach (var advanced in _advancedTowers)
    {
      foreach (var ingredientTower in advanced.recipe)
      {
        if (!_ingredientToAdvancedDict.ContainsKey(ingredientTower))
        {
          _ingredientToAdvancedDict.Add(ingredientTower, new HashSet<ScriptableAdvancedTower>());
        }
        var set = _ingredientToAdvancedDict[ingredientTower];
        set.Add(advanced);
      }
    }

    PopulateCombinationLookupHUD();
  }

  public HashSet<ScriptableAdvancedTower> GetAdvancedTowersFrom(ScriptableTower tower)
  {
    if (!_ingredientToAdvancedDict.ContainsKey(tower)) return new HashSet<ScriptableAdvancedTower>();
    return _ingredientToAdvancedDict[tower];
  }

  public IEnumerable<ScriptableAdvancedTower> GetCombinableAdvancedTowersFrom(ScriptableTower tower)
  {
    var gridTowers = GridManager.Instance.GridTowers;
    return GetAdvancedTowersFrom(tower).Where((advanced) =>
    {
      var lookup = new Dictionary<ScriptableTower, int>();
      foreach (var ingredient in advanced.recipe)
      {
        if (!lookup.ContainsKey(ingredient))
        {
          lookup.Add(ingredient, 0);
        }
        lookup[ingredient]++;
      }
      foreach (var lookupPair in lookup)
      {
        var found = gridTowers.Count(gridTowerPair => gridTowerPair.TowerBlueprint == lookupPair.Key);
        if (found < lookupPair.Value)
        {
          return false;
        }
      }
      return true;
    });
  }

  public List<BaseTower> GetGridIngredientsFor(ScriptableAdvancedTower tower, BaseTower combineFrom)
  {
    var ingredients = new List<BaseTower>();
    bool baseIngredientIncluded = false; // i.e. is combineFrom included
    foreach (var ingredient in tower.recipe)
    {
      if (!baseIngredientIncluded && combineFrom.TowerBlueprint == ingredient)
      {
        baseIngredientIncluded = true;
        ingredients.Add(combineFrom);
        continue;
      }
      var found = GridManager.Instance.GridTowers.First(gridTowerPair => gridTowerPair.TowerBlueprint == ingredient);
      if (found == null) return null;
      ingredients.Add(found);
    }
    return ingredients;
  }

  public void PopulateCombinationLookupHUD()
  {
    foreach (var advanced in _advancedTowers)
    {
      var card = Instantiate(_combinationCardPrefab, _lookupGrid.transform);
      var cardScript = card.GetComponentInChildren<ResultCard>();
      cardScript.Tower = advanced;
    }
  }
}
