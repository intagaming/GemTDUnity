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
  private Dictionary<Vector2, Tile> tiles;

  // Start is called before the first frame update
  void Start()
  {
    GenerateTiles();
    GenerateWallsAndStones();
  }

  void GenerateTiles()
  {
    tiles = new Dictionary<Vector2, Tile>();

    for (int x = 0; x < width; x++)
    {
      for (int y = 0; y < height; y++)
      {
        var spawnedTile = Instantiate(tilePrefab, new Vector3(x, y), Quaternion.identity);
        spawnedTile.name = $"Tile {x} {y}";
        spawnedTile.Init(x, y);
        tiles[new Vector2(x, y)] = spawnedTile;
      }
    }

    camera.transform.position = new Vector3((float)width / 2 - 0.5f, (float)height / 2 - 0.5f, -10);
  }

  void GenerateWallsAndStones()
  {
    // 4 edge walls
    for (int x = 0; x < width; x++)
    {
      Instantiate(invisibleWallPrefab, new Vector2(x, -1f), Quaternion.identity);
      Instantiate(invisibleWallPrefab, new Vector2(x, height), Quaternion.identity);
    }
    for (int y = 0; y < height; y++)
    {
      Instantiate(invisibleWallPrefab, new Vector2(-1f, y), Quaternion.identity);
      Instantiate(invisibleWallPrefab, new Vector2(width, y), Quaternion.identity);
    }

    // Default stones
    for (int i = 0; i <= 8; i++)
    {
      // Skip the middle position for the checkpoint
      if (i == 4) continue;
      // Left 8 stones
      Instantiate(stonePrefab, new Vector2(i, 18f), Quaternion.identity);
      // Right 8 stones
      Instantiate(stonePrefab, new Vector2(width - 1 - i, 18f), Quaternion.identity);
      // Top 8 stones
      Instantiate(stonePrefab, new Vector2(18f, height - 1 - i), Quaternion.identity);
      // Bottom 8 stones
      Instantiate(stonePrefab, new Vector2(18f, i), Quaternion.identity);
    }

    AstarPath.active.Scan();
  }
}
