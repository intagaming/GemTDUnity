using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
  [SerializeField]
  private Color _baseColor, _offsetColor;
  [SerializeField]
  private SpriteRenderer _spriteRenderer;
  [SerializeField]
  private GameObject _highlight;
  private int _x;
  private int _y;

  public void Init(int x, int y)
  {
    bool isOffset = (x + y) % 2 == 1;
    _spriteRenderer.color = isOffset ? _offsetColor : _baseColor;

    this._x = x;
    this._y = y;
  }

  void OnMouseDown()
  {
    BuildPhaseManager.Instance.PlaceGem(_x, _y);
  }

  void OnMouseEnter()
  {
    _highlight.SetActive(true);
  }

  void OnMouseExit()
  {
    _highlight.SetActive(false);
  }
}
