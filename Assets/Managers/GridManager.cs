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
  private Dictionary<Vector2, Tile> tiles;

  // Start is called before the first frame update
  void Start()
  {
    GenerateTiles();
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
}
