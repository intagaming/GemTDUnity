using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tile : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler, IDragHandler
{
  [SerializeField]
  private Color _baseColor, _offsetColor;
  [SerializeField]
  private GameObject _highlight;
  private int _x;
  private int _y;

  public static Tile MouseDownTile;

  public void Init(int x, int y)
  {
    bool isOffset = (x + y) % 2 == 1;
    var spriteRenderer = GetComponent<SpriteRenderer>();
    spriteRenderer.color = isOffset ? _offsetColor : _baseColor;

    this._x = x;
    this._y = y;
  }

  public void OnPointerDown(PointerEventData eventData)
  {
    MouseDownTile = this;
  }

  public void OnPointerUp(PointerEventData eventData)
  {
    if (MouseDownTile == this)
    {
      BuildPhaseManager.Instance.PlaceGem(_x, _y);
    }
  }

  public void OnPointerEnter(PointerEventData eventData)
  {
    _highlight.SetActive(true);
  }

  public void OnPointerExit(PointerEventData eventData)
  {
    _highlight.SetActive(false);
  }

  public void OnDrag(PointerEventData eventData)
  {
    MouseDownTile = null;
  }
}
