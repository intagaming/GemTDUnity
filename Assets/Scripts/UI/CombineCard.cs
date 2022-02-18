using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CombineCard : MonoBehaviour
{
  [SerializeField]
  public ScriptableAdvancedTower Tower;
  [SerializeField]
  private Image _towerImage;
  [SerializeField]
  private TextMeshProUGUI _towerName;

  void Start()
  {
    _towerImage.sprite = Tower.sprite;
    _towerName.text = Tower.name;
  }

  public void Commit()
  {
    var selectedTower = HUDManager.Instance.SelectedImmobileEntity.GetComponent<BaseTower>();
    if (selectedTower == null) return;
    GridManager.Instance.Combine(selectedTower, Tower);
  }
}
