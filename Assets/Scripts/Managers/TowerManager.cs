using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TowerManager : MonoBehaviour
{
  private static TowerManager _instance;

  void Awake()
  {
    _instance = this;
  }

  public static TowerManager Instance { get => _instance; }


  private ScriptableGemTower[] _gemTowers;
  private ScriptableTower[] _advancedTowers;

  void Start()
  {
    _gemTowers = Resources.LoadAll<ScriptableGemTower>("Gem Towers");
    _advancedTowers = Resources.LoadAll<ScriptableTower>("Advanced Towers");
  }

  public static float[] GetGemChance(int wave)
  {
    if (wave >= 1 && wave <= 3) return new float[] { 1.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f };
    if (wave >= 4 && wave <= 10) return new float[] { 0.8f, 0.2f, 0.0f, 0.0f, 0.0f, 0.0f };
    if (wave >= 11 && wave <= 14) return new float[] { 0.6f, 0.3f, 0.1f, 0.0f, 0.0f, 0.0f };
    if (wave >= 15 && wave <= 20) return new float[] { 0.3f, 0.2f, 0.2f, 0.2f, 0.1f, 0.0f };
    if (wave >= 20 && wave <= 50) return new float[] { 0.1f, 0.1f, 0.2f, 0.3f, 0.2f, 0.1f };
    return new float[] { 1.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f };
  }


  // Generates a random gem with proper random level based on wave
  public ScriptableGemTower GenerateRandomGem()
  {
    int wave = GameManager.Instance.Wave;

    // Determine gem level
    int chosenLevel = 0;
    float[] changes = GetGemChance(wave);
    float roll = Random.Range(0f, 1.0f);
    float cumulativeChance = 0f;
    for (int i = 0; i < changes.Length; i++)
    {
      if (roll >= cumulativeChance && roll <= changes[i])
      {
        chosenLevel = i + 1;
        break;
      }
      cumulativeChance += changes[i];
    }
    if (cumulativeChance > 1.0f) throw new System.Exception("Not proper gem chance distribution.");
    if (chosenLevel == 0) throw new System.Exception("Invalid chosen gem level");

    var levelGemTowers = _gemTowers.Where(t => t.gemLevel == chosenLevel).ToList();
    ScriptableGemTower gem = levelGemTowers[Random.Range(0, levelGemTowers.Count)];

    return gem;
  }
}
