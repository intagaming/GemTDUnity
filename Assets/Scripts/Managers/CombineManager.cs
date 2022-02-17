using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CombineManager : MonoBehaviour
{
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
}
