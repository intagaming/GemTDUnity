using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

public class CombineTowerHUDManager : MonoBehaviour
{
  [SerializeField]
  private GridLayoutGroup _grid;
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
  }

  void OnDestroy()
  {
    HUDManager.OnSelectImmobileEntity -= HandleSelectImmobileEntity;
  }

  private void HandleSelectImmobileEntity(GridImmobileEntity entity)
  {
    var lastCards = _grid.GetComponentsInChildren<CombineCard>();
    foreach (var lastCard in lastCards)
    {
      _combineCardPool.Release(lastCard);
    }

    var tower = entity != null ? entity.GetComponent<BaseTower>() : null;
    if (tower == null)
    {
      return;
    }
    var combinables = CombineManager.Instance.GetCombinableAdvancedTowersFrom(tower.TowerBlueprint);
    foreach (var combinable in combinables)
    {
      var card = _combineCardPool.Get();
      card.Tower = combinable;
      card.transform.SetParent(_grid.transform, false);
    }
  }
}
