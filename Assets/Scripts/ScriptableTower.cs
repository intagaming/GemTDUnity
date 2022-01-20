using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Tower", menuName = "Scriptable Tower")]
public class ScriptableTower : ScriptableObject
{
  public BaseTower towerPrefab;
  public int damage;
}
