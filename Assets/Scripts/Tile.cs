using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
  [SerializeField]
  private Color baseColor, offsetColor;
  [SerializeField]
  private SpriteRenderer spriteRenderer;
  [SerializeField]
  private GameObject highlight;

  public void Init(int x, int y)
  {
    bool isOffset = (x + y) % 2 == 1;
    spriteRenderer.color = isOffset ? offsetColor : baseColor;
  }

  void OnMouseEnter()
  {
    highlight.SetActive(true);
  }

  void OnMouseExit()
  {
    highlight.SetActive(false);
  }
}
