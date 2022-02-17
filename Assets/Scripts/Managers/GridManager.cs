using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GridManager : MonoBehaviour
{
  [SerializeField]
  private int _width, _height;
  [SerializeField]
  private Tile _tilePrefab;
  [SerializeField]
  private Camera _camera;
  [SerializeField]
  private GameObject _invisibleWallPrefab;
  [SerializeField]
  private Stone _stonePrefab;
  [SerializeField]
  private Transform _gridTilesParent;
  [SerializeField]
  private Transform _invisibleWallsParent;
  [SerializeField]
  private Transform _immobileEntitiesParent;
  [SerializeField]
  private bool _drawGizmos;

  public static event Action<Vector2, GridImmobileEntity> OnGridChange;

  private Dictionary<Vector2, Tile> _tiles;

  // This stores Stones and Towers and such.
  private Dictionary<Vector2, GridImmobileEntity> _immobileEntities;
  public IEnumerable<BaseTower> GridTowers
  {
    get => _immobileEntities
      .Where((pair) => pair.Value.GetComponent<BaseTower>() != null)
      .Select(pair => (BaseTower)pair.Value);
  }

  private static GridManager _instance;
  public static GridManager Instance { get { return _instance; } }

  void Awake()
  {
    _instance = this;
  }

  // Start is called before the first frame update
  void Start()
  {
    GenerateTiles();

    _immobileEntities = new Dictionary<Vector2, GridImmobileEntity>();

    GenerateWallsAndStones();
  }

  void OnDrawGizmos()
  {
    if (!_drawGizmos || Application.isPlaying) return;

    for (int x = 0; x < _width; x++)
    {
      for (int y = 0; y < _height; y++)
      {
        Gizmos.DrawWireCube(new Vector3(x, y), new Vector3(1, 1));
      }
    }
  }

  // Returns the object if placed successfully; null if the tile is occupied.
  public T Place<T>(T immobileEntityPrefab, int x, int y, Transform parent, bool replace = false) where T : GridImmobileEntity
  {
    if (IsTileOccupied(x, y))
    {
      if (!replace)
      {
        print("Tile occupied.");
        return null;
      }
      else
      {
        DestroyEntity(x, y, false);
      }
    }
    var key = new Vector2(x, y);
    var entityInstance = Instantiate(immobileEntityPrefab, key, Quaternion.identity, parent);
    _immobileEntities[key] = entityInstance;
    OnGridChange?.Invoke(key, entityInstance);
    return entityInstance;
  }

  public Stone PlaceStone(int x, int y, bool replace = false)
  {
    return Place(_stonePrefab, x, y, _immobileEntitiesParent, replace);
  }

  public T PlaceImmobileEntity<T>(T immobileEntityPrefab, int x, int y, bool replace = false) where T : GridImmobileEntity
  {
    return Place(immobileEntityPrefab, x, y, _immobileEntitiesParent, replace);
  }

  private void GenerateTiles()
  {
    _tiles = new Dictionary<Vector2, Tile>();

    for (int x = 0; x < _width; x++)
    {
      for (int y = 0; y < _height; y++)
      {
        var spawnedTile = Instantiate(_tilePrefab, new Vector3(x, y, 1), Quaternion.identity, _gridTilesParent);
        spawnedTile.name = $"Tile {x} {y}";
        spawnedTile.Init(x, y);
        _tiles[new Vector2(x, y)] = spawnedTile;
      }
    }

    _camera.transform.position = new Vector3((float)_width / 2 - 0.5f, (float)_height / 2 - 0.5f, -10);
  }

  private void PlaceInvisibleWall(int x, int y)
  {
    Instantiate(_invisibleWallPrefab, new Vector2(x, y), Quaternion.identity, _invisibleWallsParent);
  }

  private void GenerateWallsAndStones()
  {
    // 4 edge walls
    for (int x = 0; x < _width; x++)
    {
      PlaceInvisibleWall(x, -1);
      PlaceInvisibleWall(x, _height);
    }
    for (int y = 0; y < _height; y++)
    {
      PlaceInvisibleWall(-1, y);
      PlaceInvisibleWall(_width, y);
    }

    // Default stones
    for (int i = 0; i <= 8; i++)
    {
      // Skip the middle position for the checkpoint
      if (i == 4) continue;
      // Left 8 stones
      PlaceStone(i, 18);
      // Right 8 stones
      PlaceStone(_width - 1 - i, 18);
      // Top 8 stones
      PlaceStone(18, _height - 1 - i);
      // Bottom 8 stones
      PlaceStone(18, i);
    }

    AstarPath.active.Scan();
  }

  public bool DestroyEntity(int x, int y, bool invokeEvent = true)
  {
    if (!IsTileOccupied(x, y)) return false;

    var key = new Vector2(x, y);
    Destroy(_immobileEntities[key].gameObject);
    _immobileEntities.Remove(key);
    if (invokeEvent) OnGridChange?.Invoke(key, null);
    return true;
  }


  public bool IsTileOccupied(int x, int y)
  {
    return _immobileEntities.ContainsKey(new Vector2(x, y));
  }

  public GridImmobileEntity GetGridImmobileEntity(int x, int y)
  {
    if (!IsTileOccupied(x, y)) return null;
    return _immobileEntities[new Vector2(x, y)];
  }

  public void Combine(BaseTower from, ScriptableAdvancedTower to)
  {
    var ingredients = CombineManager.Instance.GetGridIngredientsFor(to, from);
    if (ingredients == null) return;
    foreach (var ingredient in ingredients)
    {
      var x = (int)ingredient.transform.position.x;
      var y = (int)ingredient.transform.position.y;
      if (ingredient == from)
      {
        PlaceImmobileEntity(to.prefab, x, y, true);
      }
      else
      {
        PlaceStone(x, y, true);
      }
    }
  }
}
