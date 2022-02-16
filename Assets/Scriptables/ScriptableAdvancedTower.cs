using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Advanced Tower", menuName = "Scriptable/Advanced Tower")]
public class ScriptableAdvancedTower : ScriptableTower
{
  public GameObject prefab;
  public ScriptableTower[] recipe;
}
