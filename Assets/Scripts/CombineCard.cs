using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CombineCard : MonoBehaviour
{
  [SerializeField]
  public ScriptableTower Tower;
  [SerializeField]
  private Image _towerImage;
  [SerializeField]
  private TextMeshProUGUI _towerName;

  void Start()
  {
    _towerImage.sprite = Tower.sprite;
    _towerName.text = Tower.name;
  }
}
