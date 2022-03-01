using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;

public class CustomLoader : MonoBehaviour
{
    public Player player;
    [SerializeField] private Tilemap _wallMap, _obstacleMap;
    string levelname;
    public GameObject[] itemsArray;
    public int[] itemsRotations = { 0, 90, 0, 90, -90 };



    public void Awake()
    {
        string json = File.ReadAllText(Controller.choosenLevel);
        ScriptableLevel level = JsonUtility.FromJson<ScriptableLevel>(json);
        if (level == null)
        {
            Debug.LogError($"Level {levelname} does not exist.");
            return;
        }

        player.startvelocity = new Vector2(level.velX, level.velY);

        foreach (var item in level.Items)
        {
            Instantiate(item.ID, item.X, item.Y);
        }

        foreach (var savedTile in level.WallTiles)
        {
            switch (savedTile.Tile.Type)
            {
                case TileType.Grass:
                    SetTile(_wallMap, savedTile);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        foreach (var savedTile in level.ObstacleTiles)
        {
            switch (savedTile.Tile.Type)
            {
                case TileType.Grass:
                    SetTile(_obstacleMap, savedTile);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        void SetTile(Tilemap map, SavedTile tile)
        {
            map.SetTile(new Vector3Int(tile.X, tile.Y, 0), tile.Tile);
        }
    }
    public void Instantiate(int ID, float X, float Y)
    {
        GameObject instantiatedItem = Instantiate(itemsArray[ID - 1], new Vector3(X, Y, 0), Quaternion.Euler(0, 0, itemsRotations[ID - 1]));
        instantiatedItem.SetActive(true);
    }
}
