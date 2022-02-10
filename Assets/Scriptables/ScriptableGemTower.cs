using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Gem Tower", menuName = "Scriptable/Gem Tower")]
public class ScriptableGemTower : ScriptableTower
{
  public GemTower towerPrefab;
  public int gemLevel = 0;
}
