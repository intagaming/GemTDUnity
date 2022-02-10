using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
  [SerializeField]
  private int width, height;
  [SerializeField]
  private Tile tilePrefab;
  [SerializeField]
  private new Camera camera;
  [SerializeField]
  private GameObject invisibleWallPrefab;
  [SerializeField]
  private Stone stonePrefab;
  [SerializeField]
  private Transform gridTilesParent;
  [SerializeField]
  private Transform invisibleWallsParent;
  [SerializeField]
  private Transform immobileEntitiesParent;

  private Dictionary<Vector2, Tile> tiles;

  // This stores Stones and Towers and such.
  private Dictionary<Vector2, GridImmobileEntity> immobileEntities;

  private static GridManager instance;
  public static GridManager Instance { get { return instance; } }

  void Awake()
  {
    instance = this;
  }

  // Start is called before the first frame update
  void Start()
  {
    GenerateTiles();

    immobileEntities = new Dictionary<Vector2, GridImmobileEntity>();

    GenerateWallsAndStones();
  }

  // Returns the object if placed successfully; null if the tile is occupied.
  public T Place<T>(T immobileEntityPrefab, int x, int y, Transform parent) where T : GridImmobileEntity
  {
    var key = new Vector2(x, y);
    if (immobileEntities.ContainsKey(key))
    {
      print("Tile occupied.");
      return null;
    }
    var entityInstance = Instantiate(immobileEntityPrefab, new Vector2(x, y), Quaternion.identity, parent);
    immobileEntities[key] = entityInstance;
    return entityInstance;
  }

  public Stone PlaceStone(int x, int y)
  {
    return Place(stonePrefab, x, y, immobileEntitiesParent);
  }

  public T PlaceImmobileEntity<T>(T immobileEntityPrefab, int x, int y) where T : GridImmobileEntity
  {
    return Place(immobileEntityPrefab, x, y, immobileEntitiesParent);
  }

  private void GenerateTiles()
  {
    tiles = new Dictionary<Vector2, Tile>();

    for (int x = 0; x < width; x++)
    {
      for (int y = 0; y < height; y++)
      {
        var spawnedTile = Instantiate(tilePrefab, new Vector3(x, y), Quaternion.identity, gridTilesParent);
        spawnedTile.name = $"Tile {x} {y}";
        spawnedTile.Init(x, y);
        tiles[new Vector2(x, y)] = spawnedTile;
      }
    }

    camera.transform.position = new Vector3((float)width / 2 - 0.5f, (float)height / 2 - 0.5f, -10);
  }

  private void PlaceInvisibleWall(int x, int y)
  {
    Instantiate(invisibleWallPrefab, new Vector2(x, y), Quaternion.identity, invisibleWallsParent);
  }

  private void GenerateWallsAndStones()
  {
    // 4 edge walls
    for (int x = 0; x < width; x++)
    {
      PlaceInvisibleWall(x, -1);
      PlaceInvisibleWall(x, height);
    }
    for (int y = 0; y < height; y++)
    {
      PlaceInvisibleWall(-1, y);
      PlaceInvisibleWall(width, y);
    }

    // Default stones
    for (int i = 0; i <= 8; i++)
    {
      // Skip the middle position for the checkpoint
      if (i == 4) continue;
      // Left 8 stones
      PlaceStone(i, 18);
      // Right 8 stones
      PlaceStone(width - 1 - i, 18);
      // Top 8 stones
      PlaceStone(18, height - 1 - i);
      // Bottom 8 stones
      PlaceStone(18, i);
    }

    AstarPath.active.Scan();
  }

  public bool DestroyEntity(int x, int y)
  {
    var key = new Vector2(x, y);
    if (!immobileEntities.ContainsKey(key)) return false;

    Destroy(immobileEntities[key].gameObject);
    immobileEntities.Remove(key);
    return true;
  }
}
