using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "Scriptable/Enemy")]
public class ScriptableEnemy : ScriptableObject
{
  public BaseEnemy enemyPrefab;
  public float movementSpeed = 3f;
  public int hp;
}
