using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

public class CombineTowerHUDManager : MonoBehaviour
{
  [SerializeField]
  private RectTransform _combineBoard;
  [SerializeField]
  private Transform _cardContainer;
  [SerializeField]
  private CombineCard _combineCardPrefab;

  private ObjectPool<CombineCard> _combineCardPool;

  void Start()
  {
    _combineCardPool = new ObjectPool<CombineCard>(() =>
    {
      return Instantiate(_combineCardPrefab);
    }, combineCard =>
    {
      combineCard.gameObject.SetActive(true);
    }, combineCard =>
    {
      combineCard.gameObject.SetActive(false);
    }, combineCard =>
    {
      Destroy(combineCard.gameObject);
    }, false, 4, 100);

    HUDManager.OnSelectImmobileEntity += HandleSelectImmobileEntity;
    GridManager.OnGridChange += HandleGridChange;
  }

  void OnDestroy()
  {
    HUDManager.OnSelectImmobileEntity -= HandleSelectImmobileEntity;
    GridManager.OnGridChange -= HandleGridChange;
  }

  private void HandleSelectImmobileEntity(GridImmobileEntity entity)
  {
    ShowCombineFor(entity);
  }

  private void ShowCombineFor(GridImmobileEntity entity)
  {
    var lastCards = _cardContainer.GetComponentsInChildren<CombineCard>();
    if (lastCards.Count() > 0)
    {
      foreach (var lastCard in lastCards)
      {
        _combineCardPool.Release(lastCard);
      }
      _combineBoard.gameObject.SetActive(false);
    }

    var tower = entity != null ? entity.GetComponent<BaseTower>() : null;
    if (tower == null)
    {
      return;
    }
    // Don't allow combining for towers other than this wave's towers in Build phase.
    // Also don't show when not finished building.
    var pos = tower.GetGridPosition();
    if (GameManager.Instance.State == GameState.Building && (
        BuildPhaseManager.Instance.GemsToPlace > 0 || !BuildPhaseManager.Instance.IsBuiltGem(pos.x, pos.y)
        ))
    {
      return;
    }

    var combinables = CombineManager.Instance.GetCombinableAdvancedTowersFrom(tower.TowerBlueprint);
    if (combinables.Any())
    {
      foreach (var combinable in combinables)
      {
        var card = _combineCardPool.Get();
        card.Tower = combinable;
        card.transform.SetParent(_cardContainer, false);
      }
      _combineBoard.gameObject.SetActive(true);
    }
  }

  private void HandleGridChange(Vector2 pos, GridImmobileEntity entity)
  {
    if (HUDManager.Instance.SelectedImmobileEntity != null)
    {
      ShowCombineFor(HUDManager.Instance.SelectedImmobileEntity);
    }
  }
}
